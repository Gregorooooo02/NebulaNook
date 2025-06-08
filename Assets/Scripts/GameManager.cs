using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Quota")]
    [SerializeField] private TMPro.TextMeshProUGUI quotaText;
    [Header("Client Spawners")]
    [SerializeField] private ClientSpawner[] clientSpawners;

    [Header("Game Settings")]
    public int currentDay = 1;
    public int currentQuota = 0;
    public int maxQuota = 100;

    private int spawnersFinishedCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateQuotaText();
        foreach (var spawner in clientSpawners)
        {
            spawner.OnAllClientsFinished += OnSpawnerFinished;
        }
    }

    private void OnSpawnerFinished()
    {
        spawnersFinishedCount++;

        if (spawnersFinishedCount >= clientSpawners.Length)
        {
            HandleEndOfDay();
        }
    }

    private void HandleEndOfDay()
    {
        Debug.Log("Koniec dnia! Przechodzimy do kolejnego dnia.");
        quotaText.text = "END OF DAY!";
    }

    public void IncrementQuota(int amount)
    {
        currentQuota += amount;
        UpdateQuotaText();
    }

    public void DecrementQuota(int amount)
    {
        currentQuota -= amount;
        UpdateQuotaText();
    }

    public void UpdateQuotaText()
    {
        if (quotaText != null)
        {
            quotaText.text = $"{currentQuota} / {maxQuota}";
        }
    }
}
