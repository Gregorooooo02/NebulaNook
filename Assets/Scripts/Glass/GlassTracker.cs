using UnityEngine;

public class GlassTracker : MonoBehaviour
{
    private GlassSpawner spawner;
    private bool hasNotifiedSpawner = false;
    private bool drinkConsumed = false;

    public void Initialize(GlassSpawner glassSpawner)
    {
        spawner = glassSpawner;

        GlassFiller glassFiller = GetComponent<GlassFiller>();
        if (glassFiller != null)
        {
            InvokeRepeating(nameof(CheckIfDrinkConsumed), 1f, 0.5f);
        }
    }

    private void CheckIfDrinkConsumed()
    {
        if (drinkConsumed) return;

        GlassFiller glassFiller = GetComponent<GlassFiller>();
        if (glassFiller != null)
        {
            bool wasServedAndEmpty = glassFiller.wasServed && glassFiller.currentFillAmount <= 0.1f;

            if (wasServedAndEmpty)
            {
                OnDrinkConsumed();
            }
        }
    }

    public void OnDrinkConsumed()
    {
        if (drinkConsumed) return;

        drinkConsumed = true;
        Invoke(nameof(NotifySpawnerAfterDrinking), 2f);
    }

    private void NotifySpawnerAfterDrinking()
    {
        if (spawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            spawner.OnGlassDrinkConsumed(gameObject);
        }
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

        CancelInvoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (spawner != null && !hasNotifiedSpawner)
        {
            hasNotifiedSpawner = true;
            spawner.OnGlassDestroyed(gameObject);
        }
        CancelInvoke();
    }
}
