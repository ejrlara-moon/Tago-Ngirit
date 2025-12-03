using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public string spotName = "Locker";

    // This is where the player's camera will snap to
    public Transform hidePoint;

    // This is where the player's body will exit to
    public Transform exitPoint;
}
