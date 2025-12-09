using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHolder : MonoBehaviour
{
    public static PointsHolder Instance;

    //[SerializeField] Transform player;

    [Header("Inot na Pc")]
    public Transform monitorStandingPoint1;
    public Transform monitorExitPoint1;
    //public Transform monitorLookTarget1;

    [Header("Panduwa na Pc")]
    public Transform monitorStandingPoint2;
    public Transform monitorExitPoint2;
    //public Transform monitorLookTarget2;

    [Header("Pangatlo na Pc")]
    public Transform monitorStandingPoint3;
    public Transform monitorExitPoint3;
    //public Transform monitorLookTarget3;

    [Header("Pangapat na Pc")]
    public Transform monitorStandingPoint4;
    public Transform monitorExitPoint4;
    //public Transform monitorLookTarget4;

    private void Awake()
    {
        Instance = this;
    }

}
