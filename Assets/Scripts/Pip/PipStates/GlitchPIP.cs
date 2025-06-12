using System.Collections;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class GlitchPIP : PipState
{
    public Renderer[] renderers;
    public Material[] replacementMaterials;

    private float currentGlitchStrenght;
    public float glitchRate;

    private bool triggered = false;

    public Animator animator;
    public float duration;

    public override PipState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(replacementMaterials[i]);
            }
            StartCoroutine("ExecuteEffect");
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        animator.enabled = false;
        while (currentGlitchStrenght < 1.0f)
        {
            currentGlitchStrenght += glitchRate * Time.fixedDeltaTime;
            if (currentGlitchStrenght > 1.0f)
            {
                currentGlitchStrenght = 1.0f;
            }
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Glitch_Effect_Strenght", currentGlitchStrenght);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }


}
