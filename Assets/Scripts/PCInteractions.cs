using UnityEngine;
using TMPro;
using System.Collections;

public class PCInteractions : MonoBehaviour
{
    public static PCInteractions Instance;

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Transform player;
    [SerializeField] Transform standingPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] Transform lookTarget;

    bool isComputing;

    //Button Random
    [SerializeField] GameObject randomButton;

    [Header("Random Bugtong Bugtong")]
    [SerializeField] GameObject[] bugtongBugtong;
    int bugtongPicker;
    //BA0 = BugtongAnswer0
    bool BA0, BA1, BA2, BA3, BA4;

    [SerializeField] GameObject answerInputGO;
    [SerializeField] TMP_InputField AnswerInput;

    string correctAnswer0 = "bola";
    string correctAnswer1 = "paa";
    string correctAnswer2 = "bag";
    string correctAnswer3 = "papel";
    string correctAnswer4 = "lapis";

    [SerializeField] GameObject greenPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        answerInputGO.SetActive(false);
        greenPanel.SetActive(false);
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

        StartCoroutine(RapidRandom());
        
        randomButton.SetActive(false);

        
    }

    IEnumerator RapidRandom()
    {
        for (int i = 0; i < bugtongBugtong.Length; i++) {

            bugtongBugtong[i].SetActive(true);

            yield return new WaitForSeconds(0.1f);

            bugtongBugtong[i].SetActive(false);

        }

        RandomPickerofBugtong();
    }

    void RandomPickerofBugtong()
    {
        answerInputGO.SetActive(true);

        bugtongPicker = Random.Range(0, bugtongBugtong.Length);

        switch (bugtongPicker) { 
        
            case 0:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA0 = true;
                break;
            case 1:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA1 = true;
                break;
            case 2:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA2 = true;
                break;
            case 3:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA3 = true;
                break;
            case 4:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA4 = true;
                break;

        }
    }

    public void AnswertheBugtong()
    {
        string playerAnswer = AnswerInput.text;

        if (BA0 && playerAnswer == correctAnswer0)
        {
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
        }
        else if (BA1 && playerAnswer == correctAnswer1)
        {
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
        }
        else if (BA2 && playerAnswer == correctAnswer2)
        {
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
        }
        else if (BA3 && playerAnswer == correctAnswer3)
        {
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
        }
        else if (BA4 && playerAnswer == correctAnswer4)
        {
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
        }
        else {

            Debug.Log("Wrong Answer");

            AnswerInput.text = "";
        }
    }

}
