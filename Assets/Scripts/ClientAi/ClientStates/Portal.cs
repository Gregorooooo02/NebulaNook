using System.Collections;
using UnityEngine;

public class Portal : ClientState
{
    public Renderer[] Renderers;
    public Material[] ReplecementMaterials;

    public GameObject portalEffect;
    public GameObject portalCloseEffect;
    public Transform suckPosition;

    private float currentEffectValue = 0;
    public float effectSpeed;
    public float effectDistance;
    public float effectTarget;

    public float suckDelayTime;
    public float portalSpawnDelay;

    private bool triggered = false;

    public GameObject Parent;

    public float finalDelay;

    public override ClientState RunState()
    {
        if (!triggered)
        {
            for (int i = 0; i < Renderers.Length; i++)
            {
                Renderers[i].material = new Material(ReplecementMaterials[i]);
                Renderers[i].material.SetFloat("_SuckRange", effectDistance);
                Renderers[i].material.SetVector("_Suck_Position", suckPosition.position);
            }
            StartCoroutine("ExecuteEffect");
            triggered = true;
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(portalSpawnDelay);
        portalEffect.SetActive(true);
        yield return new WaitForSeconds(suckDelayTime);
        Parent.GetComponent<ClientController>().mainCollider.enabled = false;
        while(currentEffectValue < effectTarget)
        {
            currentEffectValue += effectSpeed * Time.fixedDeltaTime;
            foreach (var renderer in Renderers)
            {
                renderer.material.SetFloat("_SuckEffect", currentEffectValue);
            }
            yield return new WaitForFixedUpdate();    
        }
        portalCloseEffect.SetActive(true);
        yield return new WaitForSeconds(1.05f);
        portalEffect.SetActive(false);
        yield return new WaitForSeconds(finalDelay);
        Destroy(Parent);
    }
}
