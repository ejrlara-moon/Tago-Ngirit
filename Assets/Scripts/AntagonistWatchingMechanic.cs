using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntagonistWatchingMechanic : MonoBehaviour
{
    public static AntagonistWatchingMechanic instance;

    [SerializeField] bool isWatching;
    [SerializeField] GameObject Antagonist;
    [SerializeField] float WatchingTimer = 5f;
    bool isWatchingTimer;

    int appearChance;

    void Awake()
    {
        instance = this;
        Antagonist.SetActive(false);
    }

    void Update()
    {
        if (isWatchingTimer == true && WatchingTimer > 0)
        {
            WatchingTimer -= Time.deltaTime;
            Debug.Log("TimeRemaining: " + WatchingTimer);
        }
        else if(WatchingTimer <= 0)
        {
            WatchingTimer = 0;
            if (isWatchingTimer == true)
            {
                isWatchingTimer = false;
                ChasePlayer();
            }
        }
    }

    public void AntaAppear()
    {
        appearChance = Random.Range(1, 101);    

        if (appearChance <= 100)
        {
            isWatchingTimer = true;
            Update();
            //Debug.Log("Hindi ito false alarm at May timer igdi");
            
        }
    }

    void ChasePlayer()
    {
        Antagonist.SetActive(true);
        Debug.Log("Hahabolin ka ni Tago-Ngirit");
    }


}
