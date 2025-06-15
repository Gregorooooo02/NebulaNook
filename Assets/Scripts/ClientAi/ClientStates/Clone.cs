using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Clone : ClientState
{
    public NavMeshAgent Agent;

    public float cloneSpawnDelay;
    private float _currentTime = 0.0f;

    public GameObject clientClonePrefab;
    private List<CloneControler> clones = new List<CloneControler>();

    private bool isWalking = false;
    private bool isDone = false;

    public float MinPointDist = 0.75f;
    public GameObject Parent;

    public Transform[] spawnLocations;
    private int _currentIndex = 0;

    public float destinationPollingTime = 0.5f;
    private bool triggered = false; 

    public override ClientState RunState()
    {
/*        if (!isDone)
        {
            _currentTime += Time.fixedDeltaTime;
            if(_currentTime > cloneSpawnDelay)
            {
                clones.Add(Instantiate(clientClonePrefab, spawnLocations[_currentIndex].position, Parent.transform.localRotation).GetComponent<CloneControler>());
                _currentTime = 0;
                _currentIndex++;
                if(_currentIndex >= spawnLocations.Length) isDone = true;
            }
            return this;
        }
        if (isWalking)
        {
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                isWalking = false;
                Controller.Spawner?.NotifyClientFinished();
                Destroy(Parent);
            }

        } else
        {
            Agent.SetDestination(Controller.Spawner.Exit.transform.position);
            foreach (var clone in clones)
            {
                clone.Exit(Controller.Spawner.Exit.transform.position);
            }
            isWalking = true;
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
        foreach (var pos in spawnLocations)
        {
            clones.Add(Instantiate(clientClonePrefab, pos.position, Parent.transform.localRotation).GetComponent<CloneControler>());
            yield return new WaitForSeconds(cloneSpawnDelay);
        }
        Agent.SetDestination(Controller.Spawner.Exit.transform.position);
        foreach (var clone in clones)
        {
            clone.Exit(Controller.Spawner.Exit.transform.position);
        }
        yield return new WaitForSeconds(destinationPollingTime);
        while (Agent.remainingDistance > MinPointDist)
        {
            yield return new WaitForSeconds(destinationPollingTime);
        }
        Controller.Spawner?.NotifyClientFinished();
        Destroy(Parent);
    }
}
