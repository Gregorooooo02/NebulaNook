using UnityEngine;

public class Anihilation : ClientState
{

    public float TimeToDisapear;
    private float _currentTime = 0;

    private bool triggered = false;

    public GameObject parent;
    public GameObject flash;
    public GameObject Model;
    public override ClientState RunState()
    {
        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                Model.SetActive(false);
                return this;
            }
            Destroy(parent);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            Instantiate(flash, transform);
            triggered = true;
        }
        return this;
    }
}
