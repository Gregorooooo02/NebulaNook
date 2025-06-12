using UnityEngine;

public class PipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipPrefab;
    [SerializeField] private PipOutsideBarZone pipOutsideBarZone;
    public GameObject PipInstance;

    void Start()
    {
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
