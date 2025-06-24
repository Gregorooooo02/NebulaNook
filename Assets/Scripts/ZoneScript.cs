using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ZoneScript : MonoBehaviour
{
    public Transform DrinkTarget;
    public BarChairScript BarChairScript;
    private Animator anim;

    public float glassTransitionTime = 0.5f;

    private GameObject currentGlass;

    public GameObject[] Glasses;
    public GameObject[] Fruits;

    private GameObject currentFruit;
    private GameObject enabledGlass;

    public bool Enabled = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.Play("Show");
    }

    
    private void OnDisable()
    {
        anim.Play("Hide");
    }

    public void EnableHolograms(GlassType glass, FruitType fruit)
    {
        enabledGlass = Glasses[(int)glass];
        enabledGlass.SetActive(true);
        if (fruit != FruitType.NONE)
        {
            currentFruit = Fruits[(int)fruit - 1];
            currentFruit.SetActive(true);
        }
    }

    public void DisableHolograms()
    {
        currentFruit?.SetActive(false);
        enabledGlass?.SetActive(false);
        enabledGlass.transform.parent.localScale = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!Enabled) return;
        TestEffectObject testEffectObject = other.gameObject.GetComponent<TestEffectObject>();

        if (BarChairScript.Occupied && testEffectObject != null)
        {
            DisableHolograms();
            BarChairScript.Occupier.Drink(testEffectObject.drinkEffect, GlassType.COCKTAIL, FruitType.NONE);
        }
        
        if (!BarChairScript.Occupied && currentGlass != null) return;

        if (other.gameObject.TryGetComponent<GlassFiller>(out GlassFiller component) && !component.wasServed)
        {
            if (component.currentFillAmount < 0.5f) return; // Check if the glass is filled enough
            currentGlass = other.gameObject;

            DisableHolograms();

            other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
            // Parent the glass to the drink target
            // other.gameObject.transform.SetParent(DrinkTarget);
            other.gameObject.transform.position = DrinkTarget.position;

            // Get the GlassTracker component
            GlassTracker glassTracker = currentGlass.GetComponent<GlassTracker>();

            BarChairScript.Occupier.currentGlass = currentGlass;
            BarChairScript.Occupier.Drink(component.GetFinalDrinkEffect(), glassTracker.glassType, glassTracker.attachedFruitType);
            component.wasServed = true;

            currentGlass = null;
        }
    }
}
