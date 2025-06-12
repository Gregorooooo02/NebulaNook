using System.Collections;
using UnityEngine;

public class PassOutPIP : PipState
{
    public GameObject Parent;
    public float duration;

    private bool triggered = false;

    public override PipState RunState()
    {
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
        controller.ToggleRagdoll(true);
        yield return new WaitForSeconds(duration);
        Destroy(Parent);
    }
}
