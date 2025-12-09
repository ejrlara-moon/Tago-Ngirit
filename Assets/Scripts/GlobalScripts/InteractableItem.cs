using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public string itemName = "Item";

    public ItemType type;

    /*public ItemType requiredItem = ItemType.LibraryKey;

    private void Update()
    {
        if (PlayerRaycast.instance.IsHoldingItem(requiredItem))
        {
            Debug.Log("Player is holding " + requiredItem);
        }
    }*/
}
