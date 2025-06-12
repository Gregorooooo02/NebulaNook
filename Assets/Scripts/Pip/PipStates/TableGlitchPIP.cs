using System.Collections;
using UnityEngine;

public class TableGlitchPIP : PipState
{
    public Rigidbody mainBone;

    public float tableSpeed;
    public float minTableDist;

    public float tableExplosiveForce;
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
        Rigidbody tableRB = PIPHelper.Instance.TableRB;
        PIPHelper.Instance.ToggleTablePhysics(true);
        while((tableRB.position - transform.position).magnitude > minTableDist)
        {
            tableRB.AddForce((transform.position - tableRB.position).normalized * tableSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        tableRB.constraints = RigidbodyConstraints.FreezeAll;
        controller.ToggleRagdoll(true);
        mainBone.AddExplosionForce(tableExplosiveForce, tableRB.position, 5);

        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
        PIPHelper.Instance.ResetTable();
    }
}
