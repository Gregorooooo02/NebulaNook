using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SliceController : XRGrabInteractable
{
    [Header("Slice Settings")]
    [SerializeField] private string glassTag = "Glass";
    [SerializeField] private float attachDistance = 0.1f;
    [SerializeField] private LayerMask glassLayer = -1;

    private GlassTracker nearbyGlass;
    private FruitTracker fruitTracker;
    private bool isNearGlass = false;
    private GameObject currentHologram;

    protected override void Awake()
    {
        base.Awake();

        fruitTracker = GetComponent<FruitTracker>();
    }

    private void Update()
    {
        if (isSelected)
        {
            CheckForNearbyGlass();
        }
    }

    private void CheckForNearbyGlass()
    {
        Collider[] glasses = Physics.OverlapSphere(transform.position, attachDistance, glassLayer);
        GlassTracker closestGlass = null;
        float closestDistance = float.MaxValue;

        foreach (var glassCollider in glasses)
        {
            if (glassCollider.CompareTag(glassTag))
            {
                GlassTracker glassTracker = glassCollider.GetComponent<GlassTracker>();
                if (glassTracker != null && !glassTracker.HasFruitAttached())
                {
                    float distance = Vector3.Distance(transform.position, glassCollider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestGlass = glassTracker;
                    }
                }
            }
        }

        if (closestGlass != nearbyGlass)
        {
            if (nearbyGlass != null)
            {
                HideHologram();
            }

            nearbyGlass = closestGlass;
            if (nearbyGlass != null)
            {
                ShowHologram();
            }
        }

        isNearGlass = nearbyGlass != null;
    }

    private void ShowHologram()
    {
        if (nearbyGlass == null || fruitTracker == null) return;

        currentHologram = nearbyGlass.ShowFruitHologram(fruitTracker.GetFruitType());
    }

    private void HideHologram()
    {
        if (nearbyGlass != null)
        {
            nearbyGlass.HideFruitHologram();
        }
        currentHologram = null;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (isNearGlass && nearbyGlass != null && fruitTracker != null)
        {
            AttachToGlass();
        }
        else
        {
            HideHologram();
            nearbyGlass = null;
            isNearGlass = false;
        }
    }

    private void AttachToGlass()
    {
        if (nearbyGlass == null || fruitTracker == null) return;
        FruitType fruitType = fruitTracker.GetFruitType();
        nearbyGlass.ActivateFruit(fruitType);
        Destroy(gameObject);
    }
}
