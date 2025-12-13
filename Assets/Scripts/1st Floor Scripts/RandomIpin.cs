using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomIpin : MonoBehaviour
{
    public static RandomIpin Instance;

    [SerializeField] Transform[] ipinNaTagoNgirit;
    [SerializeField] GameObject Ipin;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActivateRandomIpin();
    }

    public void ActivateRandomIpin()
    {
        int indexRandom = Random.Range(0, ipinNaTagoNgirit.Length);

        Ipin.transform.position = ipinNaTagoNgirit[indexRandom].position;
    }


}
