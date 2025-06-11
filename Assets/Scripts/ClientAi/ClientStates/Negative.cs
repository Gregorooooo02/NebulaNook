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

    public Material NegativeMaterial;
    public Renderer[] renderers;

    public GameObject Parent;
    public override ClientState RunState()
    {
        if (_isWalking && agent.remainingDistance < MinPointDist)
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
        foreach (Renderer r in renderers)
        {
            r.material = new Material(NegativeMaterial);
        }
        yield return new WaitForSeconds(initialDelay);
        agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForFixedUpdate();
        _isWalking=true;
    }

}
