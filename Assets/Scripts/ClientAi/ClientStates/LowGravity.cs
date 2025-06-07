using UnityEngine;

public class LowGravity : ClientState
{
    public float TimeToDisapear;
    public float anitgravityStrenghtMultiplier;

    private float _currentTime = 0;
    private bool triggered = false;

    public GameObject parent;
    public Rigidbody mainBone;

    public override ClientState RunState()
    {
        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                mainBone.AddForce(Physics.gravity * -2 * anitgravityStrenghtMultiplier, ForceMode.Force);
                return this;
            }
            Controller.Spawner?.NotifyClientFinished();
            Destroy(parent);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            mainBone.AddForce(Physics.gravity * -2 * anitgravityStrenghtMultiplier,ForceMode.Force);
            triggered = true;
        }
        return this;
    }
}
