using System.Collections;
using UnityEngine;

public class OvergrowthPIP : PipState
{
    private bool triggered = false;
    public GameObject VFX;

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
        VFX.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
