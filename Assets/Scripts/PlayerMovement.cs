using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

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

        staminaBarUI.SetValue(stamina);

        if (Grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if(state != MovementState.sprinting && stamina < 100)
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
        else if(Grounded && Input.GetKey(sprintKey) && stamina > 0)
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

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        /*if (Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!Grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }*/
        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }*/
}
