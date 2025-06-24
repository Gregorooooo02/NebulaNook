using System.Collections;
using UnityEngine;

public class ExplosionPIP : PipState
{
    public GameObject ExplosionEffect;

    public Rigidbody mainBone;

    public float duration;
    public float explosiveForce;

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
        controller.ToggleRagdoll(true);
        mainBone.AddExplosionForce(explosiveForce, transform.position, 5);
        ExplosionEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
