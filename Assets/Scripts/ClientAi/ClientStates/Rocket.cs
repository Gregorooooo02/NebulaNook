using System.Collections;
using UnityEngine;

public class Rocket : ClientState
{
    private bool triggered = false;
    public Animator animator;

    public float destinationPollingTime = 0.5f;
    public override ClientState RunState()
    {
/*        if (triggered)
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
        }*/

        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }

        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        Controller.ToggleRagdoll(true);
        Controller.StiffenRagdoll();
        Controller.DissableGravity();
        Controller.FreezeRotation();
        animator.enabled = true;
        animator.SetBool("Takeoff", true);
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Takeoff") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(destinationPollingTime);
        }
        Controller.Spawner?.NotifyClientFinished();
        Destroy(gameObject.transform.parent.gameObject);
    }

}
