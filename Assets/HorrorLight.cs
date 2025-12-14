using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorLight : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Drag your Spotlight here (or leave empty to use the one on this object).")]
    public Light myLight;

    [Header("Intensity Settings")]
    public float normalIntensity = 1.0f; // The brightness when "ON"
    public float dimIntensity = 0.1f;    // The brightness when it "flickers"

    [Header("Timing (Randomness)")]
    public float minWaitTime = 0.1f;     // Minimum time between flickers
    public float maxWaitTime = 1.5f;     // Maximum time between flickers

    private void Start()
    {
        // If you forgot to drag the light in, grab the one attached to this object
        if (myLight == null)
        {
            myLight = GetComponent<Light>();
        }

        // Start the infinite loop
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true) // Run forever
        {
            // 1. Wait for a random time (Light is stable here)
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            // 2. The Flicker! (Dim the light)
            myLight.intensity = dimIntensity;

            // 3. Wait a tiny fraction of a second (The "glitch")
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

            // 4. Restore light to normal
            myLight.intensity = normalIntensity;
        }
    }
}
