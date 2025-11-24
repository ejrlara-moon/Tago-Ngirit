using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollAI : MonoBehaviour
{
    public Renderer dollRenderer;   // assign in Inspector
    public float moveSpeed = 3f;



    void Update()
    {
        if (dollRenderer.isVisible)
        {
            // Doll is on camera — STOP
            print("true");
            //return;
        } else
        {
print("false");
        }
        // Doll is NOT visible — MOVE
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
        
    }
}
