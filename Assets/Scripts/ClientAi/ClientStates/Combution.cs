using UnityEngine;
using UnityEngine.AI;

public class Combution : ClientState
{
    public GameObject CombutionEffect;
    public GameObject parent;

    private bool triggered = false;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;


    public override ClientState RunState()
    {
        if (triggered)
        {
            if (Vector3.Distance(parent.transform.position, ChairManager.Instance.ExitPoint.transform.position) <= MinPointDist)
            {
                triggered = false;
                Destroy(parent);
            }
        }
        else
        {
            Agent.SetDestination(ChairManager.Instance.ExitPoint.transform.position);
            CombutionEffect.SetActive(true);
            triggered = true;
        }
        return this;
    }


}
