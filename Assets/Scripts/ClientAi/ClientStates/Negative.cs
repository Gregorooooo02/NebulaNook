using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Negative : ClientState
{
    public NavMeshAgent agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    private bool triggered = false;

    public float initialDelay;

    public GameObject inverter;
    public GameObject Parent;
    public override ClientState RunState()
    {
        if (_isWalking && agent.remainingDistance < MinPointDist)
        {
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
        inverter.SetActive(true);
        yield return new WaitForSeconds(initialDelay);
        agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForFixedUpdate();
        _isWalking=true;
    }

}
