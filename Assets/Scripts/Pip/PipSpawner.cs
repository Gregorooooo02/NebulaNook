using UnityEngine;

public class PipSpawner : MonoBehaviour
{
    public static PipSpawner Instance;
    [SerializeField] private GameObject pipPrefab;
    public GameObject PipInstance;

    void Start()
    {
        Instance = this;
        SpawnPip();
    }

    public void SpawnPip()
    {
        GameObject pip = Instantiate(pipPrefab, transform);
        PipInstance = pip;
    }

    public void DespawnPip()
    {
        Destroy(pipPrefab);
    }
}
