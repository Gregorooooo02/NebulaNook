using UnityEngine;

public abstract class PipState : MonoBehaviour
{
    public IdlePIP DefaultState;
    public float initialDelay;
    public abstract PipState RunState();
}
