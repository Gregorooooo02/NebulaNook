using UnityEngine;
using UnityEngine.AI;

public class SizeChange : ClientState
{
    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;
    public Vector3 SizeMultiplier;
    public Transform BodyPart;

    public float ChangeTime;
    private float _currentTime = 0;

    public AnimationCurve AnimationCurve;

    private bool _isWalking = false;

    private Vector3 _startScale;
    private Vector3 _deltaScale;
    private void Start()
    {
        _startScale = BodyPart.localScale;
        Vector3 _targetScale = BodyPart.localScale;
        _targetScale.x *= SizeMultiplier.x;
        _targetScale.y *= SizeMultiplier.y;
        _targetScale.z *= SizeMultiplier.z;
        _deltaScale = _targetScale - _startScale;
    }

    public override ClientState RunState()
    {
        if (_isWalking)
        {
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Controller.Spawner?.NotifyClientFinished();
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            Agent.SetDestination(Controller.Spawner.Exit.transform.position);
            _isWalking = true;
        }
        if(_currentTime < ChangeTime)
        {
            _currentTime += Time.fixedDeltaTime;
            float t = AnimationCurve.Evaluate(_currentTime/ChangeTime);
            BodyPart.localScale = _startScale + (_deltaScale * t);
        }
        return this;
    }
}
