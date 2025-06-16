using System.Collections;
using UnityEngine;

public class FreezePIP : PipState
{
    public Material frozenMaterial;
    public SkinnedMeshRenderer[] skinnedMeshes;

    public GameObject mist;

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
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        controller.ToggleRagdoll(true);
        controller.StiffenRagdoll();
        foreach(var skinnedMesh in skinnedMeshes)
        {
            skinnedMesh.material = frozenMaterial;
        }
        mist.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }
}
