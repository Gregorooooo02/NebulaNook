using UnityEngine;

public class WaitForDrink : ClientState
{
    public LeaveState nextState;
    public bool Continue = false;

    public override ClientState RunState()
    {
        if (Continue)
        {
            Continue = false;
            ClientSpawner.Instance.clientCount--;
            return nextState;
        }
        return this;
    }
}
