using System;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public GameObject[] ClientPrefabs;

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
    public ZoneScript ZoneScript;

    public bool tutorialSpawner = false;
    private int tutorialClientCount = 0;

    public DrinkEffect TutorialDrinkEffect1;
    public GlassType TutorialGlass1;

    public DrinkEffect TutorialDrinkEffect2;
    public GlassType TutorialGlass2;
    public FruitType TutorialFruit2;

    public void SpawnClient()
    {
        if (tutorialSpawner)
        {
            GameObject client = Instantiate(ClientPrefabs[0], transform);
            ClientController controller = client.GetComponent<ClientController>();
            controller.Spawner = this;
            if (tutorialClientCount == 0)
            {
                controller.CustomDrinkEffect = TutorialDrinkEffect1;
                controller.CustomFruit = FruitType.NONE;
                controller.CustomGlass = TutorialGlass1;
            } 
            else if(tutorialClientCount == 1)
            {
                controller.CustomDrinkEffect = TutorialDrinkEffect2;
                controller.CustomFruit = TutorialFruit2;
                controller.CustomGlass = TutorialGlass2;
            }
            tutorialClientCount++;
        } 
        else
        {
            currentClientCount++;
            GameObject ClientPrefab = ClientPrefabs[UnityEngine.Random.Range(0, ClientPrefabs.Length)];
            GameObject client = Instantiate(ClientPrefab, transform);
            ClientController controller = client.GetComponent<ClientController>();
            controller.Spawner = this;
        }
    }

    void FixedUpdate()
    {
        if(tutorialSpawner)return;
        if (!ClientSpawned && currentClientCount < maxClientsToSpawn)
        {
            Invoke(nameof(SpawnClient), UnityEngine.Random.Range(minClientSpawnDelay, maxClientSpawnDelay));
            ClientSpawned = true;
        }
    }

    public void NotifyClientFinished()
    {
        if (!tutorialSpawner)
        {
            finishedClientCount++;
            if (finishedClientCount >= maxClientsToSpawn)
            {
                OnAllClientsFinished?.Invoke();
            }
        } 
        else
        {

        }  
    }
}
