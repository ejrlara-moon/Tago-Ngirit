using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public static PlayerRaycast instance;

    [SerializeField] LayerMask interactableLayermask;
    public float interactionDistance = 3f;

    public CharacterController playerController;
    public PlayerCam playerCam;
    public PlayerMovement playerMovement;

    public bool isHiding = false;

    private HidingSpot currentSpot;

    public Transform holdPoint;
    public TextMeshProUGUI interactionText;

    private InteractableItem heldItemScript = null;

    void Awake()
    {
        instance = this;
        playerController.enabled = false;
    }

    void Update()
    {
        if (isHiding)
        {
            interactionText.text = "Press 'E' to Exit";
            interactionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StopHiding();
            }

            // We are hiding, so stop here and don't run any other logic this frame.
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && heldItemScript != null)
        {
            // We can't drop if we're looking at something else
            // So we'll cast a quick, short ray to check
            Ray checkRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

            if (!Physics.Raycast(checkRay, interactionDistance, interactableLayermask))
            {
                DropItem();
                return;
            }
        }

        // --- 2. RAYCAST & PICKUP/INTERACT LOGIC ---
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayermask))
        {
            // We hit something on the Interactable layer.
            // But what is it? An item, a door, or a hiding spot?

            // check if its item
            InteractableItem item = hitInfo.collider.GetComponent<InteractableItem>();
            if (item != null && heldItemScript == null) // We can only pick up if hand is empty
            {
                interactionText.text = $"Press 'E' to pick up {item.itemName}";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(item);
                }
                return;
            }

            DoorLock door = hitInfo.collider.GetComponent<DoorLock>();
            if (door != null)
            {
                // --- HOVER LOGIC ---
                if (door.isLocked)
                {
                    // It's locked. Get the key name from the enum or ItemTypeDefinition.
                    string requiredKeyName = door.requiredKeyType.ToString();
                    interactionText.text = $"{door.doorName} is locked. Requires {requiredKeyName}.";
                }
                else
                {
                    // It's already unlocked.
                    interactionText.text = $"Press 'E' to open {door.doorName}";
                }

                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // try to unlock or open the door when you press 'E'.
                    door.TryUnlock(heldItemScript);
                }
                return; // Stop here
            }

            // --- Check if it's a HIDING SPOT ---
            HidingSpot spot = hitInfo.collider.GetComponent<HidingSpot>();
            if (spot != null)
            {
                interactionText.text = $"Press 'E' to hide in {spot.spotName}";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartHiding(spot);
                }
                return;
            }
        }

        // If player is not looking at anything
        interactionText.gameObject.SetActive(false);
    }


    void PickUpItem(InteractableItem itemToPickUp)
    {
        // Store the script
        heldItemScript = itemToPickUp;

        // Disable physics
        Rigidbody rb = heldItemScript.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = heldItemScript.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Parent and snap to hold point
        heldItemScript.transform.SetParent(holdPoint);
        heldItemScript.transform.localPosition = Vector3.zero;
        heldItemScript.transform.localRotation = Quaternion.identity;

        interactionText.gameObject.SetActive(false);
    }


    void DropItem()
    {
        // Un-parent
        heldItemScript.transform.SetParent(null);

        // Re-enable physics
        Rigidbody rb = heldItemScript.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }

        Collider col = heldItemScript.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Forget the item
        heldItemScript = null;
    }

    void StartHiding(HidingSpot spot)
    {
        isHiding = true;
        currentSpot = spot;

        // 1. This stops WALKING and MOUSE LOOK
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        //if (playerCam != null)
        //{
        //    playerCam.enabled = false;
        //}

        if (playerController != null)
        {
            // Disable the controller to allow teleporting
            playerController.enabled = false;

            // Move the CONTROLLER (the body) to the hide point
            playerController.transform.position = spot.hidePoint.position;
            playerController.transform.rotation = spot.hidePoint.rotation;

        }

        // Hide the held item
        if (heldItemScript != null)
        {
            heldItemScript.gameObject.SetActive(false);
        }
    }

    void StopHiding()
    {
        isHiding = false;

        // Move player BODY to the exit point
        if (playerController != null)
        {
            playerController.enabled = false; // Must be disabled to teleport
            playerController.transform.position = currentSpot.exitPoint.position;
           // playerController.enabled = true; // Re-enables gravity/collision
        }

        // Re-enable WALKING and MOUSE LOOK
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (playerCam != null)
        {
            playerCam.enabled = true;
        }

        // Un-hide the held item
        if (heldItemScript != null)
        {
            heldItemScript.gameObject.SetActive(true);
        }

        currentSpot = null;
        interactionText.gameObject.SetActive(false);
    }
}
