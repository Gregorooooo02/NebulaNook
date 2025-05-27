using UnityEngine;
using UnityEngine.AI;

public class Bubbles : ClientState
{
    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;
    public GameObject BubblesPrefab;

    private bool _isWalking = false;

    public override ClientState RunState()
    {
        if (_isWalking)
        {
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            Agent.SetDestination(Controller.Spawner.Exit.transform.position);
            Instantiate(BubblesPrefab, transform);
            _isWalking = true;
        }
        return this;
    }
}
