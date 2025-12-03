using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPanggulat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){

            Debug.Log("Player entered the trigger for panggulat only");

            AntagonistWatchingMechanic.instance.PanggulatEngage();
        }
    }
}
