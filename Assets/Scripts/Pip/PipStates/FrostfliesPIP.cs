using System.Collections;
using UnityEngine;

public class FrostfliesPIP : PipState
{
    private bool triggered = false;

    public GameObject frostflies;
    public float duration;
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
        frostflies.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }
}
