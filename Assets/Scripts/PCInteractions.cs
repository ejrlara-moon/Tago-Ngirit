using UnityEngine;
using TMPro;
using System.Collections;

public class PCInteractions : MonoBehaviour
{
    public static PCInteractions Instance;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform player;

    public bool isPC1, isPC2, isPC3, isPC4;

    //Button Random
    [SerializeField] GameObject randomButton;

    [SerializeField] GameObject answerInputGO;
    [SerializeField] TMP_InputField AnswerInput;

    [Header("Random Bugtong Bugtong")]
    [SerializeField] GameObject[] bugtongBugtong;
    int bugtongPicker;
    //BA0 = BugtongAnswer0
    bool BA0, BA1, BA2, BA3, BA4, BA5, BA6, BA7, BA8, BA9;
    public static bool c0 ,c1, c2, c3, c4, c5, c6, c7, c8, c9;

    string correctAnswer0 = "bola";
    string correctAnswer1 = "paa";
    string correctAnswer2 = "bag";
    string correctAnswer3 = "papel";
    string correctAnswer4 = "lapis";
    string correctAnswer5 = "sapatos";
    string correctAnswer6 = "personal computer";
    string correctAnswer7 = "lapis at papel";
    string correctAnswer8 = "samgyup sa magsaysay";
    string correctAnswer9 = "tinpay at kape";

    [SerializeField] GameObject greenPanel;
    
    public static int currentCorrectCount = 0;

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
        if (isPC1 && Input.GetKeyUp(KeyCode.Escape))
        {
            ExitPC();
        }
        else if (isPC2 && Input.GetKeyUp(KeyCode.Escape))
        {
            ExitPC2();
        }
        else if (isPC3 && Input.GetKeyUp(KeyCode.Escape))
        {
            ExitPC3();
        }
        else if (isPC4 && Input.GetKeyUp(KeyCode.Escape))
        {
            ExitPC4();
        }




    }

    public void EngageToPC()
    {
        isPC1 = true;

        if (playerMovement != null) playerMovement.enabled = false;

        player.transform.position = PointsHolder.Instance.monitorStandingPoint1.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EngageToPC2()
    {
        isPC2 = true;

        if (playerMovement != null) playerMovement.enabled = false;

        player.transform.position = PointsHolder.Instance.monitorStandingPoint2.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EngageToPC3()
    {
        isPC3 = true;

        if (playerMovement != null) playerMovement.enabled = false;

        player.transform.position = PointsHolder.Instance.monitorStandingPoint3.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EngageToPC4()
    {
        isPC4 = true;

        if (playerMovement != null) playerMovement.enabled = false;

        player.transform.position = PointsHolder.Instance.monitorStandingPoint4.position;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitPC()
    {
        isPC1 = false;

        if (playerMovement != null) playerMovement.enabled = true;

        player.transform.position = PointsHolder.Instance.monitorExitPoint1.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void ExitPC2()
    {
        isPC2 = false;

        if (playerMovement != null) playerMovement.enabled = true;

        player.transform.position = PointsHolder.Instance.monitorExitPoint2.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void ExitPC3()
    {
        isPC3 = false;

        if (playerMovement != null) playerMovement.enabled = true;

        player.transform.position = PointsHolder.Instance.monitorExitPoint3.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void ExitPC4()
    {
        isPC4 = false;

        if (playerMovement != null) playerMovement.enabled = true;

        player.transform.position = PointsHolder.Instance.monitorExitPoint4.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Clickme()
    {
        //Debug.Log("Some functions here");

        StartCoroutine(RapidRandom());

        randomButton.SetActive(false);


    }

    IEnumerator RapidRandom()
    {
        for (int i = 0; i < bugtongBugtong.Length; i++)
        {

            bugtongBugtong[i].SetActive(true);

            yield return new WaitForSeconds(0.1f);

            bugtongBugtong[i].SetActive(false);

        }

        RandomPickerofBugtong();
    }

    void RandomPickerofBugtong()
    {
        answerInputGO.SetActive(true);

        bool[] correctFlags = new bool[] { c0, c1, c2, c3, c4, c5, c6, c7, c8, c9 };

        int maxAttempts = 100; // Safety measure to prevent infinite loop
        int attempt = 0;

        do
        {
            // Pick a random index between 0 and 9
            bugtongPicker = Random.Range(0, bugtongBugtong.Length);
            attempt++;

            // Check if all questions are done to prevent infinite loops
            if (attempt > maxAttempts)
            {
                Debug.Log("All Bugtong questions have been answered correctly.");
                // You should probably hide the randomButton/answerInput here
                return;
            }

        } while (correctFlags[bugtongPicker] == true);


        switch (bugtongPicker)
        {

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
            case 5:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA5 = true;
                break;
            case 6:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA6 = true;
                break;
            case 7:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA7 = true;
                break;
            case 8:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA8 = true;
                break;
            case 9:
                bugtongBugtong[bugtongPicker].SetActive(true);
                BA9 = true;
                break;

        }
    }

    public void AnswertheBugtong()
    {
        string playerAnswer = AnswerInput.text;

        if (BA0 && playerAnswer == correctAnswer0)
        {
            c0 = true;
            Debug.Log($"c0 is set to {c0}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA0 = false;
            
        }
        else if (BA1 && playerAnswer == correctAnswer1)
        {
            c1 = true;
            Debug.Log($"c1 is set to {c1}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA1 = false;
            
        }
        else if (BA2 && playerAnswer == correctAnswer2)
        {
            c2 = true;
            Debug.Log($"c2 is set to {c2}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA2 = false;
            
        }
        else if (BA3 && playerAnswer == correctAnswer3)
        {
            c3 = true;
            Debug.Log($"c3 is set to {c3}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA3 = false;
            
        }
        else if (BA4 && playerAnswer == correctAnswer4)
        {
            c4 = true;
            Debug.Log($"c4 is set to {c4}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA4 = false;
            
        }
        else if (BA5 && playerAnswer == correctAnswer5)
        {
            c5 = true;
            Debug.Log($"c5 is set to {c5}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA5 = false;
            
        }
        else if (BA6 && playerAnswer == correctAnswer6)
        {
            c6 = true;
            Debug.Log($"c6 is set to {c6}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA6 = false;
            
        }
        else if (BA7 && playerAnswer == correctAnswer7)
        {
            c7 = true;
            Debug.Log($"c7 is set to {c7}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA7 = false;
            
        }
        else if (BA8 && playerAnswer == correctAnswer8)
        {
            c8 = true;
            Debug.Log($"c8 is set to {c8}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA8 = false;
            
        }
        else if (BA9 && playerAnswer == correctAnswer9)
        {
            c9 = true;
            Debug.Log($"c9 is set to {c9}");
            CheckAtLeast4CorrectAnswers();
            greenPanel.SetActive(true);
            answerInputGO.SetActive(false);
            BA9 = false;
            
        }
        else
        {

            Debug.Log("Wrong Answer");

            AnswerInput.text = "";
        }
    }

    public void CheckAtLeast4CorrectAnswers()
    {
        int calculatedCorrectCount = 0;

        bool[] correctHistoryFlags = new bool[] { c0, c1, c2, c3, c4, c5, c6, c7, c8, c9 };

        foreach (bool hasAnsweredCorrectly in correctHistoryFlags)
        {
            if (hasAnsweredCorrectly)
            {
                calculatedCorrectCount++;
            }
        }

        currentCorrectCount = calculatedCorrectCount;

        Debug.Log("Total Correct Answers: " + currentCorrectCount);

        if (currentCorrectCount >= 4)
        {
            TV207.Instance.WordPicker();
        }
    }
}
