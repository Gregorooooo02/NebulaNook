using UnityEngine;

public class ToungeCordinator : MonoBehaviour
{
    public Animator animator;
    public GameObject toungeModel;
    private float originalSpeed;
    public void StopOnFrameone()
    {
        originalSpeed = animator.speed;
        animator.speed = 0;
        toungeModel.SetActive(true);
    }

    public void Resume()
    {
        animator.speed = originalSpeed;
    }
}
