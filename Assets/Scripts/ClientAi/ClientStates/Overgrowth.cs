using UnityEngine;
using UnityEngine.AI;

public class Overgrowth : ClientState
{
    public float waitTime;
    private float _currentTime = 0;
    private bool waitDone = false;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

    private bool triggered = false;
    public GameObject VFX;

    private bool isWalking = false;
    public GameObject Parent;

    public override ClientState RunState()
    {
        if (triggered)
        {
            if (isWalking)
            {
                if (Vector3.Distance(Parent.transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
                {
                    Destroy(Parent);
                }
            }
            else
            {
                _currentTime += Time.fixedDeltaTime;
                if (_currentTime > waitTime)
                {
                    Agent.SetDestination(Controller.Spawner.Exit.transform.position);
                    isWalking = true;
                }
            }
        } 
        else
        {
            VFX.SetActive(true);
            triggered = true;
        }
        return this;
    }

}
