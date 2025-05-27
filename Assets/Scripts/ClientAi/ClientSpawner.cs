using System.Collections;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public GameObject ClientPrefab;

    public BarChairScript barChairScript;
    public GameObject Exit;
    public bool ClientSpawned = false;

    public float minClientSpawnDelay;
    public float maxClientSpawnDelay;

    private void SpawnClient()
    {
        GameObject client = Instantiate(ClientPrefab, transform);
        client.GetComponent<ClientController>().Spawner = this;
    }

    void FixedUpdate()
    {
        if(!ClientSpawned)
        {
            Invoke("SpawnClient", Random.Range(minClientSpawnDelay, maxClientSpawnDelay));
            ClientSpawned = true;
        }
    }
}
