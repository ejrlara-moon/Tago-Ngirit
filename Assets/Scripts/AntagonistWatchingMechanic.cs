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
    float maxWatchingTimer = 5f;
    float currentWatchingTimer;
    bool isWatchingTimer;
    int appearChance;

    void Awake()
    {
        instance = this;
        Antagonist.SetActive(false);
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
            if (isWatchingTimer == true)
            {
                isWatchingTimer = false;
                ChasePlayer();

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

        }
        else if (appearChance >= 51)
        {
            //This is false alarm
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
