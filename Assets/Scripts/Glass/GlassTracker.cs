using UnityEngine;

public class GlassTracker : MonoBehaviour
{
    private GlassSpawner spawner;
    private bool hasNotifiedSpawner = false;

    public void Initialize(GlassSpawner glassSpawner)
    {
        spawner = glassSpawner;
    }

    public void DestroyGlass()
    {
        if (!hasNotifiedSpawner && spawner != null)
        {
            hasNotifiedSpawner = true;
            if (spawner != null)
            {
                spawner.OnGlassDestroyed(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (spawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            spawner.OnGlassDestroyed(gameObject);
        }
    }
}
