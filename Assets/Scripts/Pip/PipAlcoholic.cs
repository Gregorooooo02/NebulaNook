using UnityEngine;

public class PipAlcoholic : MonoBehaviour
{
    public PipCoordinator PipCoordinator;

    public void Trigger(DrinkEffect effect)
    {
        PipCoordinator.SwitchPips(effect);
    }
    
}
