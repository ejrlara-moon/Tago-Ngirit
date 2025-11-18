using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorORGates : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.isKinematic = true;
    }

    public void DoorisUnlocked()
    {
        rb.isKinematic = false;
    }
}
