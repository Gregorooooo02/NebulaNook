using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GlassSpawner : XRBaseInteractable
{
    [Header("References")]
    [SerializeField] private GameObject glassPrefab;

    [Header("Glass Management")]
    [SerializeField] private int maxGlasses = 5;
    [SerializeField] private bool removeOldestWhenFull = true;
    [SerializeField] private float minFillToProtect = 0.1f;
    [SerializeField] private float removeConsumedDelay = 2f;

    private List<GameObject> spawnedGlasses = new List<GameObject>();

    private void Start()
    {
        FindExistingGlasses();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (CanSpawnNewGlass())
        {
            SpawnNewGlass(args);
        }
        else
        {
            if (removeOldestWhenFull)
            {
                if (RemoveOldestRemovableGlass())
                {
                    SpawnNewGlass(args);
                }
                else
                {
                    Debug.LogWarning("Cannot spawn new glass: No removable glasses available");
                }
            }
        }

        base.OnSelectEntered(args);
    }

    private void SpawnNewGlass(SelectEnterEventArgs args)
    {
        GameObject newGlass = Instantiate(glassPrefab, transform.position, transform.rotation);
        GlassTracker glassTracker = newGlass.GetComponent<GlassTracker>();
        if (glassTracker == null)
        {
            glassTracker = newGlass.AddComponent<GlassTracker>();
        }
        glassTracker.Initialize(this);

        if (!newGlass.CompareTag("Glass"))
        {
            newGlass.tag = "Glass";
        }

        spawnedGlasses.Add(newGlass);

        XRGrabInteractable grabInteractable = newGlass.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            interactionManager.SelectEnter(args.interactorObject, grabInteractable);
        }

        TutorialManager.Instance?.NotifyGlassPicked();
    }

    public void OnGlassDrinkConsumed(GameObject glass)
    {
        Debug.Log($"[GlassSpawner] Drink został wypity z szklanki: {glass.name}");
        StartCoroutine(RemoveConsumedGlassDelayed(glass));
    }

    private System.Collections.IEnumerator RemoveConsumedGlassDelayed(GameObject glass)
    {
        yield return new WaitForSeconds(removeConsumedDelay);
        
        if (glass != null)
        {
            Debug.Log($"[GlassSpawner] Usuwam wypitą szklankę: {glass.name}");
            RemoveGlass(glass);
        }
    }

    public void OnGlassDestroyed(GameObject glass)
    {
        if (spawnedGlasses.Contains(glass))
        {
            spawnedGlasses.Remove(glass);
            Debug.Log($"[GlassSpawner] Szklanka usunięta z listy. Pozostało: {spawnedGlasses.Count}");
        }
    }

    public void RemoveGlass(GameObject glass)
    {
        if (glass == null) return;
        OnGlassDestroyed(glass);
        Destroy(glass);
    }

    private bool CanSpawnNewGlass()
    {
        CleanupNullReferences();
        int activeCount = spawnedGlasses.Count;
        Debug.Log($"[GlassSpawner] Sprawdzam czy można zespawnować: {activeCount}/{maxGlasses}");
        return activeCount < maxGlasses;
    }

    private bool RemoveOldestRemovableGlass()
    {
        CleanupNullReferences();

        for (int i = 0; i < spawnedGlasses.Count; i++)
        {
            GameObject glass = spawnedGlasses[i];
            if (glass != null && CanRemoveGlass(glass))
            {
                RemoveGlass(glass);
                return true;
            }
        }
        return false;
    }

    private bool CanRemoveGlass(GameObject glass)
    {
        GlassFiller glassFiller = glass.GetComponent<GlassFiller>();
        if (glassFiller == null) return true;

        bool hasContent = glassFiller.currentFillAmount >= minFillToProtect;
        bool wasServedButNotConsumed = glassFiller.wasServed && glassFiller.currentFillAmount > 0.1f;
        
        return !hasContent && !wasServedButNotConsumed;
    }

    private void FindExistingGlasses()
    {
        GlassTracker[] existingGlasses = FindObjectsByType<GlassTracker>(FindObjectsSortMode.None);
        foreach (GlassTracker glass in existingGlasses)
        {
            if (!spawnedGlasses.Contains(glass.gameObject))
            {
                spawnedGlasses.Add(glass.gameObject);
                glass.Initialize(this);
            }
        }
    }

    private void CleanupNullReferences()
    {
        int beforeCount = spawnedGlasses.Count;
        spawnedGlasses.RemoveAll(glass => glass == null);
        
        if (beforeCount != spawnedGlasses.Count)
        {
            Debug.Log($"[GlassSpawner] Usunięto {beforeCount - spawnedGlasses.Count} null referencji");
        }
    }
}
