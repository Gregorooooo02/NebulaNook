using UnityEngine;

public class PipCoordinator : MonoBehaviour
{
    public GameObject PipPhysics;
    public GameObject PipEffects;

    public PipController PipController;
    public Collider MainCollider;
    public PipScript pipScript;

    public void SwitchPips(DrinkEffect effect)
    {
        PipPhysics.SetActive(false);

        PipEffects.transform.localPosition = PipPhysics.transform.localPosition;
        PipEffects.transform.localRotation = PipPhysics.transform.localRotation;

        PipEffects.SetActive(true);
        PipController.Drink(effect);
    }

}
