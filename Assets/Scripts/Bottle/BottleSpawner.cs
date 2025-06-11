using UnityEngine;

public class BottleSpawner : MonoBehaviour
{
    [Header("Bottle Settings")]
    [SerializeField] private GameObject bottlePrefab;
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("Detection")]
    [SerializeField] private LayerMask bottleLayer = -1;
    [SerializeField] private float detectionRadius = 0.1f;

    private GameObject bottleInstance;
    private bool isRespawning = false;

    private void Awake()
    {
        SpawnBottle();
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckBottleStatus), 1f, 0.5f);
    }

    private void OnDestroy()
    {
        if (bottleInstance != null)
        {
            Destroy(bottleInstance);
        }
        CancelInvoke();
    }

    public void SpawnBottle()
    {
        if (bottleInstance == null && !isRespawning)
        {
            bottleInstance = Instantiate(bottlePrefab, transform.position, Quaternion.identity);

            if (bottleInstance.TryGetComponent(out BottleTracker bottleTracker))
            {
                bottleTracker.Initialize(this);
            }
        }
    }

    private void CheckBottleStatus()
    {
        if (bottleInstance == null && !isRespawning)
        {
            StartRespawn();
        }
    }

    public void OnBottleDestroyed()
    {
        bottleInstance = null;
        if (!isRespawning)
        {
            StartRespawn();
        }
    }

    private void StartRespawn()
    {
        if (isRespawning) return;
        isRespawning = true;
        Invoke(nameof(RespawnBottle), spawnDelay);
    }

    private void RespawnBottle()
    {
        isRespawning = false;
        SpawnBottle();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
