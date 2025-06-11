using UnityEngine;

public class TestPIPZone : MonoBehaviour
{
    public PipController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TestEffectObject>(out TestEffectObject c))
        {
            controller.Drink(c.drinkEffect);
        }
    }
}
