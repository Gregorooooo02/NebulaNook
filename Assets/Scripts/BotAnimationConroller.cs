using UnityEngine;
using UnityEngine.AI;

public class BotAnimationConroller : MonoBehaviour
{
    private Animator animator;
    // private NavMeshAgent agent;
    // private ClientScript clientScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }

        // agent = GetComponent<NavMeshAgent>();
        // if (agent == null)
        // {
        //     Debug.LogError("NavMeshAgent component not found on this GameObject.");
        // }
    }

    private void Update()
    {
        // if (agent != null)
        // {
        //     float speed = agent.velocity.magnitude;
        //     if (speed > 0.1f) {
        //         animator.SetBool("IsWalking", true);
        //     } else {
        //         animator.SetBool("IsWalking", false);
        //     }
        // }
        // if (clientScript) {
        //     if (clientScript.state == ClientScript.ClientState.WaitingForDrink) {
        //         animator.SetBool("isWaving", true);
        //     } else {
        //         animator.SetBool("isWaving", false);
        //     }
        // }
    }
}
