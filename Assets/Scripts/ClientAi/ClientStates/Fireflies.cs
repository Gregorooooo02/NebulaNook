using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Fireflies : ClientState
{
    public NavMeshAgent agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    private bool triggered = false;

    public GameObject Parent;

    public GameObject fireflies;
    public float waitingTime;

    public override ClientState RunState()
    {
        if (_isWalking && agent.remainingDistance < MinPointDist)
        {
            Controller.Spawner?.NotifyClientFinished();
            Destroy(Parent);
        }
        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        fireflies.SetActive(true);
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForSeconds(0.25f);
        _isWalking = true;
    }

}
