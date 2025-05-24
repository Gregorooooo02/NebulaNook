using UnityEngine;
using UnityEngine.AI;

public class ClientScript : MonoBehaviour
{
    public enum ClientState
    {
        NoOperation,
        WaitForChair,
        MovingToChair,
        Sit,
        WaitingForDrink,
        GettingUp,
        MovingToExit,
        Despawning
    }

    public NavMeshAgent agent;
    public BarChairScript occupiedBarChair = null;
    public ClientState state;

    [Header("GeneralMovement")]
    public float WalkSpeed;
    public float MinPointDist;

    [Header("Waiting for chair")]
    public float ChairTimeout = 5.0f;
    private float _currentTimeoutValue = 0;
    private bool _isInChairTimeout = false;

    [Header("Walking to chair")]
    public bool _isWalking = false;

    [Header("Sitting")]
    private bool _isSitting = false;

    private bool _walkAway = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.speed = WalkSpeed;
        //agent.SetDestination(new Vector3(0, 1, 6));
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case ClientState.WaitForChair:
            {
                if (_isInChairTimeout)
                {
                    if(_currentTimeoutValue < ChairTimeout)
                    {
                        _currentTimeoutValue += Time.fixedDeltaTime;
                        return;

                    } else
                    {
                        _currentTimeoutValue = 0;
                        _isInChairTimeout = false;
                    }
                }
                BarChairScript result = ChairManager.Instance.ReserveAnyAvailableChair();
                if(result == null)
                {
                    _isInChairTimeout = true;
                    return;
                }
                else
                {
                    occupiedBarChair = result;
                    state = ClientState.MovingToChair;
                }
                break;
            }
            case ClientState.MovingToChair:
            {
                if (_isWalking)
                {
                    if(Vector3.Distance(transform.position,occupiedBarChair.AccessPoint.transform.position) <= MinPointDist)
                    {
                        _isWalking = false;
                        state = ClientState.Sit;
                        return;
                    }
                } 
                else
                {
                    agent.SetDestination(occupiedBarChair.AccessPoint.transform.position);
                        
                    _isWalking = true;
                }
                break;
            }
            case ClientState.Sit:
            {
                if (_isSitting)
                {
                    if (Vector3.Distance(transform.position, occupiedBarChair.SeatPoint.transform.position) <= MinPointDist)
                    {
                        _isSitting = false;
                        state = ClientState.WaitingForDrink;
                        return;
                    }
                }
                else
                {
                    agent.SetDestination(occupiedBarChair.SeatPoint.transform.position);
                    _isSitting = true;
                }
                break;
            }
            case ClientState.WaitingForDrink:
            {
                if (_walkAway)
                {
                    state = ClientState.GettingUp;
                    _walkAway = false;
                }
                break;
            }
            case ClientState.GettingUp:
            {
                if (_isSitting)
                {
                    if (Vector3.Distance(transform.position, occupiedBarChair.AccessPoint.transform.position) <= MinPointDist)
                    {
                        _isSitting = false;
                        state = ClientState.MovingToExit;
                        ChairManager.Instance.VacateChair(occupiedBarChair);
                        occupiedBarChair = null;
                        return;
                    }
                }
                else
                {
                    agent.SetDestination(occupiedBarChair.AccessPoint.transform.position);
                    _isSitting = true;
                }
                break;
            }
            case ClientState.MovingToExit:
            {
                if (_isWalking)
                {
                    if (Vector3.Distance(transform.position, ChairManager.Instance.ExitPoint.transform.position) <= MinPointDist)
                    {
                        _isWalking = false;
                        state = ClientState.Despawning;
                        return;
                    }
                }
                else
                {
                    agent.SetDestination(ChairManager.Instance.ExitPoint.transform.position);

                    _isWalking = true;
                }
                break;
            }
            case ClientState.Despawning:
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _walkAway = true;
    }
}
