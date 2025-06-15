using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Glitch : ClientState
{
    public NavMeshAgent agent;
    public float MinPointDist = 0.75f;

    private bool triggered = false;
    private bool isWalking = false;

    public Renderer[] renderers;
    public Material[] replacemnetMaterials;

    private float currentGlitchStrenght;
    public float glitchRate;

    public GameObject Parent;
    public Animator animator;

    public override ClientState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = new Material(replacemnetMaterials[i]);
            }
            StartCoroutine("ExecuteEffect");
        }
        if (isWalking && agent.remainingDistance < MinPointDist)
        {
            Controller.Spawner?.NotifyClientFinished();
            Destroy(Parent);
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        animator.enabled = false;
        while(currentGlitchStrenght < 1.0f)
        {
            currentGlitchStrenght += glitchRate * Time.fixedDeltaTime;
            if(currentGlitchStrenght > 1.0f)
            {
                currentGlitchStrenght = 1.0f;
            }
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Glitch_Effect_Strenght", currentGlitchStrenght);
            }
            yield return new WaitForFixedUpdate();
        }
        agent.SetDestination(Controller.Spawner.Exit.transform.position);
        yield return new WaitForFixedUpdate();
        isWalking = true;
    }
}
