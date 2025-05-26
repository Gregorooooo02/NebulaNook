using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Rocket : ClientState
{
    private bool triggered = false;
    public Animator animator;
    public override ClientState RunState()
    {
        if (triggered)
        {

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
            animator.SetBool("Takeoff",true);
        }

        return this;
    }
}
