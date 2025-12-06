using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeLock : MonoBehaviour
{
    public static CodeLock Instance;

    public GameObject inputPanel;
    public TMP_InputField codeInput;
    string correctCode = "1234";

    public DoorORGates TargetGate;
    public DoorORGates TargetGate2;

    public bool isLocked = true;

    Rigidbody rb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputPanel.SetActive(false);
            //Time.timeScale = 1.0f;
        }
    }

    public void TryUnlockCode()
    {
        if (!isLocked)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputPanel.SetActive(false);
            
            return;
        }

        string playerText = codeInput.text;

        if (playerText == correctCode)
        {
            isLocked = false;

            OpenDoorCode();
            TargetGate.DoorisUnlocked();
            TargetGate2.DoorisUnlocked();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong code");

            codeInput.text = "";
        }
    }

    public void OpenInputPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    void OpenDoorCode()
    {
        rb.isKinematic = false;
    }

}
