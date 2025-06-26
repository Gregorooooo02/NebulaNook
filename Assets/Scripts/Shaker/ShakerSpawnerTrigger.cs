using UnityEngine;

public class ShakerSpawnerTrigger : MonoBehaviour
{
    [SerializeField] private ShakerSpawner shakerSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shaker") {
            shakerSpawner.DestroyShaker();
            shakerSpawner.SpawnShaker();
        }
    }
}
