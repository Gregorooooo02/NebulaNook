using UnityEngine;
using UnityEngine.AI;

public class PassOut : ClientState
{
    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

    public float TimeToDisapear;
    public float TimeToPassOut;
    private float _currentTime = 0;

    private bool triggered = false;
    private bool _isWalking = false;

    public override ClientState RunState()
    {
        if (_isWalking)
        {
            if (triggered)
            {
                if (_currentTime < TimeToDisapear)
                {
                    _currentTime += Time.fixedDeltaTime;
                    return this;
                }
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                if(_currentTime < TimeToPassOut)
                {
                    _currentTime += Time.fixedDeltaTime;
                    return this;
                } 
                else
                {
                    GetComponentInParent<ClientController>().ToggleRagdoll(true);
                    triggered = true;
                    _currentTime = 0;
                }
            }
        } 
        else
        {
            Agent.SetDestination(ChairManager.Instance.ExitPoint.transform.position);
            _isWalking=true;
        }
        return this;
    }
}
