using System.Collections;
using UnityEngine;

public class BlackHolePIP : PipState
{
    public GameObject BlackHole;

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
        GameObject blackhole = Instantiate(BlackHole, transform);
        yield return new WaitForSeconds (duration);
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
