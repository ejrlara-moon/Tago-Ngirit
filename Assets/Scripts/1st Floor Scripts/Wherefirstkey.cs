
using UnityEngine;

public class Wherefirstkey : MonoBehaviour
{
    public static Wherefirstkey instance;

    [SerializeField] Transform[] randomFKSP; //FirstKeySpawnPoints
    [SerializeField] GameObject FirstKey;

    public bool disableForceField;

    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SelectRandomSpawnPoint();
    }

    public void SelectRandomSpawnPoint()
    {
        int randomSP = Random.Range(0, 10);

        FirstKey.transform.position = randomFKSP[randomSP].position;
    }

    

}
