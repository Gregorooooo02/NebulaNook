using UnityEngine;

public class PipOutsideBarZone : MonoBehaviour
{
    [SerializeField] private PipScript pipScript;

    void OnTriggerEnter(Collider other)
    {
        pipScript.isOutsideBarZone = false;
    }
    void OnTriggerExit(Collider other)
    {
        pipScript.isOutsideBarZone = true;
    }
}
