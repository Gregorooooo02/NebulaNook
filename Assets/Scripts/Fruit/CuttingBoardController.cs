using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
public class CuttingBoardController : MonoBehaviour
{
    [Header("Cutting Board Settings")]
    [SerializeField] private int maxFruitsOnBoard = 1;
    [SerializeField] private bool allowSlicedFruits = false;

    private int currentFruitsOnBoard = 0;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var fruit = other.GetComponent<FruitController>();
        if (fruit == null) return;

        if (currentFruitsOnBoard >= maxFruitsOnBoard)
        {
            PushFruitAway(other.gameObject);
            return;
        }

        currentFruitsOnBoard++;

        TutorialManager.Instance?.NotifyCuttingBoardPlaced();

        FruitTracker tracker = fruit.GetComponent<FruitTracker>();
        if (tracker != null)
        {
            tracker.SetOnCuttingBoard(true);
        }

        fruit.EnableSlicing();
        // Froze the fruit in place
        var rb = fruit.GetComponent<Rigidbody>();
        rb.Sleep();

        var collider = fruit.GetComponent<Collider>();
        if (collider != null) collider.isTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        var fruit = other.GetComponent<FruitController>();
        if (fruit == null) return;

        currentFruitsOnBoard = Mathf.Max(0, currentFruitsOnBoard - 1);

        FruitTracker tracker = fruit.GetComponent<FruitTracker>();
        if (tracker != null)
        {
            tracker.SetOnCuttingBoard(false);
        }

        fruit.DisableSlicing();
        // Unfroze the fruit
        var rb = fruit.GetComponent<Rigidbody>();
        rb.WakeUp();

        var collider = fruit.GetComponent<Collider>();
        if (collider != null) collider.isTrigger = false;
    }

    private void PushFruitAway(GameObject fruit)
    {
        Rigidbody rb = fruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 pushDirection = (fruit.transform.position - transform.position).normalized;
            pushDirection.y = 0.2f; // Lekkie odbicie w górę
            rb.AddForce(pushDirection * 2f, ForceMode.Impulse);
        }
    }

    public void OnFruitSliced(GameObject originalFruit, GameObject slicedFruit)
    {   
        currentFruitsOnBoard = Mathf.Max(0, currentFruitsOnBoard - 1);
        
        if (allowSlicedFruits)
        {
            currentFruitsOnBoard++;
            
            FruitTracker slicedTracker = slicedFruit.GetComponent<FruitTracker>();
            if (slicedTracker == null)
            {
                slicedTracker = slicedFruit.AddComponent<FruitTracker>();
            }
            slicedTracker.SetOnCuttingBoard(true);
        }
    }
}
