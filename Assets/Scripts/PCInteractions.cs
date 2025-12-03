using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PCInteractions : MonoBehaviour
{
    public static PCInteractions Instance;

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Transform player;
    [SerializeField] Transform standingPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] Transform lookTarget;

    bool isComputing;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isComputing && Input.GetKeyUp(KeyCode.Escape))
        {
            ExitPC();
        }
    }

    public void EngageToPC()
    {
        isComputing = true;

        if (playerMovement != null) playerMovement.enabled = false;

        player.transform.position = standingPoint.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitPC()
    {
        isComputing = false;

        if (playerMovement != null) playerMovement.enabled = true;

        player.transform.position = exitPoint.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Clickme()
    {
        Debug.Log("Some functions here");
    }

}
