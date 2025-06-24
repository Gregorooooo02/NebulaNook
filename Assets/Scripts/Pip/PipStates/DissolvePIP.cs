using System.Collections;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class DissolvePIP : PipState
{
    public Renderer[] renderers;
    public Material[] replacemnetMaterials;

    public float dissolveSpeed;
    public float duration;

    private bool triggered = false;
    private float dissolveAmount = 0.0f;

    public GameObject particleEffect;

    public override PipState RunState()
    {
        if (!triggered)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(replacemnetMaterials[i]);
                renderers[i].material.SetFloat("_Dissolve_Amount", 0.0f);
            }
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        particleEffect.SetActive(true);
        while (dissolveAmount < 1.0f)
        {
            dissolveAmount += dissolveSpeed * Time.fixedDeltaTime;
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Dissolve_Amount", dissolveAmount);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(duration);
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
