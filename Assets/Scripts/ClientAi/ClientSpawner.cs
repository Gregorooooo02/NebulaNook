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

    public Transform MeteorSpawnPoint;

    private void SpawnClient()
    {
        GameObject client = Instantiate(ClientPrefab, transform);
        ClientController controller = client.GetComponent<ClientController>();
        controller.Spawner = this;
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
