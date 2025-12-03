using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] Transform flashlightHolder;
    private bool FlashlightActive = false;

    [SerializeField] public float maxBatteryLife = 100f;
    [SerializeField] private float drainSpeed = 0.7f;
    public float currentBatteryLife;

    [Header("Sway Settings")]
    [SerializeField] private float smoothAmount = 10f; // How fast it returns to center
    [SerializeField] private float swayAmount = 4f;    // How far it swings out
    private Quaternion initialRotation;

    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        currentBatteryLife = maxBatteryLife;
        
        if (flashlightHolder != null)
        {
            initialRotation = flashlightHolder.localRotation;
        }
        else
        {
            Debug.LogError("Please assign the Flashlight Holder in the Inspector!");
        }
    }

    void Update()
    {
        HandleInput();
        HandleBattery();
        SwayFlashlight();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashlightActive == false)
            {
                if (currentBatteryLife > 0)
                {
                    FlashlightLight.gameObject.SetActive(true);
                    FlashlightActive = true;
                }
            }
            else
            {
                FlashlightLight.gameObject.SetActive(false);
                FlashlightActive = false;
            }
        }
    }

    void HandleBattery()
    {
        if (FlashlightActive == true)
        {
            if (currentBatteryLife > 0)
            {
                currentBatteryLife -= drainSpeed * Time.deltaTime;
                //Debug.Log(currentBatteryLife);
            }
            else
            {
                currentBatteryLife = 0;
                FlashlightLight.gameObject.SetActive(false);
                FlashlightActive = false;
                Debug.Log("Flashlight battery died!");
            }

        }
    }

    void SwayFlashlight()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        // FIX: Invert the directions to create "Lag"
        // If we look UP (Positive Mouse Y), we want the flashlight to point DOWN (Positive X axis)
        // If we look RIGHT (Positive Mouse X), we want the flashlight to point LEFT (Negative Y axis)

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);  // Removed the negative sign
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);    // Added a negative sign

        Quaternion targetRotation = initialRotation * rotationX * rotationY;

        // Smoothly rotate towards the target
        flashlightHolder.localRotation = Quaternion.Slerp(flashlightHolder.localRotation, targetRotation, smoothAmount * Time.deltaTime);
    }
}
