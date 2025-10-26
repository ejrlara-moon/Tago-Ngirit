using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashlightActive = false;

    [SerializeField] public float maxBatteryLife = 100f;
    [SerializeField] private float drainSpeed = 0.7f;
    public float currentBatteryLife;

    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        currentBatteryLife = maxBatteryLife;
    }

    void Update()
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

        if (FlashlightActive == true)
        {
            if (currentBatteryLife > 0)
            {
                currentBatteryLife -= drainSpeed * Time.deltaTime;
                Debug.Log(currentBatteryLife);
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
}
