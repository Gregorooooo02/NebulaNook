using UnityEngine;

public abstract class PipState : MonoBehaviour
{
    public PipController controller;
    public IdlePIP DefaultState;
    public float initialDelay;
    public abstract PipState RunState();
}
