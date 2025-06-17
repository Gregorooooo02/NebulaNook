using UnityEngine;

public class TasakTracker : MonoBehaviour
{
    private TasakSpawner spawner;
    private bool hasNotifiedSpawner = false;

    public void Initialize(TasakSpawner tasakSpawner)
    {
        spawner = tasakSpawner;
    }

    private void OnDestroy()
    {
        if (spawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            spawner.OnTasakDestroyed(gameObject);
        }
    }

    public void DestroyTasak()
    {
        if (!hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            if (spawner != null)
            {
                spawner.OnTasakDestroyed(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
