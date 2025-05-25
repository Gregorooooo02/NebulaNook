using UnityEngine;
using UnityEngine.AI;

public class Slow : ClientState
{
    public NavMeshAgent Agent;
    public Animator animator;
    public float MinPointDist = 0.75f;

    public float SlowValue;

    private bool _isWalking = false;
    public override ClientState RunState()
    {
        if (_isWalking)
        {
            if (Vector3.Distance(transform.position, ChairManager.Instance.ExitPoint.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            Agent.SetDestination(ChairManager.Instance.ExitPoint.transform.position);
            animator.speed = SlowValue;
            Agent.speed *= SlowValue;
            _isWalking = true;
        }
        return this;
    }
}
