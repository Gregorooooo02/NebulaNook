using UnityEngine;

public class TasakSpawner : MonoBehaviour
{
    [Header("Tasak Settings")]
    [SerializeField] private GameObject tasakPrefab;
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("Detection")]
    [SerializeField] private LayerMask tasakLayer = -1;
    [SerializeField] private float detectionRadius = 0.1f;

    private GameObject tasakInstance;
    private bool isRespawning = false;

    private void Awake()
    {
        SpawnTasak();
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckTasakStatus), 1f, 0.5f);
    }

    private void OnDestroy()
    {
        if (tasakInstance != null)
        {
            Destroy(tasakInstance);
        }
        CancelInvoke();
    }

    public void SpawnTasak()
    {
        if (tasakInstance == null && !isRespawning)
        {
            tasakInstance = Instantiate(tasakPrefab, transform.position, Quaternion.identity);

            if (tasakInstance.TryGetComponent(out TasakTracker tasakTracker))
            {
                tasakTracker.Initialize(this);
            }
        }
    }

    private void CheckTasakStatus()
    {
        if (tasakInstance == null && !isRespawning)
        {
            StartRespawn();
        }
    }

    public void OnTasakDestroyed(GameObject tasak)
    {
        if (tasakInstance == tasak)
        {
            tasakInstance = null;
            RespawnTasak();
        }
    }

    private void StartRespawn()
    {
        isRespawning = true;
        Invoke(nameof(SpawnTasak), spawnDelay);
        isRespawning = false;
    }

    private void RespawnTasak()
    {
        isRespawning = false;
        SpawnTasak();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
