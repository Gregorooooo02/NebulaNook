using UnityEngine;

public class BottleTracker : MonoBehaviour
{
    private BottleSpawner spawner;
    private bool hasNotifiedSpawner = false;

    public void Initialize(BottleSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnDestroy()
    {
        if (spawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            spawner.OnBottleDestroyed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BarArea"))
        {
            DestroyBottle();
        }
    }

    public void DestroyBottle()
    {
        if (!hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            if (spawner != null)
            {
                spawner.OnBottleDestroyed();
            }
            Destroy(gameObject);
        }
    }
}
