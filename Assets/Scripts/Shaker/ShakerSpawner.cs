using UnityEditor;
using UnityEngine;

public class ShakerSpawner : MonoBehaviour
{
    private GameObject instance;
    [SerializeField] private GameObject prefab;

    void OnTriggerExit(Collider other)
    {
        print("fortnite");
    }

    void Start()
    {
        SpawnShaker();
    }

    public void SpawnShaker()
    {
        if (!instance)
        {
            instance = Instantiate(prefab, transform);
        }
    }

    public void DestroyShaker()
    {
        if (instance)
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("Shaker")) {
                Destroy(gameObject);
            }
            Destroy(instance);
            instance = null;
        }
    }
}
