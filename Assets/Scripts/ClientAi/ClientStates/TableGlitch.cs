using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TableGlitch : ClientState
{
    public float initialDelay;

    public Rigidbody mainBone;
    public NavMeshAgent agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    private bool triggered = false;

    public float animationActivationDelay;
    public GameObject Parent;

    public float yeetDelay;
    public float yeetForce;

    public override ClientState RunState()
    {
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
        agent.SetDestination(TableHandle.Instance.transform.position);
        yield return new WaitForFixedUpdate();
        while(agent.remainingDistance > 0.05f)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(animationActivationDelay);
        TableHandle.Instance.TriggerAnimation(this);
    }

    public void TriggerYeet(Vector3 direction)
    {
        Controller.ToggleRagdoll(true);
        direction.Normalize();
        Vector3 explosionPoint = mainBone.transform.position - direction;
        mainBone.AddExplosionForce(yeetForce, explosionPoint, 5);
        StartCoroutine("Delete");
    }


    IEnumerator Delete()
    {
        yield return new WaitForSeconds(yeetDelay);
        Destroy(Parent);
    }

}
