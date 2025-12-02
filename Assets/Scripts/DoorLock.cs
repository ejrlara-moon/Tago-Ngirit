using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    // 1. Choose which key unlocks this door in the Inspector
    public ItemType requiredKeyType;
    public DoorORGates TargetGate;
    public DoorORGates TargetGate2;

    public string doorName = "Locked Door";
    public bool isLocked = true;
    Rigidbody rb;

    int stoneLife = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    // 2. This function will be called by our player
    public void TryUnlock(InteractableItem heldItem)
    {
        if (!isLocked)
        {
            TargetGate.DoorisUnlocked();
            TargetGate2 .DoorisUnlocked();
            OpenDoor();
            return;
        }

        // 3. Check if the player is holding an item AND if it's the right type
        if (heldItem != null && heldItem.type == requiredKeyType)
        {
            // --- SUCCESS! ---
            Debug.Log($"Unlocked {doorName} with {heldItem.itemName}!");
            isLocked = false;

            if(heldItem.itemName == "Stone")
            {
                Debug.Log("Stone is held");
            }

            // This is just an example. You could also play an animation.
            //gameObject.SetActive(false); // "Opens" the door by disabling it

            // We return 'true' to tell the player to consume the key
            // (But we'll add that logic in the next step)
        }
        else
        {
            // --- FAIL! ---
            Debug.Log($"{doorName} is locked. You need the {requiredKeyType}.");
        }
    }

    void OpenDoor()
    {
        Debug.Log($"Opening {doorName}...");
       // gameObject.SetActive(false); // "Opens" the door
        rb.isKinematic = false;
    }
}
