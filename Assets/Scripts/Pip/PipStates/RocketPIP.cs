using UnityEngine;

public class RocketPIP : PipState
{
    public Animator animator;

    private bool triggered = false;

    public override PipState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            animator.enabled = true;
            //controller.ToggleRagdoll(true);
            //controller.StiffenRagdoll();
            //controller.FreezeRotation();
            animator.SetBool("Takeoff",true);
        }
        return this;
    }

}
