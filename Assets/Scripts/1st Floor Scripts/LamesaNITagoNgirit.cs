using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LamesaNITagoNgirit : MonoBehaviour
{
    public static LamesaNITagoNgirit instance;

    // Use a single Transform if all three Ipin place in the same spot,
    // or change to Transform[] Lamesa if they have separate spots.
    // Assuming Lamesa now holds 3 separate Transforms for 3 spots.
    [SerializeField] Transform[] Lamesa;

    // This array holds the 3 hidden, permanent GameObjects for the visuals.
    [SerializeField] GameObject[] IpinVisuals;

    // You removed IpinToHold, which was the source of the error.
    // We don't need 'IpinToHold' because the item being held is passed in as 'heldItem'.

    private const int Index_1st = 0;
    private const int Index_2nd = 1;
    private const int Index_3rd = 2;

    private void Awake()
    {
        instance = this;
    }

    // Helper method to consolidate logic and check bounds
    private bool TryPlaceIpin(InteractableItem heldItem, ItemType requiredType, int index)
    {
        // 1. Basic Check: Is the held item correct?
        if (heldItem == null || heldItem.type != requiredType)
        {
            Debug.Log($"Requires the {requiredType} to be placed here.");
            return false;
        }

        // 2. Safety Check: Are the arrays large enough?
        if (IpinVisuals.Length <= index || Lamesa.Length <= index)
        {
            Debug.LogError($"Setup Error: IpinVisuals or Lamesa array is missing an element for index {index}.");
            return false;
        }

        // Get the static object and the target position
        GameObject visualObject = IpinVisuals[index];
        Transform targetSpot = Lamesa[index];

        // 3. Perform Placement on the STATIC VISUAL OBJECT
        Debug.Log($"Successfully placed the {requiredType}! Activating scene object at {targetSpot.position}");

        // Move and show the static visual object
        visualObject.transform.position = targetSpot.position;
        visualObject.transform.rotation = targetSpot.rotation; // Good practice
        visualObject.SetActive(true);

        // Freeze the visual object's Rigidbody (optional, but good for static puzzle pieces)
        Rigidbody visualRb = visualObject.GetComponent<Rigidbody>();
        if (visualRb != null)
        {
            visualRb.isKinematic = true;
            visualRb.useGravity = false;
        }

        // 4. Return TRUE: This tells PlayerRaycast to call ConsumeHeldItem() 
        // which DESTROYS the item currently held by the player (heldItem.gameObject).
        return true;
    }

    // --- Public methods called by PlayerRaycast ---

    public bool TryPlace1stIpin(InteractableItem heldItem)
    {
        return TryPlaceIpin(heldItem, ItemType.FirstIpin, Index_1st);
    }

    public bool TryPlace2ndIpin(InteractableItem heldItem)
    {
        return TryPlaceIpin(heldItem, ItemType.SecondIpin, Index_2nd); // Line 40 is now here, clean.
    }

    public bool TryPlace3rdIpin(InteractableItem heldItem)
    {
        return TryPlaceIpin(heldItem, ItemType.ThirdIpin, Index_3rd);
    }
}