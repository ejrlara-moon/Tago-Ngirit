using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public static Test Instance;

    [SerializeField] GameObject testCube;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateCube()
    {
        testCube.SetActive(true);
    }
}
