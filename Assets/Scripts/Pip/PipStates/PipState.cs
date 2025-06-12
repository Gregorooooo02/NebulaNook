using UnityEngine;

public abstract class PipState : MonoBehaviour
{
    public PipController controller;
    public IdlePIP DefaultState;
    public float initialDelay = 0.5f;
    public abstract PipState RunState();
}
