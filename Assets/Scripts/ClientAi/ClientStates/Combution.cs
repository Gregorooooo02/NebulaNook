using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Combution : ClientState
{
    public GameObject CombutionEffect;
    public GameObject parent;

    private bool triggered = false;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

    public float leaveDelay = 2;
    public float destinationPollingTime = 0.5f;
    public AudioSource CombutionAudioSource;

    public override ClientState RunState()
    {

        /*        if (triggered)
                {
                    if (Vector3.Distance(parent.transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
                    {
                        triggered = false;
                        Controller.Spawner?.NotifyClientFinished();
                        Destroy(parent);
                    }
                }
                else
                {
                    Agent.SetDestination(Controller.Spawner.Exit.transform.position);
                    CombutionEffect.SetActive(true);
                    triggered = true;
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
        CombutionEffect.SetActive(true);
        CombutionAudioSource.Play();
        yield return new WaitForSeconds(leaveDelay);
        Agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForSeconds(destinationPollingTime);
        while(Agent.remainingDistance > MinPointDist)
        {
            yield return new WaitForSeconds(destinationPollingTime);
        }
        Controller.Spawner?.NotifyClientFinished();
        Destroy(parent);
    }
}
