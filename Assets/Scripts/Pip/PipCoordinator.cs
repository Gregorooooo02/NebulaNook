using UnityEngine;

public class PipCoordinator : MonoBehaviour
{
    public GameObject PipPhysics;
    public GameObject PipEffects;

    public PipController PipController;

    public void SwitchPips(DrinkEffect effect)
    {
        PipPhysics.SetActive(false);

        PipEffects.transform.localPosition = PipPhysics.transform.position;
        PipEffects.transform.localRotation = PipPhysics.transform.rotation;

        PipEffects.SetActive(true);
        PipController.Drink(effect);
    }

}
