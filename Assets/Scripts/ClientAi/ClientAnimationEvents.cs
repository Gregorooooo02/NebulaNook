using UnityEngine;

public class ClientAnimationEvents : MonoBehaviour
{
    public ClientController clientController;

    void Start()
    {
        if (clientController == null)
        {
            clientController = GetComponentInParent<ClientController>();
            if (clientController == null)
            {
                Debug.LogError("ClientController not found in parent hierarchy!");
            }
        }
    }

    public void OnGrabGlass()
    {
        if (clientController != null)
        {
            clientController.GrabGlass();
        }
    }

    public void OnReleaseGlass()
    {
        if (clientController != null)
        {
            clientController.ReleaseGlass();
        }
    }
}
