using UnityEngine;
using UnityEngine.SceneManagement;

public class PipSpawner : MonoBehaviour
{
    public static PipSpawner Instance;
    [SerializeField] private GameObject pipPrefab;
    public GameObject PipInstance;

    void Start()
    {
        Instance = this;
        SpawnPip();
    }

    public void SpawnPip()
    {
        GameObject pip = Instantiate(pipPrefab, transform.position, transform.rotation);
        PipInstance = pip;

        Scene gameScene = SceneManager.GetSceneByName("GameScene");
        if (gameScene.isLoaded)
        {
            SceneManager.MoveGameObjectToScene(pip, gameScene);
        }
    }

    public void DespawnPip()
    {
        if (PipInstance != null)
        {
            Destroy(PipInstance);
            PipInstance = null;
        }
    }
}
