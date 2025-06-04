using UnityEngine;

public class Possesion : ClientState
{
    private bool triggered = false;
    public Animator animator; 
    public override ClientState RunState()
    {
        if (!triggered)
        {
            ClientController controller = GetComponentInParent<ClientController>();
            controller.ToggleRagdoll(true);
            animator.enabled = true;
            animator.SetBool("Possesed",true);
            triggered = true;   
        }
        return this;
    }
}
