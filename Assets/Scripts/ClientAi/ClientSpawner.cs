using System;
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

    public int maxClientsToSpawn = 5;
    [HideInInspector] public int currentClientCount = 0;
    private int finishedClientCount = 0;

    public event Action OnAllClientsFinished;

    private void SpawnClient()
    {
        currentClientCount++;
        GameObject client = Instantiate(ClientPrefab, transform);
        ClientController controller = client.GetComponent<ClientController>();
        controller.Spawner = this;
    }

    void FixedUpdate()
    {
        if (!ClientSpawned && currentClientCount < maxClientsToSpawn)
        {
            Invoke(nameof(SpawnClient), UnityEngine.Random.Range(minClientSpawnDelay, maxClientSpawnDelay));
            ClientSpawned = true;
        }
    }

    public void NotifyClientFinished()
    {
        finishedClientCount++;
        if (finishedClientCount >= maxClientsToSpawn)
        {
            OnAllClientsFinished?.Invoke();
        }
    }
}
