using System.Collections;
using UnityEngine;

public class Freeze : ClientState
{
    public float TimeToDisapear;
    private float _currentTime = 0;

    private bool triggered = false;

    public SkinnedMeshRenderer[] materialsToChange;
    public Material FrozenMaterial;

    public GameObject FreezeEffect;
    public GameObject parent;
    public override ClientState RunState()
    {
/*        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                return this;
            }

            Controller.Spawner?.NotifyClientFinished();
            Destroy(parent);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            GetComponentInParent<ClientController>().StiffenRagdoll();
            Instantiate(FreezeEffect, transform);
            foreach(SkinnedMeshRenderer m in materialsToChange)
            {
                m.material = FrozenMaterial;
            }
            triggered = true;
        }*/


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
        Controller.ToggleRagdoll(true);
        Controller.StiffenRagdoll();
        Instantiate(FreezeEffect, transform);
        foreach (SkinnedMeshRenderer m in materialsToChange)
        {
            m.material = FrozenMaterial;
        }
        yield return new WaitForSeconds(TimeToDisapear);
        Controller.Spawner?.NotifyClientFinished();
        Destroy(parent);
    }

}
