using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistWatchingMechanic : MonoBehaviour
{
    public static AntagonistWatchingMechanic instance;

    [SerializeField] GameObject Antagonist;
    float antagonistSpeedPanggulat = 50f;
    float antagonistAcelPanggulat = 35f;


    [Header("SpawnPoints")]
    [SerializeField] Transform[] spawnPoints;
    

    NavMeshAgent enemyAgent;
    Transform playerTransform;
    public bool isChasingPlayer = false;

    //Watching variables
    float maxWatchingTimer = 5f;
    float currentWatchingTimer;
    public bool isWatchingTimer;
    int appearChance;
    int jumpScareChance;
    bool jumpScare;
    public bool isPlayerHiding;
    private Coroutine stopAndHideCoroutine;

    public GameObject jumpScarePanel;

    [Header("Panggulat Variables")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("MiniJumpScares Variables")]
    bool miniJumpScare = false;

    private bool isPlayerCurrentlyLooking = false;

    void Awake()
    {
        instance = this;
        Antagonist.SetActive(false);
        jumpScarePanel.SetActive(false);

        enemyAgent = Antagonist.GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player NOT found! Make sure your Player has the tag 'Player'.");
        }

    }

    void Update()
    {
        if (isChasingPlayer && playerTransform != null && Antagonist.activeSelf)
        {
            enemyAgent.SetDestination(playerTransform.position);

        }

        if (isWatchingTimer == true && currentWatchingTimer > 0)
        {
            currentWatchingTimer -= Time.deltaTime;
            //Debug.Log("TimeRemaining: " + currentWatchingTimer);
        }
        else if (currentWatchingTimer <= 0)
        {
            currentWatchingTimer = 0;
            if (isWatchingTimer == true && PlayerRaycast.instance.isHiding == false && jumpScare == false)
            {
                isWatchingTimer = false;

                ChasePlayer();


            }
            else if (PlayerRaycast.instance.isHiding == true && isWatchingTimer == true && jumpScare == true)
            {
                jumpScare = false;
                isWatchingTimer = false;

                jumpScareChance = Random.Range(1, 101);
                if (jumpScareChance <= 100)
                {
                    jumpScarePanel.SetActive(true);

                }
            }

        }
    }

    public void AntaAppear()
    {
        isChasingPlayer = false;

        if (isWatchingTimer || Antagonist.activeSelf)
        {
            return; // Do nothing
        }

        appearChance = Random.Range(1, 101);

        if (appearChance <= 100)
        {
            ChooseWhereSpawn();
            isWatchingTimer = true;
            currentWatchingTimer = maxWatchingTimer;
            jumpScare = false;

        }
        else if (appearChance >= 101)
        {
            //This is false alarm
            isWatchingTimer = true;
            currentWatchingTimer = maxWatchingTimer;
            jumpScare = true;

            //must have a indicator that this is a false alarm

        }
    }

    void ChasePlayer()
    {

        Debug.Log("Tago-Ngirit will chase the player");


        isChasingPlayer = true;


        if (enemyAgent != null)
        {
            enemyAgent.isStopped = false;
        }

    }


    void ChooseWhereSpawn()
    {
        Antagonist.SetActive(true);

        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Transform randomSpawnPoint = spawnPoints[spawnIndex];

        //Antagonist.transform.position = randomSpawnPoint.position;

        if (enemyAgent != null)
        {
            enemyAgent.Warp(randomSpawnPoint.position);
        }
        else
        {
            Antagonist.transform.position = randomSpawnPoint.position;
        }
    }

    public void PanggulatEngage()
    {
        Antagonist.transform.position = pointA.position;
        enemyAgent.speed = antagonistSpeedPanggulat;
        enemyAgent.acceleration = antagonistAcelPanggulat;
        Antagonist.SetActive(true);

        enemyAgent.SetDestination(pointB.position);
    }

    public void GameOver()
    {
        jumpScarePanel.SetActive(true);

        if (enemyAgent == null)
        {
            enemyAgent.isStopped = true;
        }

        
    }

    public void StopMovingTagoNgiritWhenHide()
    {
        // Start the stop and hide logic
        if (enemyAgent != null && enemyAgent.isActiveAndEnabled)
        {
            enemyAgent.isStopped = true;
        }

        isPlayerHiding = true;

        // Save the coroutine reference so it can be stopped later
        stopAndHideCoroutine = StartCoroutine(DoDelayedHide());
    }

    private IEnumerator DoDelayedHide()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(1f);

        // After the wait, check if the state is still 'hiding' before hiding the enemy
        if (isPlayerHiding)
        {
            Antagonist.SetActive(false);
        }
    }

    public void ResumeMovingTagoNgiritWhenHide()
    {
        // 1. **Crucial:** Stop the delayed action if it's currently waiting.
        if (stopAndHideCoroutine != null)
        {
            StopCoroutine(stopAndHideCoroutine);
            stopAndHideCoroutine = null; // Clear the reference
        }

        if (enemyAgent != null && enemyAgent.isActiveAndEnabled)
        {
            // This line caused the error when the Antagonist GameObject was deactivated.
            enemyAgent.isStopped = false;
        }
        // 2. Reset the state and resume movement
        isPlayerHiding = false;
        isWatchingTimer = false;
        isChasingPlayer = false;
        //Antagonist.SetActive(true); // Ensure the antagonist is active again
    }

    public void StopTagoNgiritWhenLook(bool islooked)
    {
        if (islooked)
        {
            if (!isPlayerHiding)
            {
                if (enemyAgent != null && enemyAgent.isActiveAndEnabled)
                {
                    Debug.Log("stop tago ngirit");
                    enemyAgent.isStopped = true;
                }
            }
        }
        else
        {
            if (!isPlayerHiding)
            {
                if (enemyAgent != null && enemyAgent.isActiveAndEnabled)
                {
                    Debug.Log("Resume tago ngirit");
                    enemyAgent.isStopped = false;
                }
            }
        }
    }
}
