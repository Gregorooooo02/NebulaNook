using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int currentDay = 1;
    public int currentQuota = 0;
    public int maxQuota = 100;
    public int baseClientToSpawn = 1;

    private int spawnersFinishedCount = 0;

    [SerializeField] private TMPro.TextMeshProUGUI quotaText;
    [SerializeField] private ClientSpawner[] clientSpawners;
    [SerializeField] private GameObject endOfDayScreenGood;
    [SerializeField] private GameObject endOfDayScreenBad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (SceneManager.sceneCount == 1)
            {
                SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        Scene gameScene = SceneManager.GetSceneByName("GameScene");
        if (gameScene.isLoaded)
        {
            InitializeGameScene();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InitializeGameScene();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "GameScene")
        {
            ClearGameSceneReferences();
        }
    }

    private void InitializeGameScene()
    {
        RefreshSceneReferences();
        UpdateQuotaText();
        SetupSpawners();
    }

    private void SetupSpawners()
    {
        if (clientSpawners == null || clientSpawners.Length == 0) return;

        spawnersFinishedCount = 0;

        foreach (var spawner in clientSpawners)
        {
            // Unsubscribe from previous events to avoid duplicates
            spawner.OnAllClientsFinished -= OnSpawnerFinished;

            spawner.maxClientsToSpawn = baseClientToSpawn + currentDay - 1;
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
        Debug.Log("Koniec dnia!");

        if (clientSpawners == null || clientSpawners.Length == 0) return;

        if (currentQuota >= maxQuota)
        {
            endOfDayScreenGood.SetActive(true);
            var quotaText = GameObject.Find("QuotaTextGood")?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (quotaText != null)
            {
                quotaText.text = $"{currentQuota}";
            }
        }
        else
        {
            endOfDayScreenBad.SetActive(true);
            var quotaText = GameObject.Find("QuotaTextBad")?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (quotaText != null)
            {
                quotaText.text = $"{currentQuota}";
            }
        }
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

    public void NextDay()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.FadeIntoScene("GameScene", ExecuteNextDayLogic);
        }
    }

    private void ExecuteNextDayLogic()
    {
        currentDay++;
        currentQuota = 0;
        spawnersFinishedCount = 0;
        maxQuota += 100;

        endOfDayScreenBad.SetActive(false);
        endOfDayScreenGood.SetActive(false);

        InitializeGameScene();
    }

    public void ResetGame()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.FadeIntoScene("GameScene", ExecuteResetGameLogic);
        }
    }

    private void ExecuteResetGameLogic()
    {
        currentDay = 1;
        currentQuota = 0;
        spawnersFinishedCount = 0;
        maxQuota = 100;

        if (endOfDayScreenGood != null) endOfDayScreenGood.SetActive(false);
        if (endOfDayScreenBad != null) endOfDayScreenBad.SetActive(false);

        InitializeGameScene();
    }

    private void RefreshSceneReferences()
    {
        Scene gameScene = SceneManager.GetSceneByName("GameScene");
        if (!gameScene.isLoaded)
        {
            Debug.LogWarning("GameScene nie jest załadowana!");
            return;
        }

        Debug.Log("Odświeżam referencje do obiektów w scenie...");

        // Wyczyść stare referencje
        quotaText = null;
        clientSpawners = null;
        endOfDayScreenGood = null;
        endOfDayScreenBad = null;
        spawnersFinishedCount = 0;

        // Znajdź quota text
        GameObject quotaObject = GameObject.FindGameObjectWithTag("QuotaText");
        if (quotaObject != null)
        {
            quotaText = quotaObject.GetComponent<TMPro.TextMeshProUGUI>();
            Debug.Log($"Znaleziono QuotaText: {quotaObject.name}");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu z tagiem 'QuotaText'");
            // Fallback - znajdź pierwszy TextMeshPro w scenie
            TMPro.TextMeshProUGUI[] allTexts = FindObjectsByType<TMPro.TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var text in allTexts)
            {
                if (text.gameObject.scene.name == "GameScene")
                {
                    quotaText = text;
                    Debug.Log($"Użyto fallback dla QuotaText: {text.gameObject.name}");
                    break;
                }
            }
        }

        // Znajdź spawners
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");
        if (spawnerObjects.Length > 0)
        {
            clientSpawners = new ClientSpawner[spawnerObjects.Length];
            for (int i = 0; i < spawnerObjects.Length; i++)
            {
                clientSpawners[i] = spawnerObjects[i].GetComponent<ClientSpawner>();
            }
            Debug.Log($"Znaleziono {spawnerObjects.Length} spawnerów");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektów z tagiem 'Spawner'");
            // Fallback - znajdź wszystkie ClientSpawner w scenie
            ClientSpawner[] allSpawners = FindObjectsByType<ClientSpawner>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var gameSceneSpawners = new System.Collections.Generic.List<ClientSpawner>();
            foreach (var spawner in allSpawners)
            {
                if (spawner.gameObject.scene.name == "GameScene")
                {
                    gameSceneSpawners.Add(spawner);
                }
            }
            clientSpawners = gameSceneSpawners.ToArray();
            Debug.Log($"Użyto fallback dla spawnerów: znaleziono {clientSpawners.Length}");
        }

        // Znajdź ekrany końca dnia
        endOfDayScreenGood = GameObject.FindGameObjectWithTag("GoodScreen");
        if (endOfDayScreenGood != null)
        {
            Debug.Log($"Znaleziono GoodScreen: {endOfDayScreenGood.name}");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu z tagiem 'GoodScreen'");
        }

        endOfDayScreenBad = GameObject.FindGameObjectWithTag("BadScreen");
        if (endOfDayScreenBad != null)
        {
            Debug.Log($"Znaleziono BadScreen: {endOfDayScreenBad.name}");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu z tagiem 'BadScreen'");
        }

        // Upewnij się, że ekrany są wyłączone
        if (endOfDayScreenGood != null) endOfDayScreenGood.SetActive(false);
        if (endOfDayScreenBad != null) endOfDayScreenBad.SetActive(false);

        Debug.Log($"RefreshSceneReferences zakończony. QuotaText: {quotaText != null}, Spawners: {clientSpawners?.Length ?? 0}, GoodScreen: {endOfDayScreenGood != null}, BadScreen: {endOfDayScreenBad != null}");
    }

    private void ClearGameSceneReferences()
    {
        quotaText = null;
        clientSpawners = null;
        endOfDayScreenGood = null;
        endOfDayScreenBad = null;
    }
}
