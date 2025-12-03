using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeLock : MonoBehaviour
{
    public static CodeLock Instance;

    public GameObject inputPanel;
    public TMP_InputField codeInput;
    [SerializeField] string correctCode;
    int[] randomCode = new int[4];
    [SerializeField] TextMeshPro code1, code2, code3, code4;
    

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

        GenerateRandomCodes();
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

    void GenerateRandomCodes()
    {
        for (int i = 0; i < randomCode.Length; i++) {

            randomCode[i] = Random.Range(0, 10);
        }

        correctCode = string.Join("",randomCode);
        Debug.Log("Random Code: " + correctCode);

        code1.text = randomCode[0].ToString();
        code2.text = randomCode[1].ToString();
        code3.text = randomCode[2].ToString();
        code4.text = randomCode[3].ToString();
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
