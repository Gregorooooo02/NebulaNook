using UnityEngine;
using UnityEngine.AI;

public class ClientScript : MonoBehaviour
{
    public NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.SetDestination(new Vector3(0, 1, 6));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
