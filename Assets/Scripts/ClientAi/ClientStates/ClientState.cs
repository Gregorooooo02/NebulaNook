using UnityEngine;

public abstract class ClientState : MonoBehaviour
{
    public ClientController Controller;
    public abstract ClientState RunState();
}
