using System.Collections;
using UnityEngine;

public class AnihilationPIP : PipState
{
    public GameObject Flash;

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
        Instantiate(Flash,transform);
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }
}
