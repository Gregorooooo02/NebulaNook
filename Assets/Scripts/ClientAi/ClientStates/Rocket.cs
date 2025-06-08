using UnityEngine;

public class Rocket : ClientState
{
    private bool triggered = false;
    public Animator animator;
    public override ClientState RunState()
    {
        if (triggered)
        {
            // Check if "Takeoff" animation is finished
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Takeoff"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Controller.Spawner?.NotifyClientFinished();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            ClientController controller = GetComponentInParent<ClientController>();
            controller.ToggleRagdoll(true);
            controller.StiffenRagdoll();
            controller.DissableGravity();
            controller.FreezeRotation();
            triggered = true;
            animator.enabled = true;
            animator.SetBool("Takeoff", true);
        }

        return this;
    }
}
