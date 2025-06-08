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
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Controller.Spawner?.NotifyClientFinished();
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            Agent.SetDestination(Controller.Spawner.Exit.transform.position);
            _isWalking = true;
        }
        return this;
    }
}
