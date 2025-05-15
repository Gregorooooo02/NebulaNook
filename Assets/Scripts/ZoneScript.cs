using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    public BarChairScript BarChairScript;

    public void OnTriggerEnter(Collider other)
    {
        //Add passing some sort of argument later
        //For example the drink contents or maybe the drink effects only
        BarChairScript.Occupier.Drink();
    }
}
