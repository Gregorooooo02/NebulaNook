using System.Collections;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public static ClientSpawner Instance;

    public GameObject ClientPrefab;

    [HideInInspector]
    public int clientCount = 0;
    public bool overrideClientLimit = false;
    public int maxClients = 0;

    public float minClientSpawnDelay;
    public float maxClientSpawnDelay;

    private bool ready = false;

    void Start()
    {
        Instance = this;
        if(!overrideClientLimit)
        {
            if(ChairManager.Instance == null)
            {
                StartCoroutine("WaitForChairManagerInstance");
                return;
            }
            maxClients = ChairManager.Instance.BarChairs.Length;
            ready = true;
        } 
    }

    IEnumerator WaitForChairManagerInstance()
    {
        while (true)
        {
            if(ChairManager.Instance == null)
            {
                yield return new WaitForSeconds(1);
            } 
            else
            {
                maxClients = ChairManager.Instance.BarChairs.Length;
                ready = true;
                break;
            }
        }
    }

    private void SpawnClient()
    {
        Instantiate(ClientPrefab, transform);
    }

    void FixedUpdate()
    {
        if(!ready) return;
        if(clientCount < maxClients)
        {
            Invoke("SpawnClient", Random.Range(minClientSpawnDelay, maxClientSpawnDelay));
            clientCount++;
        }
    }
}
