using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

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
    public Combution combutionState;
    public Rocket rocketState;
    public Transparency transparencyState;
    public Clone cloneState;
    public Rave raveState;
    public Possesion possesionState;    
    public Overgrowth overgrowthState;
    public Portal portalState;  
    public Negative negativeState; 
    public Aplause aplauseState;
    public Fireflies firefliesState;
    public Fireflies frostfliesState;
    public Dissolve dissolveState;
    public Meteor meteorState;
    public TableGlitch tableGlitchState;
    public Glitch glitchState;

    public bool Continue = false;

    public DrinkEffect DrinkEffect;

    private BarChairScript barChairScript;

    public override ClientState RunState()
    {
        if (Continue)
        {
            Continue = false;
            Controller.Spawner.ClientSpawned = false;
            Controller.Spawner.barChairScript.Occupied = false;
            Controller.Spawner.barChairScript.Occupier = null;
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
                case DrinkEffect.COMBUTION:
                    return combutionState;
                case DrinkEffect.OIL:
                    return rocketState;
                case DrinkEffect.TRANSPARENCY:
                    return transparencyState;
                case DrinkEffect.CLONE:
                    return cloneState;
                case DrinkEffect.RAVE:
                    return raveState;
                case DrinkEffect.POSSESION:
                    return possesionState;
                case DrinkEffect.OVERGROWTH:
                    return overgrowthState;
                case DrinkEffect.PORTAL:
                    return portalState;
                case DrinkEffect.NEGATIVE:
                    return negativeState;
                case DrinkEffect.APLAUSE:
                    return aplauseState;
                case DrinkEffect.FIREFLIES:
                    return firefliesState;
                case DrinkEffect.FROSTFLIES: 
                    return frostfliesState;
                case DrinkEffect.DISSOLVE:
                    return dissolveState;
                case DrinkEffect.METEOR:
                    return meteorState;
                case DrinkEffect.TABLE_GLITCH:
                    return tableGlitchState;
                case DrinkEffect.GLITCH:
                    return glitchState;
                case DrinkEffect.WATER:
                case DrinkEffect.EMPTY:
                    break;
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
