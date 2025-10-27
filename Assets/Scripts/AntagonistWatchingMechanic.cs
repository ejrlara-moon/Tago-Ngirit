using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistWatchingMechanic : MonoBehaviour
{
    public static AntagonistWatchingMechanic instance;

    [SerializeField] GameObject Antagonist;

    [Header("SpawnPoints")]
    [SerializeField] Transform[] spawnPoints;



    //Watching variables
    float maxWatchingTimer = 10f;
    public float currentWatchingTimer;
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

    }

    void Update()
    {
        if (isWatchingTimer == true && currentWatchingTimer > 0)
        {
            currentWatchingTimer -= Time.deltaTime;
            Debug.Log("TimeRemaining: " + currentWatchingTimer);
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
                jumpScareChance = Random.Range(1, 101);
                jumpScare = false;
                isWatchingTimer = false;
                if (jumpScareChance <= 100)
                {
                    jumpScarePanel.SetActive(true);
                }
            }
        }
    }

    public void AntaAppear()
    {

        if (isWatchingTimer || Antagonist.activeSelf)
        {
            return; // Do nothing
        }

        appearChance = Random.Range(1, 101);

        if (appearChance <= 50)
        {
            ChooseWhereSpawn();
            isWatchingTimer = true;
            currentWatchingTimer = maxWatchingTimer;
            jumpScare = false;

        }
        else if (appearChance >= 51)
        {
            isWatchingTimer = true;
            currentWatchingTimer = maxWatchingTimer;
            jumpScare = true;
            //This is false alarm
            //must have a indicator that this is a false alarm

        }
    }

    void ChasePlayer()
    {

        Debug.Log("Hahabolin ka ni Tago-Ngirit");
    }


    void ChooseWhereSpawn()
    {
        Antagonist.SetActive(true);

        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Transform randomSpawnPoint = spawnPoints[spawnIndex];

        Antagonist.transform.position = randomSpawnPoint.position;
    }
}
