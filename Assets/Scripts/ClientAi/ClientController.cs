using UnityEngine;
using UnityEngine.AI;

public class ClientController : MonoBehaviour
{
    public ClientState CurrentState;

    public bool IsWaiting = false;

    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _isWalking = false;
    private bool _isWaving = false;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        ClientState nextState = CurrentState.RunState();

        if (nextState != null)
        {
            CurrentState = nextState;
        }
        CheckWaiting();


        CheckWalking();
        CheckWaiving(); 
    }

    private void CheckWaiting()
    {
        if(CurrentState is WaitForDrink)
        {
            IsWaiting = true;
        }
        else
        {
            IsWaiting = false;
        }
    }

    private void CheckWalking()
    {
        float speed = _agent.velocity.magnitude;
        if (!_isWalking && speed > 0.1f)
        {
            _animator.SetBool("isWalking", true);
            _isWalking = true;
        }
        else if(_isWalking && speed <= 0.1f)
        {
            _animator.SetBool("isWalking", false);
            _isWalking = false;
        }
    }

    private void CheckWaiving()
    {
        if (!_isWaving && IsWaiting)
        {
            _animator.SetBool("isWaving", true);
            _isWaving = true;
        }
        else if(_isWaving && !IsWaiting)
        {
            _animator.SetBool("isWaving", false);
            _isWaving = false;
        }
    }
}
