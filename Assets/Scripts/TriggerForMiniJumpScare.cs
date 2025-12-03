using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForMiniJumpScare : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
