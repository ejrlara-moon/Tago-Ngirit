using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MelenitasDev.SoundsGood;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private Sound walking = new Sound(SFX.walkingSFX).SetSpatialSound(false).SetVolume(0.1f);

    [Header("Audio")]
    public float baseStepSpeed = 0.5f; // Time between steps while walking
    public float sprintStepMultiplier = 0.6f; // Makes steps faster (0.6 * 0.5 = 0.3s)
    public float crouchStepMultiplier = 1.5f; // Makes steps slower
    private float footstepTimer = 0;

    [Header("Audio Variation")]
    [Range(0.1f, 1f)] public float minVolume = 0.4f; // Minimum loudness
    [Range(0.1f, 1f)] public float maxVolume = 0.6f; // Maximum loudness
    [Range(0.8f, 1.2f)] public float minPitch = 0.9f; // Slightly lower pitch
    [Range(0.8f, 1.2f)] public float maxPitch = 1.1f;

    [Header("Movement")]
    private float moveSpeed = 7f;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 20f;
    public BarSlider staminaBarUI;


    [Header("FOV")]
    public Camera MainCamera;
    public float sprintFOV;
    private float startFOV;
    public float transitionSpeed;

    /*
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;*/

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool Grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
        startFOV = MainCamera.fieldOfView;

        stamina = maxStamina;
        staminaBarUI.SetMaxValue(maxStamina);

        //readyToJump = true;
        //Debug.Log("Ground LayerMask value is: " + Ground.value);
    }

    private void Update()
    {
        #region
        //Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        //raycast vizualization
        //Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), Color.red);
        #endregion
        // Start the ray 0.1f *above* the player's center to ensure it's outside the ground
        Vector3 rayStart = transform.position + new Vector3(0, 0.1f, 0);

        // Make the ray 0.1f longer to match
        float rayLength = playerHeight * 0.5f + 0.3f;

        Grounded = Physics.Raycast(rayStart, Vector3.down, rayLength, Ground);

        Debug.DrawRay(rayStart, Vector3.down * rayLength, Color.red);

        //Debug.Log("Grounded status: " + Grounded);


        MyInput();
        SpeedControl();
        StateHandler();
        ChangeFOVifSprinting();

        HandleFootsteps();

        staminaBarUI.SetValue(stamina);

        if (Grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (state != MovementState.sprinting && stamina < 100)
        {
            stamina += staminaRegenRate * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        /*
        //Jump
        if(Input.GetKey(jumpKey) && readyToJump && Grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }*/

        //Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    private void StateHandler()
    {
        //Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        //Mode - Sprinting
        else if (Grounded && Input.GetKey(sprintKey) && stamina > 0)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            stamina -= staminaDrainRate * Time.deltaTime;

        }

        //Mode - Walking
        else if (Grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        //calculate Move Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;



        //OnSlope When enabling the jump MUST PUT THIS INSIDE THIS: if(OnSlope()  && !exitingSlope)
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        /*if (Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }*/

        //Turn off the Gravity when on Slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //When enabling the jump MUST PUT THIS INSIDE THIS: if (OnSlope() && !exitingSlope)
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        //limiting speed on ground or air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }


    }

    private void ChangeFOVifSprinting()
    {
        if (state == MovementState.sprinting && stamina > 1)
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, sprintFOV, Time.deltaTime * transitionSpeed);
        }
        else
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, startFOV, Time.deltaTime * transitionSpeed);
        }
    }
    /*
    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }*/

    private bool OnSlope()
    {
        Vector3 rayStart = transform.position + new Vector3(0, 0.1f, 0);
        float rayLength = playerHeight * 0.5f + 0.3f;

        if (Physics.Raycast(rayStart, Vector3.down, out slopeHit, rayLength))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void HandleFootsteps()
    {
        // Only play footsteps if Grounded AND we have input (WASD)
        if (Grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                // 1. Calculate Random Pitch (0.9 to 1.1 makes it sound organic)
                float randomPitch = Random.Range(minPitch, maxPitch);

                // 2. Calculate Random Volume (0.4 to 0.6 makes some steps lighter/heavier)
                float randomVol = Random.Range(minVolume, maxVolume);

                // 3. Apply settings and Play using Method Chaining
                walking
                    .SetPitch(randomPitch)
                    .SetVolume(randomVol)
                    .Play();

                // 4. Reset timer based on current State
                if (state == MovementState.sprinting)
                {
                    footstepTimer = baseStepSpeed * sprintStepMultiplier;
                }
                else if (state == MovementState.crouching)
                {
                    footstepTimer = baseStepSpeed * crouchStepMultiplier;
                }
                else
                {
                    footstepTimer = baseStepSpeed;
                }
            }
        }
        else
        {
            // Reset the timer so the sound plays immediately when you start moving again
            footstepTimer = 0;
        }
    }
}