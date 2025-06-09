using System.Collections;
using UnityEngine;

public class CombustPIP : PipState
{
    public GameObject Fire;

    public float duration;

    private bool triggered = false;
    private bool done = false;

    public override PipState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }
        if (done)
        {
            done = false;
            triggered = false;
            return DefaultState;
        }
        return this;
    }


    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        Fire.SetActive(true);
        yield return new WaitForSeconds(duration);
        Fire.SetActive(false);
        done = true;
    }
}
