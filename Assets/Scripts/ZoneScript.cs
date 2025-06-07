using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public BarChairScript BarChairScript;
    private Animator anim;

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

    public void OnTriggerEnter(Collider other)
    {
        //Add passing some sort of argument later
        //For example the drink contents or maybe the drink effects only

        /*
        TestEffectObject testEffectObject = other.gameObject.GetComponent<TestEffectObject>();

        if (BarChairScript.Occupied && testEffectObject != null) BarChairScript.Occupier.Drink(testEffectObject.drinkEffect);
        */
        if (BarChairScript.Occupied && other.gameObject.TryGetComponent<GlassFiller>(out GlassFiller component))
        {
            BarChairScript.Occupier.Drink(component.GetFinalDrinkEffect());
        }
    }
}
