using System.Collections;
using UnityEngine;

public class AplausePIP : PipState
{

    private bool triggered = false;

    public float duration;
    public GameObject[] confetti;

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
        foreach (var f in confetti)
        {
            f.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
