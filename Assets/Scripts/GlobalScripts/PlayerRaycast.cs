using TMPro;
using UnityEngine;

// Assuming InteractableItem, ItemType, DoorLock, FirstAndLastLock, HidingSpot,
// CodeLock, TV207, PCInteractions, and CircuitBreakers scripts exist.

public class PlayerRaycast : MonoBehaviour
{
    public static PlayerRaycast instance;

    [SerializeField] LayerMask interactableLayermask;
    [SerializeField] LayerMask codeLockLayermask;
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
        // This is often temporarily disabled for setup, enable it later if needed.
        playerController.enabled = false;
    }

    void Update()
    {
        // 1. HIDING STATE CHECK
        if (isHiding)
        {
            interactionText.text = "Press 'E' to Exit";
            interactionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StopHiding();
            }
            return;
        }

        // 2. ITEM DROP LOGIC
        if (Input.GetKeyDown(KeyCode.E) && heldItemScript != null)
        {
            Ray checkRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

            // Drop if NOT looking at any interactable
            // NOTE: Using interactionDistance here is correct for checking if we can safely drop.
            if (!Physics.Raycast(checkRay, interactionDistance))
            {
                DropItem();
                return;
            }
        }

        // 3. CONSOLIDATED RAYCAST & INTERACT LOGIC
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            Collider hitCollider = hitInfo.collider;

            // ** (OPTIONAL) Force Field Check **
            // NOTE: This check runs every frame, regardless of what the raycast hits.
            if (IsHoldingItem(ItemType.FirstKey) && Wherefirstkey.instance.disableForceField == false)
            {
                DisableForceField.instance.ActivateForceFieild();
            }
            // **********************************

            // --- A. PRIORITY 1: GENERIC ITEM PICKUP (FIXED) ---
            // MUST run first to allow picking up items before checking other interactions.
            InteractableItem item = hitCollider.GetComponent<InteractableItem>();
            if (item != null && heldItemScript == null)
            {
                if (hitCollider.CompareTag("InternetRoomKey") && DisableForceField.instance.isPlacedAll == false)
                {
                    Debug.Log("InternetKeyRoom is detected");
                    interactionText.text = $"Press 'E' to pick up {item.itemName}";
                    interactionText.gameObject.SetActive(false);
                }
                else
                {
                    interactionText.text = $"Press 'E' to pick up {item.itemName}";
                    interactionText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(item);
                }
                return; // Interaction handled (hover or pickup), stop here.
            }


            // --- B. PRIORITY 2: TAG-BASED INTERACTIONS (Locks, Monitors, Placement Spots) ---

            // 1. Code Lock Logic (Now includes text)
            if (hitCollider.CompareTag("CodeLock"))
            {
                interactionText.text = "Press 'E' to use Keypad";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CodeLock.Instance.OpenInputPanel();
                }
                return;
            }

            // 2. Code Lock For Faculty Room Logic (Now includes text)
            if (hitCollider.CompareTag("CodeLockForFacultyRoom"))
            {
                interactionText.text = "Press 'E' to use TV Keypad";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    TV207.Instance.OpenInputPanel();
                }
                return;
            }

            // 3. Monitor Checks (Now includes text)
            if (hitCollider.CompareTag("Monitor") || hitCollider.CompareTag("Monitor2") ||
                hitCollider.CompareTag("Monitor3") || hitCollider.CompareTag("Monitor4"))
            {
                // Determine which PC instance to check/engage based on the tag
                PCInteractions pcInstance = PCInteractions.Instance; // Assuming PCInteractions handles logic for all 4

                if (hitCollider.CompareTag("Monitor") && !pcInstance.isPC1 ||
                    hitCollider.CompareTag("Monitor2") && !pcInstance.isPC2 ||
                    hitCollider.CompareTag("Monitor3") && !pcInstance.isPC3 ||
                    hitCollider.CompareTag("Monitor4") && !pcInstance.isPC4)
                {
                    interactionText.text = "Press 'E' to use PC";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hitCollider.CompareTag("Monitor")) pcInstance.EngageToPC();
                        else if (hitCollider.CompareTag("Monitor2")) pcInstance.EngageToPC2();
                        else if (hitCollider.CompareTag("Monitor3")) pcInstance.EngageToPC3();
                        else if (hitCollider.CompareTag("Monitor4")) pcInstance.EngageToPC4();

                        interactionText.gameObject.SetActive(false);
                    }
                }
                
                return;
            }


            // 4. Circuit Breaker Checks (Now includes text)
            if (hitCollider.CompareTag("205CircuitBreaker") || hitCollider.CompareTag("201CircuitBreaker") ||
                hitCollider.CompareTag("ICTCircuitBreaker") || hitCollider.CompareTag("ARCircuitBreaker"))
            {
                interactionText.text = "Press 'E' to flip Circuit Breaker";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hitCollider.CompareTag("205CircuitBreaker")) CircuitBreakers.instance.Activate205CB();
                    else if (hitCollider.CompareTag("201CircuitBreaker")) CircuitBreakers.instance.Activate201CB();
                    else if (hitCollider.CompareTag("ICTCircuitBreaker")) CircuitBreakers.instance.ActivateICTCB();
                    else if (hitCollider.CompareTag("ARCircuitBreaker")) CircuitBreakers.instance.ActivateARCB();
                }
                return;
            }


            // 5. Lamesa (Placement Spot) Logic (Now positioned correctly, outside of pickup check)
            if (hitCollider.CompareTag("Lamesa"))
            {
                if (IsHoldingItem(ItemType.FirstIpin))
                {
                    interactionText.text = "Press 'E' to Place 1st Ipin";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        bool placementSuccessful = LamesaNITagoNgirit.instance.TryPlace1stIpin(heldItemScript);

                        if (placementSuccessful)
                        {
                            ConsumeHeldItem();
                        }
                    }
                }
                else if (IsHoldingItem(ItemType.SecondIpin))
                {
                    interactionText.text = "Press 'E' to Place 1st Ipin";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        bool placementSuccessful = LamesaNITagoNgirit.instance.TryPlace2ndIpin(heldItemScript);

                        if (placementSuccessful)
                        {
                            ConsumeHeldItem();
                        }
                    }
                }
                else if (IsHoldingItem(ItemType.ThirdIpin))
                {
                    interactionText.text = "Press 'E' to Place 1st Ipin";
                    interactionText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        bool placementSuccessful = LamesaNITagoNgirit.instance.TryPlace3rdIpin(heldItemScript);

                        if (placementSuccessful)
                        {
                            ConsumeHeldItem();
                        }
                    }
                }
                else
                {
                    interactionText.text = "Requires Ipin of Tago-Ngirit";
                    interactionText.gameObject.SetActive(true);
                }
                return;
            }

            if (IsHoldingItem(ItemType.AntingAnting))
            {
                PlayerRaycast2.instance.ActivateRaycast();
            }
            else
            {
                PlayerRaycast2.instance.DeactivateRaycast();
            }

            if (IsHoldingItem(ItemType.LastKey))
            {
                AntagonistWatchingMechanic.instance.TheLastChase();
            }


                // --- C. PRIORITY 3: COMPONENT-BASED INTERACTIONS (Doors, Hiding Spots) ---

                // 1. Regular Door Check
                DoorLock door = hitCollider.GetComponent<DoorLock>();
            if (door != null)
            {
                if (door.isLocked)
                {
                    string requiredKeyName = door.requiredKeyType.ToString();
                    interactionText.text = $"{door.doorName} is locked. Requires {requiredKeyName}.";
                }
                else
                {
                    interactionText.text = $"Press 'E' to open {door.doorName}";
                }
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.TryUnlock(heldItemScript);
                }
                return;
            }

            SingleDoorLock singledoor = hitCollider.GetComponent<SingleDoorLock>();
            if (singledoor != null)
            {
                if (singledoor.isLocked)
                {
                    string requiredKeyName = singledoor.requiredKeyType.ToString();
                    interactionText.text = $"{singledoor.doorName} is locked. Requires {requiredKeyName}.";
                }
                else
                {
                    interactionText.text = $"Press 'E' to open {singledoor.doorName}";
                }
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    singledoor.TryUnlock(heldItemScript);
                }
                return;
            }

            // 2. FirstAndLastLock Check
            FirstAndLastLock FLLock = hitCollider.GetComponent<FirstAndLastLock>();
            if (FLLock != null)
            {
                if (FLLock.isLocked)
                {
                    string requiredKeyName = FLLock.requiredKeyType.ToString();
                    interactionText.text = $"{FLLock.doorName} is locked. Requires {requiredKeyName}.";
                }
                else
                {
                    interactionText.text = $"Press 'E' to open {FLLock.doorName}";
                }
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    FLLock.TryUnlock(heldItemScript);
                }
                return;
            }

            // 3. Hiding Spot Check
            HidingSpot spot = hitCollider.GetComponent<HidingSpot>();
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

        // 4. DEFAULT STATE (No interaction)
        interactionText.gameObject.SetActive(false);
    } // End of Update()

    public bool IsHoldingItem(ItemType requiredType)
    {
        if (heldItemScript == null)
        {
            return false;
        }
        return heldItemScript.type == requiredType;
    }

    void PickUpItem(InteractableItem itemToPickUp)
    {
        heldItemScript = itemToPickUp;

        Rigidbody rb = heldItemScript.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; }

        Collider col = heldItemScript.GetComponent<Collider>();
        if (col != null) { col.enabled = false; }

        heldItemScript.transform.SetParent(holdPoint);
        heldItemScript.transform.localPosition = Vector3.zero;
        heldItemScript.transform.localRotation = Quaternion.identity;

        interactionText.gameObject.SetActive(false);
    }

    void DropItem()
    {
        heldItemScript.transform.SetParent(null);

        Rigidbody rb = heldItemScript.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }

        Collider col = heldItemScript.GetComponent<Collider>();
        if (col != null) { col.enabled = true; }

        heldItemScript = null;
    }

    void ConsumeHeldItem()
    {
        if (heldItemScript != null)
        {
            // 1. Store a temporary reference to the GameObject the player is holding
            GameObject itemToDestroy = heldItemScript.gameObject;

            // 2. CLEAR THE PLAYER'S HAND FIRST (MOST IMPORTANT STEP)
            heldItemScript = null;

            // 3. Destroy the physical item that was in the player's hand
            Destroy(itemToDestroy);

            // Hide UI
            interactionText.gameObject.SetActive(false);
        }
    }

    void StartHiding(HidingSpot spot)
    {
        AntagonistWatchingMechanic.instance.StopMovingTagoNgiritWhenHide();
        isHiding = true;
        currentSpot = spot;
        

        if (playerMovement != null) { playerMovement.enabled = false; }
        // if (playerCam != null) { playerCam.enabled = false; } // Your original script commented this out

        if (playerController != null)
        {
            playerController.enabled = false;
            playerController.transform.position = spot.hidePoint.position;
            playerController.transform.rotation = spot.hidePoint.rotation;
        }

        if (heldItemScript != null)
        {
            heldItemScript.gameObject.SetActive(false);
        }

        
    }

    void StopHiding()
    {
        AntagonistWatchingMechanic.instance.ResumeMovingTagoNgiritWhenHide(); 
        isHiding = false;

        if (playerController != null)
        {
            playerController.enabled = false;
            playerController.transform.position = currentSpot.exitPoint.position;
            // You may need to enable playerController.enabled = true; here or in a coroutine.
        }

        if (playerMovement != null) { playerMovement.enabled = true; }
        if (playerCam != null) { playerCam.enabled = true; }

        if (heldItemScript != null)
        {
            heldItemScript.gameObject.SetActive(true);
        }

        currentSpot = null;
        interactionText.gameObject.SetActive(false);

        // RE-ENABLE PLAYER CONTROLLER
        if (playerController != null)
        {
            // Enable the controller after it's moved, often requires a brief delay.
            playerController.enabled = true;
        }

        
    }
} // End of class