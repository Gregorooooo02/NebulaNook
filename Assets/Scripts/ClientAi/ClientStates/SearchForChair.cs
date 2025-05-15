using UnityEngine;

public class SearchForChair : ClientState
{
    public AproachBar nextState;

    public float ChairTimeout;

    private bool _isInChairTimeout = false;
    private float _currentTimeoutValue = 0;

    public override ClientState RunState()
    {
        if (_isInChairTimeout)
        {
            if (_currentTimeoutValue < ChairTimeout)
            {
                _currentTimeoutValue += Time.fixedDeltaTime;
                return this;
            }
            else
            {
                _currentTimeoutValue = 0;
                _isInChairTimeout = false;
            }
        }
        BarChairScript result = ChairManager.Instance.ReserveAnyAvailableChair();
        if (result == null)
        {
            _isInChairTimeout = true;
            return this;
        }
        else
        {
            result.Occupier = gameObject.GetComponentInParent<ClientController>();
            nextState.SetDestination(result);
            return nextState;
        }
    }
}
