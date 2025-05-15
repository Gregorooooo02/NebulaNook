using UnityEngine;
using UnityEngine.AI;

public class ClientController : MonoBehaviour
{
    public ClientState CurrentState;
    
    public SpeechBubble bubble;

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
        if(!IsWaiting && CurrentState is WaitForDrink)
        {
            IsWaiting = true;
            bubble.gameObject.SetActive(true);
            bubble.SetText("Did you ever hear the tragedy of Darth Plagueis The Wise? I thought not. It’s not a story the Jedi would tell you. It’s a Sith legend. Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could use the Force to influence the midichlorians to create life… He had such a knowledge of the dark side that he could even keep the ones he cared about from dying. The dark side of the Force is a pathway to many abilities some consider to be unnatural. He became so powerful… the only thing he was afraid of was losing his power, which eventually, of course, he did. Unfortunately, he taught his apprentice everything he knew, then his apprentice killed him in his sleep. Ironic. He could save others from death, but not himself.");
        }
        else if(IsWaiting && CurrentState is not WaitForDrink)
        {
            bubble.gameObject.SetActive(false);
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

    public void Drink()
    {
        if (IsWaiting)
        {
            ((WaitForDrink)CurrentState).Continue = true;
        }
    }
}
