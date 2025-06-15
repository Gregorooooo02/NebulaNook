using System.Collections;
using UnityEngine;

public class Anihilation : ClientState
{
    public float TimeToDisapear;

    private bool triggered = false;

    public GameObject parent;
    public GameObject flash;
    public GameObject Model;
    public override ClientState RunState()
    {
        if (!triggered)
        {
            StartCoroutine("ExecuteEffect");
            triggered = true;
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        Controller.ToggleRagdoll(true);
        Instantiate(flash, transform);
        yield return new WaitForFixedUpdate();
        Model.SetActive(false);
        yield return new WaitForSeconds(TimeToDisapear);
        Controller.Spawner?.NotifyClientFinished();
        Destroy(parent);
    }
}
