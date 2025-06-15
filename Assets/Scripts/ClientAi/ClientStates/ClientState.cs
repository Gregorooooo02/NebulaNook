using UnityEngine;

public abstract class ClientState : MonoBehaviour
{
    public ClientController Controller;
    public float initialDelay = 0.5f;
    public abstract ClientState RunState();
}
