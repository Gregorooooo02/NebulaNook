using UnityEngine;

public class WaitForDrink : ClientState
{
    public LeaveState nextState;
    public PassOut passOutState;
    public Explode explodeState;
    public Freeze freezeState;

    public bool Continue = false;

    public DrinkEffect DrinkEffect;

    private BarChairScript barChairScript;

    public override ClientState RunState()
    {
        if (Continue)
        {
            Continue = false;
            ClientSpawner.Instance.clientCount--;
            ChairManager.Instance.VacateChair(barChairScript);
            switch (DrinkEffect)
            {
                case DrinkEffect.MATTER:
                    return passOutState;
                case DrinkEffect.EXPLOSION:
                    return explodeState;
                case DrinkEffect.FREEZE:
                    return freezeState;
                default:
                    return nextState;       
            }
        }
        return this;
    }

    public void SetBarChair(BarChairScript barChair)
    {
        this.barChairScript = barChair;
    }
}
