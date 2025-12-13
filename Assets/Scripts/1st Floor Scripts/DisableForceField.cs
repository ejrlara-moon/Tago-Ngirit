using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableForceField : MonoBehaviour
{
    public static DisableForceField instance;
    public bool isPlacedAll;
    [SerializeField] GameObject ForceField;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (LamesaNITagoNgirit.instance.IsIpin1Placed() &&

                LamesaNITagoNgirit.instance.IsIpin2Placed() &&

                LamesaNITagoNgirit.instance.IsIpin3Placed() && isPlacedAll == false)
                
        {

            Debug.Log("All Ipin placed! Puzzle solved!");
            isPlacedAll = true;
            ActivateForceFieild();

            // OpenDoor();

        }

    }

    public void ActivateForceFieild()
    {
        if(isPlacedAll == false)
        {
            ForceField.SetActive(true);
        }
        else
        {
            ForceField.SetActive(false);
        }
        
    }
}
