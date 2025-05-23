using UnityEngine;

public class PassOut : ClientState
{
    public float TimeToDisapear;
    private float _currentTime = 0;

    private bool triggered = false;

    public override ClientState RunState()
    {
        if (triggered)
        {
            if(_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                return this;
            }
            Destroy(gameObject.transform.parent.gameObject);
        } 
        else
        {
            GetComponentInParent<ClientController>().StiffenRagdoll();
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            triggered = true;
        }
        return this;
    }
}
