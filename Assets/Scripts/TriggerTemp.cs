using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTemp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone");
            AntagonistWatchingMechanic.instance.AntaAppear();
        }
    }
}
