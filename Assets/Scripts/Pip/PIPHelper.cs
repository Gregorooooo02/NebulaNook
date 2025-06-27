using UnityEngine;

public class PIPHelper : MonoBehaviour
{
    public static PIPHelper Instance;

    public Transform MeteorSpawn;
    public Transform[] CloneSpawningLocations;

    public GameObject TableHandle;
    public Rigidbody TableRB;

    public bool enableCollisionsForMiniTable = false;

    public GameObject gameOverAnimationObject;
    public GameObject[] objectsToDisable;
    public AudioSource Jukebox;

    public bool TriggerGameOver = false;
 
    private void Start()
    {
        Instance = this;
    }

    public void ToggleTablePhysics(bool isActive)
    {
        TableRB.isKinematic = !isActive;
        if(!enableCollisionsForMiniTable)TableRB.useGravity = !isActive;
        if(enableCollisionsForMiniTable)TableHandle.GetComponent<BoxCollider>().enabled = isActive;
    }

    public void ResetTable()
    {
        ToggleTablePhysics(false);
        TableHandle.transform.localPosition = Vector3.zero;
        TableHandle.transform.localRotation = Quaternion.identity;
    }

    public void StartGameOverSequence()
    {
        Light[] sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach(Light light in sceneLights)
        {
            light.enabled = false;
        }

        foreach(GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        // Jukebox.enabled = false;

        PipCoordinator co = FindAnyObjectByType<PipCoordinator>();

        co.gameObject.SetActive(false);

        gameOverAnimationObject.SetActive(true);
    }

    public void Update()
    {
        if (TriggerGameOver)
        {
            TriggerGameOver = false;
            StartGameOverSequence();
        }
    }
}
