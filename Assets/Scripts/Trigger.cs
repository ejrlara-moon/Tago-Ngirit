using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && AntagonistWatchingMechanic.instance.isWatchingTimer == false  && AntagonistWatchingMechanic.instance.isChasingPlayer == false)
        {
            Debug.Log("Player entered trigger zone");
            AntagonistWatchingMechanic.instance.AntaAppear();
        }
    }
}
