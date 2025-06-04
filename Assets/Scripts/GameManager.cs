using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Quota")]
    [SerializeField] private TMPro.TextMeshProUGUI quotaText;
    public int currentQuota = 0;
    public int maxQuota = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateQuotaText();
    }

    public void IncrementQuota(int amount)
    {
        currentQuota += amount;
        if (currentQuota > maxQuota)
        {
            currentQuota = maxQuota; // Ensure we don't exceed max quota
        }
        UpdateQuotaText();
    }

    public void DecrementQuota(int amount)
    {
        currentQuota -= amount;
        if (currentQuota < 0)
        {
            currentQuota = 0; // Ensure we don't go below zero
        }
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
