using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BarBoundaryTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnExit = true;
    [SerializeField] private LayerMask bottleLayer = -1;
    [SerializeField] private LayerMask glassLayer = -1;
    [SerializeField] private float graceTimer = 0.5f;

    void OnTriggerExit(Collider other)
    {
        if (IsBottle(other.gameObject))
        {
            HandleBottleExit(other);
        }
        else if (IsGlass(other.gameObject))
        {
            HandleGlassExit(other);
        }
        else if (IsFruit(other.gameObject))
        {
            HandleFruitExit(other);
        }
    }

    private void HandleBottleExit(Collider other)
    {
        XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            Debug.Log($"Butelka {other.name} jest chwytana - ignoruję");
            return;
        }

        Debug.Log($"Butelka {other.name} opuściła obszar baru");

        if (destroyOnExit)
        {
            StartCoroutine(DelayedDestroyBottle(other.gameObject, graceTimer));
        }
    }

    private void HandleGlassExit(Collider other)
    {
        XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            Debug.Log($"Szklanka {other.name} jest chwytana - ignoruję");
            return;
        }

        GlassFiller filler = other.GetComponent<GlassFiller>();
        if (filler != null)
        {
            if (filler.currentFillAmount > 0.1f)
            {
                Debug.Log($"Szklanka {other.name} ma ciecz - wydłużam czas grace");
                StartCoroutine(DelayedDestroyGlass(other.gameObject, graceTimer * 3));
                return;
            }

            if (filler.wasServed)
            {
                Debug.Log($"Szklanka {other.name} została podana - wydłużam czas grace");
                StartCoroutine(DelayedDestroyGlass(other.gameObject, graceTimer * 5));
                return;
            }
        }

        Debug.Log($"Szklanka {other.name} opuściła obszar baru");

        if (destroyOnExit)
        {
            StartCoroutine(DelayedDestroyGlass(other.gameObject, graceTimer));
        }
    }

    private void HandleFruitExit(Collider other)
    {
        XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            Debug.Log($"Owoc {other.name} jest chwytany - ignoruję");
            return;
        }

        // Sprawdź czy owoc jest na desce do krojenia
        FruitTracker tracker = other.GetComponent<FruitTracker>();
        if (tracker != null && tracker.IsOnCuttingBoard())
        {
            Debug.Log($"Owoc {other.name} jest na desce - nie usuwam");
            return;
        }

        Debug.Log($"Owoc {other.name} opuścił obszar baru");

        if (destroyOnExit)
        {
            StartCoroutine(DelayedDestroyFruit(other.gameObject, graceTimer));
        }
    }

    private IEnumerator DelayedDestroyBottle(GameObject bottle, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bottle != null)
        {
            XRGrabInteractable grabInteractable = bottle.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                Debug.Log($"Butelka {bottle.name} jest nadal chwytana - nie niszczę");
                yield break;
            }

            Collider bottleCollider = bottle.GetComponent<Collider>();
            if (bottleCollider != null && GetComponent<Collider>().bounds.Intersects(bottleCollider.bounds))
            {
                Debug.Log($"Butelka {bottle.name} wróciła do obszaru baru - nie niszczę");
                yield break;
            }

            BottleTracker tracker = bottle.GetComponent<BottleTracker>();
            if (tracker != null)
            {
                tracker.DestroyBottle();
            }
            else
            {
                Destroy(bottle);
            }
        }
    }

    private IEnumerator DelayedDestroyGlass(GameObject glass, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (glass != null)
        {
            XRGrabInteractable grabInteractable = glass.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                Debug.Log($"Szklanka {glass.name} jest nadal chwytana - nie niszczę");
                yield break;
            }

            Collider glassCollider = glass.GetComponent<Collider>();
            if (glassCollider != null && GetComponent<Collider>().bounds.Intersects(glassCollider.bounds))
            {
                Debug.Log($"Szklanka {glass.name} wróciła do obszaru baru - nie niszczę");
                yield break;
            }

            GlassFiller filler = glass.GetComponent<GlassFiller>();
            if (filler != null && (filler.currentFillAmount > 0.1f || filler.wasServed))
            {
                yield break;
            }

            GlassTracker tracker = glass.GetComponent<GlassTracker>();
            if (tracker != null)
            {
                tracker.DestroyGlass();
            }
            else
            {
                Destroy(glass);
            }
        }
    }

    private IEnumerator DelayedDestroyFruit(GameObject fruit, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (fruit != null)
        {
            // Sprawdź ponownie czy można usunąć
            XRGrabInteractable grabInteractable = fruit.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                yield break;
            }

            FruitTracker tracker = fruit.GetComponent<FruitTracker>();
            if (tracker != null && tracker.IsOnCuttingBoard())
            {
                yield break;
            }

            Debug.Log($"Niszczę owoc {fruit.name}");
            
            if (tracker != null)
            {
                tracker.DestroyFruit();
            }
            else
            {
                Destroy(fruit);
            }
        }
    }

    private bool IsBottle(GameObject obj)
    {
        if (obj.CompareTag("Bottle")) return true;
        if (((1 << obj.layer) & bottleLayer) != 0) return true;
        if (obj.GetComponent<BottleTracker>() != null) return true;
        return false;
    }

    private bool IsGlass(GameObject obj)
    {
        if (obj.CompareTag("Glass")) return true;
        if (((1 << obj.layer) & glassLayer) != 0) return true;
        if (obj.GetComponent<GlassTracker>() != null) return true;
        return false;
    }
    
    private bool IsFruit(GameObject obj)
    {
        if (obj.CompareTag("Fruit")) return true;
        if (obj.GetComponent<FruitTracker>() != null) return true;
        if (obj.GetComponent<FruitController>() != null) return true;
        return false;
    }
}