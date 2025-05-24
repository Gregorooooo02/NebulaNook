using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public BarChairScript BarChairScript;

    public void OnTriggerEnter(Collider other)
    {
        //Add passing some sort of argument later
        //For example the drink contents or maybe the drink effects only

        TestEffectObject testEffectObject = other.gameObject.GetComponent<TestEffectObject>();

        if(BarChairScript.Occupied && testEffectObject != null)BarChairScript.Occupier.Drink(testEffectObject.drinkEffect);
    }
}
