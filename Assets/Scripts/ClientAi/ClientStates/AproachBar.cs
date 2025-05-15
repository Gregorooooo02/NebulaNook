using UnityEngine;
using UnityEngine.AI;

public class AproachBar : ClientState
{
    public WaitForDrink nextState;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;
    public float finalAngularSpeed = 240.0f;

    private BarChairScript Destination;

    private bool _finalAproach = false;
    private bool _isWalking = false;
    private float _baseAngularSpeed;

    public override ClientState RunState()
    {
        if (_finalAproach)
        {
            if (_isWalking)
            {
                if (Vector3.Distance(transform.position, Destination.AccessPoint.transform.position) <= MinPointDist)
                {
                    _isWalking = false;
                    _finalAproach = false;
                    Agent.angularSpeed = _baseAngularSpeed;
                    return nextState;
                }
            }
            else
            {
                Agent.SetDestination(Destination.SeatPoint.transform.position);
                _baseAngularSpeed = Agent.angularSpeed;
                Agent.angularSpeed = finalAngularSpeed;
                _isWalking = true;
            }
        } 
        else
        {
            if (_isWalking)
            {
                if (Vector3.Distance(transform.position, Destination.AccessPoint.transform.position) <= MinPointDist)
                {
                    _isWalking = false;
                    _finalAproach = true;
                }
            }
            else
            {
                Agent.SetDestination(Destination.AccessPoint.transform.position);
                _isWalking = true;   
            }
        }
        return this;
    }

    public void SetDestination(BarChairScript barChairScript)
    {
        Destination = barChairScript;
    }

}
