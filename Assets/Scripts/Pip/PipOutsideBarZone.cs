using UnityEngine;

public class PipOutsideBarZone : MonoBehaviour
{
    private PipSpawner pipSpawner;

    void Start()
    {
        pipSpawner = FindAnyObjectByType<PipSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (pipSpawner.PipInstance != null && other.tag == "Pip")
        {
            pipSpawner.PipInstance.GetComponent<PipScript>().isOutsideBarZone = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (pipSpawner.PipInstance != null && other.tag == "Pip")
        {
            pipSpawner.PipInstance.GetComponent<PipScript>().isOutsideBarZone = true;
        }
    }
}
