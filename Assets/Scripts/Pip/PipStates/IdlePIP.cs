using UnityEngine;

public class IdlePIP : PipState
{
    public bool GotDrink;
    public DrinkEffect drinkEffect;

    private bool AnimationPlaying = true;

    public float minIdleCooldown;
    public float maxIdleCooldown; 
    private float currentCooldownTime;
    private float currentTime;

    public Animator animator;

    public BlackHolePIP blackHolePIP;
    public CombustPIP combustState;
    public FreezePIP freezeState;
    public BubblesPIP bubblesState;
    public RocketPIP rocketState;
    public PassOutPIP passOutState;
    public ExplosionPIP expsionState;
    public AnihilationPIP anihilationState;
    public GravityPIP gravityState;
    public DissolvePIP dissolveState;
    public TransparencyPIP transparencyState;
    public NegativePIP negativeState;
    public ClonePIP cloneState;
    public RavePIP raveState;
    public PossesionPIP possesionState;
    public OvergrowthPIP overgrowthState;
    public PortalPIP portalState;
    public AplausePIP aplauseState;
    public FirefliesPIP firefliesState;
    public FrostfliesPIP frostfliesState;
    public MeteorPIP meteorState;

    public GlitchPIP glitchState;

    public override PipState RunState()
    {
/*        if(!AnimationPlaying && currentTime >= currentCooldownTime)
        {
            PlayRandomIdle();
        } 
        else if(currentTime < currentCooldownTime)
        {
            currentTime += Time.fixedDeltaTime;
        }*/

        if (GotDrink)
        {
            GotDrink = false;
            switch (drinkEffect)
            {
                case DrinkEffect.BLACK_HOLE:
                    return blackHolePIP;
                case DrinkEffect.COMBUTION:
                    return combustState;
                case DrinkEffect.FREEZE:
                    return freezeState;
                case DrinkEffect.LIFE:
                    return bubblesState;
                case DrinkEffect.OIL:
                    return rocketState;
                case DrinkEffect.MATTER:
                    return passOutState;
                case DrinkEffect.EXPLOSION:
                    return expsionState;
                case DrinkEffect.ANIHILATION:
                    return anihilationState;
                case DrinkEffect.GRAVITY_LIFT:
                    return gravityState;
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

                    break;
                case DrinkEffect.GLITCH:
                    return glitchState;
                case DrinkEffect.WATER:
                case DrinkEffect.EMPTY:
                default:
                    return this;
            }
        }

        return this;
    }


    public void PlayRandomIdle()
    {
        currentCooldownTime = Random.Range(minIdleCooldown, maxIdleCooldown);
        //animator.SetBool(); play random idle animation here
        AnimationPlaying = true;
    }


    public void AnimationEnded()
    {
        //Setup it so every time the idle animation ends this method is called
        AnimationPlaying = false;
        currentTime = 0;
    }

    public void SetDrinkEffect(DrinkEffect drink)
    {
        this.drinkEffect = drink;
        GotDrink = true;
    }

}
