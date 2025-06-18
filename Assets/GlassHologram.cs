using UnityEngine;

public class GlassHologram : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator.SetBool("Apear", true);
    }

    private void OnDisable()
    {
        animator.SetBool("Apear", false);
    }
}
