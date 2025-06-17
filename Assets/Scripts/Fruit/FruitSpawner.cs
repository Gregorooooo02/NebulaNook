using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FruitSpawner : XRBaseInteractable
{
    [Header("References")]
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private FruitType spawnedFruitType = FruitType.JUMPY;

    [Header("Fruit Management")]
    [SerializeField] private int maxFruits = 3;
    [SerializeField] private bool removeOldestWhenFull = true;
    [SerializeField] private float minTimeToProtect = 5f;

    private List<GameObject> spawnedFruits = new List<GameObject>();

    private void Start()
    {
        FindExistingFruits();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (CanSpawnNewFruit())
        {
            SpawnNewFruit(args);
        }
        else
        {
            if (removeOldestWhenFull)
            {
                if (RemoveOldestRemovableFruit())
                {
                    SpawnNewFruit(args);
                }
            }
        }

        base.OnSelectEntered(args);
    }

    private void SpawnNewFruit(SelectEnterEventArgs args)
    {
        GameObject newFruit = Instantiate(fruitPrefab, transform.position, transform.rotation);
        FruitTracker tracker = newFruit.GetComponent<FruitTracker>();
        if (tracker == null)
        {
            tracker = newFruit.AddComponent<FruitTracker>();
        }
        tracker.Initialize(this, spawnedFruitType);

        if (!newFruit.CompareTag("Fruit"))
        {
            newFruit.tag = "Fruit";
        }

        spawnedFruits.Add(newFruit);

        XRGrabInteractable grabInteractable = newFruit.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            interactionManager.SelectEnter(args.interactorObject, grabInteractable);
        }
    }

    private bool CanSpawnNewFruit()
    {
        CleanupNullReferences();
        return spawnedFruits.Count < maxFruits;
    }

    private bool RemoveOldestRemovableFruit()
    {
        CleanupNullReferences();

        for (int i = 0; i < spawnedFruits.Count; i++)
        {
            GameObject fruit = spawnedFruits[i];
            if (fruit != null && CanRemoveFruit(fruit))
            {
                RemoveFruit(fruit);
                return true;
            }
        }
        return false;
    }

    private bool CanRemoveFruit(GameObject fruit)
    {
        FruitTracker tracker = fruit.GetComponent<FruitTracker>();
        if (tracker == null) return true;

        if (tracker.IsOnCuttingBoard())
        {
            Debug.Log($"[FruitSpawner] Owoc {fruit.name} jest na desce - nie usuwam");
            return false;
        }

        if (tracker.GetTimeAlive() < minTimeToProtect)
        {
            return false;
        }

        return true;
    }

    public void OnFruitDestroyed(GameObject fruit)
    {
        if (spawnedFruits.Contains(fruit))
        {
            spawnedFruits.Remove(fruit);
        }
    }

    public void RemoveFruit(GameObject fruit)
    {
        if (fruit == null) return;
        OnFruitDestroyed(fruit);
        Destroy(fruit);
    }

    private void FindExistingFruits()
    {
        FruitTracker[] existingFruits = FindObjectsByType<FruitTracker>(FindObjectsSortMode.None);
        foreach (FruitTracker fruit in existingFruits)
        {
            if (!spawnedFruits.Contains(fruit.gameObject))
            {
                spawnedFruits.Add(fruit.gameObject);
                fruit.Initialize(this, spawnedFruitType);
            }
        }
    }

    private void CleanupNullReferences()
    {
        int beforeCount = spawnedFruits.Count;
        spawnedFruits.RemoveAll(fruit => fruit == null);
    }
}