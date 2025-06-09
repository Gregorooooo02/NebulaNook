using System.Collections;
using UnityEngine;

public class BubblesPIP : PipState
{
    public GameObject bubbles;

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
        bubbles.SetActive(true);
        yield return new WaitForSeconds(duration);
        bubbles.SetActive(false);
        done = true;
    }
}
