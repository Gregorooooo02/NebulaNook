using UnityEngine;

public class WaitForDrink : ClientState
{
    public LeaveState nextState;

    public override ClientState RunState()
    {
        return nextState;
    }
}
