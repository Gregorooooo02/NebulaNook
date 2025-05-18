using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
public class CuttingBoardController : MonoBehaviour
{
    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var fruit = other.GetComponent<FruitController>();
        if (fruit == null) return;

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
        fruit.DisableSlicing();
        // Unfroze the fruit
        var rb = fruit.GetComponent<Rigidbody>();
        rb.WakeUp();

        var collider = fruit.GetComponent<Collider>();
        if (collider != null) collider.isTrigger = false;
    }
}
