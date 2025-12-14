using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TheLastChase : MonoBehaviour
{
    public static TheLastChase instance;

    [SerializeField] GameObject dupTagoNgirit; //this is the only script the can access the duplicate tago ngirit
    [SerializeField] Transform finalChaseSpawnPoint;
    Transform playerTransform;
    NavMeshAgent enemyAgent;

    public bool stopAllTriggers = false;

    private void Awake()
    {
        instance = this;
        enemyAgent = dupTagoNgirit.GetComponent<NavMeshAgent>();

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

    private void Update()
    {   
        if(stopAllTriggers)
        {
            enemyAgent.SetDestination(playerTransform.position);    
        }
        
        
    }

    public void EngageTheLastChase(bool isOk)
    {
        if (isOk)
        {
            dupTagoNgirit.transform.position = finalChaseSpawnPoint.position;
            Debug.Log("All Goods");
            dupTagoNgirit.SetActive(true);

            stopAllTriggers = true;
        }
    }
}
