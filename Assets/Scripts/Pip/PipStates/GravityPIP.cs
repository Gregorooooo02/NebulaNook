using System.Collections;
using UnityEngine;

public class GravityPIP : PipState
{
    public Rigidbody mainBone;
    public float upwardForce;
    public float floattime;

    private float currentTime = 0;
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
        while (currentTime < floattime)
        {
            mainBone.AddForce(Physics.gravity * -2 * upwardForce,ForceMode.Force);
            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
