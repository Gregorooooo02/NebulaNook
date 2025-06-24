using System.Collections;
using UnityEngine;

public class ClonePIP : PipState
{
    public GameObject ClonePrefab;

    private bool triggered = false;

    public float cloudDelay;
    public float duration;
    public float finalDelay;

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
        int cloneNum = PIPHelper.Instance.CloneSpawningLocations.Length;
        PipClone[] cloneList = new PipClone[cloneNum];
        for(int i = 0;i < cloneNum; i++)
        {
            cloneList[i] = Instantiate(ClonePrefab, PIPHelper.Instance.CloneSpawningLocations[i]).GetComponent<PipClone>();
        }
        yield return new WaitForSeconds(cloudDelay);
        foreach (var clone in cloneList)
        {
            clone.Model.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        foreach (var clone in cloneList)
        {
            clone.TriggerParticles();
        }
        yield return new WaitForSeconds(cloudDelay);
        foreach (var clone in cloneList)
        {
            clone.Model.SetActive(false);
        }
        yield return new WaitForSeconds(finalDelay);
        foreach (var clone in cloneList)
        {
            Destroy(clone.gameObject);
        }
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
