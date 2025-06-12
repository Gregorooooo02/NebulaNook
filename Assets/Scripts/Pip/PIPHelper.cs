using UnityEngine;

public class PIPHelper : MonoBehaviour
{
    public static PIPHelper Instance;

    public Transform MeteorSpawn;
    public Transform[] CloneSpawningLocations;
 
    private void Start()
    {
        Instance = this;
    }
}
