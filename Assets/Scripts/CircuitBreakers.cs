using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircuitBreakers : MonoBehaviour
{
    public static CircuitBreakers instance;

    [SerializeField] GameObject PC205Panel;
    [SerializeField] GameObject PCICTPanel;
    [SerializeField] GameObject PCARPanel;
    [SerializeField] GameObject PC201Panel;


    private void Awake()
    {
        instance = this;
        PC205Panel.SetActive(false);
        PC201Panel.SetActive(false);
        PCICTPanel.SetActive(false);
        PCARPanel.SetActive(false);
    }

    public void Activate205CB()
    {
        PC205Panel.SetActive(true);
    }

    public void Activate201CB()
    {
        PC201Panel.SetActive(true);
    }

    public void ActivateICTCB()
    {
        PCICTPanel.SetActive(true);
    }

    public void ActivateARCB()
    {
        PCARPanel.SetActive(true);
    }
}
