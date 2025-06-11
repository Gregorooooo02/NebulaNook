using System.Collections;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class NegativePIP : PipState
{
    public Material NegativeMaterial;
    public Renderer[] renderers;

    private bool triggered = false;

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
        foreach (Renderer r in renderers)
        {
            r.material = new Material(NegativeMaterial);
        }
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }
}
