using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BarBoundaryTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnExit = true;
    [SerializeField] private LayerMask bottleLayer = -1;
    [SerializeField] private float graceTimer = 0.5f;

    private void OnTriggerExit(Collider other)
    {
        // Sprawdź czy obiekt jest butelką
        if (IsBottle(other.gameObject))
        {
            var grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                // Jeśli butelka jest trzymana, nie niszcz jej od razu
                Debug.Log($"Butelka {other.name} jest trzymana, nie niszczę jej jeszcze.");
                return;
            }

            Debug.Log($"Butelka {other.name} opuściła obszar baru");
            
            if (destroyOnExit)
            {
                StartCoroutine(DelayedDestroy(other.gameObject, graceTimer));
            }
        }
    }

    private IEnumerator DelayedDestroy(GameObject bottle, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bottle != null)
        {
            var grabInteractable = bottle.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                // Jeśli butelka jest nadal trzymana, nie niszcz jej
                Debug.Log($"Butelka {bottle.name} jest nadal trzymana, nie niszczę jej.");
                yield break;
            }

            Collider bottleCollider = bottle.GetComponent<Collider>();
            if (bottleCollider != null && GetComponent<Collider>().bounds.Intersects(bottleCollider.bounds))
            {
                // Sprawdź czy butelka jest wciąż w obszarze
                Debug.Log($"Butelka {bottle.name} jest nadal w obszarze, nie niszczę jej.");
                yield break;
            }

            BottleTracker bottleTracker = bottle.GetComponent<BottleTracker>();
            if (bottleTracker != null)
            {
                // Jeśli butelka ma tracker, zniszcz ją przez tracker
                bottleTracker.DestroyBottle();
            }
            else
            {
                // W przeciwnym razie zniszcz ją bezpośrednio
                Debug.Log($"Zniszczenie butelki {bottle.name} przez BarBoundaryTrigger.");
                Destroy(bottle);
            }
        }
    }

    private bool IsBottle(GameObject obj)
    {
        // Sprawdź tag
        if (obj.CompareTag("Bottle")) return true;

        // Lub sprawdź layer
        if (((1 << obj.layer) & bottleLayer) != 0) return true;

        // Lub sprawdź komponent
        if (obj.GetComponent<BottleTracker>() != null) return true;

        return false;
    }
}
