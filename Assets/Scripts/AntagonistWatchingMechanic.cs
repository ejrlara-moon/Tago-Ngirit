using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AntagonistWatchingMechanic : MonoBehaviour
{
    public static AntagonistWatchingMechanic instance;

    [SerializeField] GameObject Antagonist;

    [Header("SpawnPoints")]
    [SerializeField] Transform[] spawnPoints;

    NavMeshAgent enemyAgent;
    Transform playerTransform;
    bool isChasingPlayer = false;

    //Watching variables
    float maxWatchingTimer = 5f;
    float currentWatchingTimer;
    bool isWatchingTimer;
    int appearChance;
    int jumpScareChance;
    bool jumpScare;


    public GameObject jumpScarePanel;

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


}
