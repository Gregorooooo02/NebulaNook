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


    public CombustPIP combustState;

    public override PipState RunState()
    {
        if(!AnimationPlaying && currentTime >= currentCooldownTime)
        {
            PlayRandomIdle();
        } 
        else if(currentTime < currentCooldownTime)
        {
            currentTime += Time.fixedDeltaTime;
        }

        if (GotDrink)
        {
            switch (drinkEffect)
            {
                case DrinkEffect.BLACK_HOLE:

                    break;
                case DrinkEffect.COMBUTION:
                    return combustState;
                case DrinkEffect.FREEZE:

                    break;
                case DrinkEffect.LIFE:

                    break;
                case DrinkEffect.OIL:

                    break;
                case DrinkEffect.MATTER:

                    break;
                case DrinkEffect.EXPLOSION:

                    break;
                case DrinkEffect.ANIHILATION:

                    break;
                case DrinkEffect.GRAVITY_LIFT:

                    break;
                case DrinkEffect.TRANSPARENCY:

                    break;
                case DrinkEffect.CLONE:

                    break;
                case DrinkEffect.RAVE:

                    break;
                case DrinkEffect.POSSESION:

                    break;
                case DrinkEffect.OVERGROWTH:

                    break;
                case DrinkEffect.PORTAL:

                    break;
                case DrinkEffect.NEGATIVE:

                    break;
                case DrinkEffect.APLAUSE:

                    break;
                case DrinkEffect.FIREFLIES:

                    break;
                case DrinkEffect.FROSTFLIES:

                    break;
                case DrinkEffect.DISSOLVE:

                    break;
                case DrinkEffect.METEOR:

                    break;
                case DrinkEffect.TABLE_GLITCH:

                    break;
                case DrinkEffect.GLITCH:

                    break;
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
