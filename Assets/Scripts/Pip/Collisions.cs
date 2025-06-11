using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private PipScript pipScript;
    void OnTriggerEnter(Collider other)
    {
        if (!pipScript.IsGrabbed)
        {
            pipScript.TriggerFollowing();
        }
    }
}
