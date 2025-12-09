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
            if (!Physics.Raycast(checkRay, interactionDistance, interactableLayermask))
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

            // --- A. TAG-BASED INTERACTIONS (Code Locks, Monitors, Circuit Breakers) ---

            if (hitCollider.CompareTag("CodeLock"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CodeLock.Instance.OpenInputPanel();
                }
                return;
            }

            if (hitCollider.CompareTag("CodeLockForFacultyRoom"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TV207.Instance.OpenInputPanel();
                }
                return;
            }

            if (hitCollider.CompareTag("Monitor"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC1 == false)
                {
                    PCInteractions.Instance.EngageToPC();
                }
                return;
            }

            if (hitCollider.CompareTag("Monitor2"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC2 == false)
                {
                    PCInteractions.Instance.EngageToPC2();
                }
                return;
            }

            if (hitCollider.CompareTag("Monitor3"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC3 == false)
                {
                    PCInteractions.Instance.EngageToPC3();
                }
                return;
            }

            if (hitCollider.CompareTag("Monitor4"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC4 == false)
                {
                    PCInteractions.Instance.EngageToPC4();
                }
                return;
            }

            if (hitCollider.CompareTag("205CircuitBreaker"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC4 == false)
                {
                    CircuitBreakers.instance.Activate205CB();
                }
                return;
            }

            if (hitCollider.CompareTag("201CircuitBreaker"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC4 == false)
                {
                    CircuitBreakers.instance.Activate201CB();
                }
                return;
            }

            if (hitCollider.CompareTag("ICTCircuitBreaker"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC4 == false)
                {
                    CircuitBreakers.instance.ActivateICTCB();
                }
                return;
            }

            if (hitCollider.CompareTag("ARCircuitBreaker"))
            {
                if (Input.GetKeyDown(KeyCode.E) && PCInteractions.Instance.isPC4 == false)
                {
                    CircuitBreakers.instance.ActivateARCB();
                }
                return;
            }

            
           
            
            if (IsHoldingItem(ItemType.FirstKey))
            {
                //Debug.Log("Player is holding FirstKey");
                Test.Instance.ActivateCube();
            }
                

                
            

            // --- C. COMPONENT-BASED INTERACTIONS (Items, Doors, Hiding Spots) ---

            // 1. Check if its item to pick up (only if hand is empty)
            InteractableItem item = hitCollider.GetComponent<InteractableItem>();
            if (item != null && heldItemScript == null)
            {
                interactionText.text = $"Press 'E' to pick up {item.itemName}";
                interactionText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(item);
                }
                return;
            }

            // 2. Check if it's a regular Door
            // FIX: Removed redundant .collider
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

            // 3. Check if it's a FirstAndLastLock
            // FIX: Removed redundant .collider
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

            // 4. Check if it's a HIDING SPOT
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

    void StartHiding(HidingSpot spot)
    {
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
    }
} // End of class