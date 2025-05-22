using UnityEngine;

public class WaitForDrink : ClientState
{
    public LeaveState nextState;
    public bool Continue = false;

    private BarChairScript barChairScript;

    public override ClientState RunState()
    {
        if (Continue)
        {
            Continue = false;
            ClientSpawner.Instance.clientCount--;
            ChairManager.Instance.VacateChair(barChairScript);
            return nextState;
        }
        return this;
    }

    public void SetBarChair(BarChairScript barChair)
    {
        this.barChairScript = barChair;
    }
}
