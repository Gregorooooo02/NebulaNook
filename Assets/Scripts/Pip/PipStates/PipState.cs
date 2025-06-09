using UnityEngine;

public abstract class PipState : MonoBehaviour
{
    public IdlePIP DefaultState;
    public abstract PipState RunState();
}
