using System.Collections;
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
/*        if (triggered)
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
        }*/

        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }

        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        Controller.ToggleRagdoll(true);
        while (_currentTime < TimeToDisapear)
        {
            mainBone.AddForce(Physics.gravity * -2 * anitgravityStrenghtMultiplier, ForceMode.Force);
            _currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Controller.Spawner?.NotifyClientFinished();
        Destroy(parent);
    }

}
