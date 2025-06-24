using UnityEngine;

public class GlassTracker : MonoBehaviour
{
    private GlassSpawner spawner;
    private bool hasNotifiedSpawner = false;
    private bool drinkConsumed = false;

    [Header("Fruit Objects")]
    [SerializeField] private GameObject jumpyHologram;
    [SerializeField] private GameObject explosiveHologram;
    [SerializeField] private GameObject jumpySlice;
    [SerializeField] private GameObject explosiveSlice;

    public GlassType glassType = GlassType.COCKTAIL;
    public FruitType attachedFruitType = FruitType.NONE;
    private GameObject currentHologram;

    private void Start()
    {
        if (jumpyHologram) jumpyHologram.SetActive(false);
        if (explosiveHologram) explosiveHologram.SetActive(false);
        if (jumpySlice) jumpySlice.SetActive(false);
        if (explosiveSlice) explosiveSlice.SetActive(false);
    }

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

    public bool HasFruitAttached()
    {
        return attachedFruitType != FruitType.NONE;
    }

    public GameObject ShowFruitHologram(FruitType fruitType)
    {
        HideFruitHologram();

        switch (fruitType)
        {
            case FruitType.JUMPY:
                if (jumpyHologram)
                {
                    jumpyHologram.SetActive(true);
                    currentHologram = jumpyHologram;
                }
                break;
            case FruitType.EXPLOSIVE:
                if (explosiveHologram)
                {
                    explosiveHologram.SetActive(true);
                    currentHologram = explosiveHologram;
                }
                break;
        }

        return currentHologram;
    }

    public void HideFruitHologram()
    {
        if (jumpyHologram) jumpyHologram.SetActive(false);
        if (explosiveHologram) explosiveHologram.SetActive(false);
        currentHologram = null;
    }

    public void ActivateFruit(FruitType fruitType)
    {
        HideFruitHologram();
        attachedFruitType = fruitType;

        TutorialManager.Instance?.NotifyFruitMounted();

        switch (fruitType)
        {
            case FruitType.JUMPY:
                if (jumpySlice) jumpySlice.SetActive(true);
                break;
            case FruitType.EXPLOSIVE:
                if (explosiveSlice) explosiveSlice.SetActive(true);
                break;
        }
    }

    public void RemoveAttachedFruit()
    {
        if (jumpySlice) jumpySlice.SetActive(false);
        if (explosiveSlice) explosiveSlice.SetActive(false);
        attachedFruitType = FruitType.NONE;
    }
}
