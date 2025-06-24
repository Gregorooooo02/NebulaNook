using System.Collections;
using UnityEngine;

public class BubblesPIP : PipState
{
    public GameObject bubbles;
    private ParticleSystem.EmissionModule system;
    
    public float duration;

    private bool triggered = false;
    private bool done = false;

    private void Start()
    {
        system = bubbles.GetComponent<ParticleSystem>().emission;
    }

    public override PipState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }
        if (done)
        {
            done = false;
            triggered = false;
            Destroy(controller.transform.parent.gameObject);
            PipSpawner.Instance?.SpawnPip();
        }
        return this;
    }

    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        system.enabled = true;
        yield return new WaitForSeconds(duration);
        system.enabled = false;
        done = true;
    }
}
