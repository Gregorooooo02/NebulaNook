using UnityEngine;

public class PipAlcoholic : MonoBehaviour
{
    public PipCoordinator PipCoordinator;

    public void Trigger(DrinkEffect effect)
    {
        Debug.Log("Pip Triggered!");
        PipCoordinator.SwitchPips(effect);
    }
    
}
