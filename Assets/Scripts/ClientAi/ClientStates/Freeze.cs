using UnityEngine;

public class Freeze : ClientState
{
    public float TimeToDisapear;
    private float _currentTime = 0;

    private bool triggered = false;

    public GameObject FreezeEffect;
    public GameObject parent;
    public override ClientState RunState()
    {
        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                return this;
            }
            Destroy(parent);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            GetComponentInParent<ClientController>().StiffenRagdoll();
            Instantiate(FreezeEffect, transform);
            triggered = true;
        }
        return this;
    }

}
