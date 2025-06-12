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
    }

    private bool CanSpawnNewGlass()
    {
        CleanupNullReferences();
        return spawnedGlasses.Count < maxGlasses;
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

        return glassFiller.currentFillAmount < minFillToProtect && !glassFiller.wasServed;
    }

    public void OnGlassDestroyed(GameObject glass)
    {
        if (spawnedGlasses.Contains(glass))
        {
            spawnedGlasses.Remove(glass);
        }
    }

    public void RemoveGlass(GameObject glass)
    {
        if (glass == null) return;
        OnGlassDestroyed(glass);
        Destroy(glass);
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
        spawnedGlasses.RemoveAll(glass => glass == null);
    }

    void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUI.Label(new Rect(10, 95, 300, 20), $"Szklanki: {spawnedGlasses.Count}/{maxGlasses}");
        }
    }
}
