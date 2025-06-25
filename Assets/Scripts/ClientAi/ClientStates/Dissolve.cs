using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Dissolve : ClientState
{
    public Renderer[] renderers;
    public Material[] replacementMaterials;

    public float dissolveSpeed;
    public float finalDelay;

    private bool triggered = false;
    private float dissolveAmount = 0.0f;

    public GameObject particleEffect;
    public GameObject Parent;
    public AudioSource DissolveAudioSource;

    public override ClientState RunState()
    {
        if (!triggered)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(replacementMaterials[i]);
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
        DissolveAudioSource.Play();
        while (dissolveAmount < 1.0f)
        {
            dissolveAmount += dissolveSpeed * Time.fixedDeltaTime;
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Dissolve_Amount", dissolveAmount);
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(finalDelay);
        Controller.Spawner?.NotifyClientFinished();
        Destroy(Parent);
    }
}
