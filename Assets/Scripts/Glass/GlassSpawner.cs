using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GlassSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject glassPrefab;

    private Collider glassTrigger;

    private void Awake()
    {
        glassTrigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            // Check if player grabs the glass
            var interactor = other.GetComponentInChildren<XRBaseInteractor>();
            if (interactor != null && interactor.hasSelection)
            {
                // Player has grabbed the glass, spawn a new one
                SpawnGlass();
            }
        }
    }

    private void SpawnGlass()
    {
        GameObject newGlass = Instantiate(glassPrefab, transform.position, Quaternion.identity);
    }
}
