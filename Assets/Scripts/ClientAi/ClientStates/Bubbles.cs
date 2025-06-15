using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bubbles : ClientState
{
    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;
    public GameObject BubblesPrefab;

    private bool _isWalking = false;

    private bool triggered = false; 

    public float walkDelay = 2;
    public float destinationPollingTime = 0.5f;

    public override ClientState RunState()
    {
/*        if (_isWalking)
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
            Instantiate(BubblesPrefab, transform);
            _isWalking = true;
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
        Instantiate(BubblesPrefab, transform);
        yield return new WaitForSeconds(walkDelay);
        Agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForSeconds(destinationPollingTime);
        while (Agent.remainingDistance > MinPointDist)
        {
            yield return new WaitForSeconds(destinationPollingTime);
        }
        Controller.Spawner?.NotifyClientFinished();
        Destroy(Controller.gameObject);
    }
}
