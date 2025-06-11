using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Aplause : ClientState
{
    public NavMeshAgent agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    private bool triggered = false;

    public float waitingTime;

    public GameObject[] confetti;
    public GameObject Parent;


    public override ClientState RunState()
    {
        if(_isWalking && agent.remainingDistance < MinPointDist)
        {
            Controller.Spawner?.NotifyClientFinished();
            Destroy(Parent);
        }
        if (!triggered)
        {
            StartCoroutine("ExecuteEffect");
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        foreach (var f in confetti)
        {
            f.SetActive(true);
        }
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForFixedUpdate();
        _isWalking = true;
    }
}
