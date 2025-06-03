using UnityEngine;
using UnityEngine.AI;

public class CloneControler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator Animator;
    public float minRemoveDist;

    private bool _isWalking = false;
    private Vector3 targetPos;

    public GameObject Model;

    public float timeToActivate;
    private float _currentTime = 0;

    public void Exit(Vector3 position)
    {
        agent.SetDestination(position);
        targetPos = position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position,targetPos) < minRemoveDist)
        {
            Destroy(gameObject);
            return;
        }
        CheckWalking();
        _currentTime += Time.fixedDeltaTime;
        if (!Model.activeInHierarchy && _currentTime >= timeToActivate)
        {
            Model.SetActive(true);
        }
    }

    private void CheckWalking()
    {
        float speed = agent.velocity.magnitude;
        if (!_isWalking && speed > 0.1f)
        {
            Animator.SetBool("isWalking", true);
            _isWalking = true;
        }
        else if (_isWalking && speed <= 0.1f)
        {
            Animator.SetBool("isWalking", false);
            _isWalking = false;
        }
    }
}
