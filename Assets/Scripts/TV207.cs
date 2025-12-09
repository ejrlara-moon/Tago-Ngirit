using UnityEngine;
using TMPro;

public class TV207 : MonoBehaviour
{
    public static TV207 Instance;

    [SerializeField] GameObject TVPanel;

    [SerializeField] GameObject[] words;

    [Header("Variables for CodeLock for faculty room")]
    [SerializeField] string correctCode;
    public GameObject inputPanel;
    public TMP_InputField codeInput;
    [SerializeField] PlayerMovement playerMovement;
    bool isInputingCode;
    bool isLocked = true;
    [SerializeField] GameObject doorOfSafe;

    private void Awake()
    {
        Instance = this;
        inputPanel.SetActive(false);
    }

    public void WordPicker()
    {
        int wordPicker = Random.Range(0, words.Length);

        switch (wordPicker)
        {
            case 0:
                words[wordPicker].SetActive(true);
                correctCode = "2665";
                break;
            case 1:
                words[wordPicker].SetActive(true);
                correctCode = "3288";
                break;
            case 2:
                words[wordPicker].SetActive(true);
                correctCode = "4663";
                break;
            case 3:
                words[wordPicker].SetActive(true);
                correctCode = "5455";
                break;
            case 4:
                words[wordPicker].SetActive(true);
                correctCode = "6668";
                break;
            case 5:
                words[wordPicker].SetActive(true);
                correctCode = "7264";
                break;
            case 6:
                words[wordPicker].SetActive(true);
                correctCode = "7293";
                break;
            case 7:
                words[wordPicker].SetActive(true);
                correctCode = "7844";
                break;
            case 8:
                words[wordPicker].SetActive(true);
                correctCode = "8744";
                break;
            case 9:
                words[wordPicker].SetActive(true);
                correctCode = "9476";
                break;
        }
    }

    private void Update()
    {
        if(isInputingCode && Input.GetKeyUp(KeyCode.Escape))
        {
            isInputingCode = false;
            inputPanel.SetActive(false);
            if (playerMovement != null) playerMovement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void OpenInputPanel()
    {
        isInputingCode = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputPanel.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = false;
    }

    public void TryUnlockCode()
    {
        if (!isLocked)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputPanel.SetActive(false);
            if (playerMovement != null) playerMovement.enabled = true;

            return;
        }

        string playerText = codeInput.text;

        if (playerText == correctCode)
        {
            isLocked = false;

            OpenDoorCode();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputPanel.SetActive(false);
            if (playerMovement != null) playerMovement.enabled = true;
        }
        else
        {
            Debug.Log("Wrong code");

            codeInput.text = "";
        }
    }

    void OpenDoorCode()
    {
        doorOfSafe.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

}
