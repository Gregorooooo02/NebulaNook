using UnityEngine;

public class PipController : MonoBehaviour
{
    public PipState CurrentState;

    private void FixedUpdate()
    {
        PipState nextState = CurrentState.RunState();
        if (nextState != null)
        {
            CurrentState = nextState;
        }
    }


}
