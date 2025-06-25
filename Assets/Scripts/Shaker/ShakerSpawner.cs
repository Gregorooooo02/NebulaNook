using UnityEngine;

public class ShakerSpawner : MonoBehaviour
{
    private GameObject shakerInstance;
    [SerializeField] private GameObject shakerPrefab;

    void Start()
    {
        spawnShaker();
    }

    void Update()
    {

    }

    private void spawnShaker()
    {
        shakerInstance = Instantiate(shakerPrefab, transform);
    }

    private void destroyShaker()
    {
        Destroy(shakerInstance);
    }
}
