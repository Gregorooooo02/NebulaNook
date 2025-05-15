using UnityEngine;
using UnityEngine.AI;

public class LeaveState : ClientState
{
    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

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
            _isWalking = true;
        }
        return this;
    }
}
