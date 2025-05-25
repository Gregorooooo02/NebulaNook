using UnityEngine;

public class WaitForDrink : ClientState
{
    public LeaveState nextState;
    public PassOut passOutState;
    public Explode explodeState;
    public Freeze freezeState;
    public LowGravity low_gravityState;
    public Speed speedState;
    public Slow slowState;
    public Anihilation anihilationState;
    public Bubbles bubblesState;
    public SizeChange bigState;
    public SizeChange smallState;
    public Blackhole blackHoleState;

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
                case DrinkEffect.GRAVITY_LIFT:
                    return low_gravityState;
                case DrinkEffect.QUICKNESS:
                    return speedState;
                case DrinkEffect.SLOWNESS:
                    return slowState;
                case DrinkEffect.ANIHILATION:
                    return anihilationState;
                case DrinkEffect.LIFE:
                    return bubblesState;
                case DrinkEffect.ENLARGEMENT:
                    return bigState;
                case DrinkEffect.SHRINKING:
                    return smallState;
                case DrinkEffect.BLACK_HOLE:
                    return blackHoleState;
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
