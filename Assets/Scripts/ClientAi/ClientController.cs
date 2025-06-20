using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum DrinkEffect
{
    BLACK_HOLE, // Game over
    COMBUTION,
    FREEZE,
    LIFE,
    OIL,
    MATTER,
    EXPLOSION,
    ANIHILATION,
    GRAVITY_LIFT,
    TRANSPARENCY,
    CLONE,
    RAVE,
    POSSESION,
    OVERGROWTH,
    PORTAL,
    NEGATIVE,
    APLAUSE,
    FIREFLIES,
    FROSTFLIES,
    DISSOLVE,
    METEOR,
    TABLE_GLITCH,
    GLITCH,

    EMPTY,
    WATER
}

public enum GlassType
{
    COCKTAIL,
    WHISKY,
    PEG,
    MARTINI
}

public enum FruitType
{
    NONE,
    EXPLOSIVE,
    JUMPY
}

public enum QuestSourceType
{
    HUMAN,
    ROBOT
}

public class ClientController : MonoBehaviour
{
    public ClientState CurrentState;
    public bool useCustomDrinkEffect = false;
    public DrinkEffect CustomDrinkEffect = DrinkEffect.EXPLOSION;
    public DrinkEffect DesiredDrinkEffect = DrinkEffect.COMBUTION;
    public GlassType DesiredGlassType = GlassType.COCKTAIL;
    public FruitType DesiredFruitType = FruitType.EXPLOSIVE;
    
    public SpeechBubble bubble;

    public bool IsWaiting = false;

    private NavMeshAgent _agent;
    public Animator Animator;
    public CapsuleCollider mainCollider;

    private bool _isWalking = false;
    private bool _isWaving = false;

    private WaitForDrink _drinkWaiting;

    public QuestSourceType SourceType = QuestSourceType.HUMAN;
    public bool useIcons = false;
    private Quests questsSource;

    private Rigidbody[] Joints;
    private CharacterJoint[] CharacterJoints;
    public Transform GlassSocket;

    [HideInInspector]
    public ClientSpawner Spawner = null;
    public GameObject currentGlass;
    private bool _isGlassInHand = false;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _drinkWaiting = GetComponentInChildren<WaitForDrink>();

        SetDesiredDrink();

        Joints = GetComponentsInChildren<Rigidbody>();
        CharacterJoints = GetComponentsInChildren<CharacterJoint>();
        switch (SourceType)
        {
            case QuestSourceType.HUMAN:
                questsSource = new NormalQuests();
                break;
            case QuestSourceType.ROBOT:
                break;
        }

        ToggleRagdoll(false);
    }

    private void SetDesiredDrink()
    {
        if (useCustomDrinkEffect)
        {
            DesiredDrinkEffect = CustomDrinkEffect;
        }
        else
        {
            var v = Enum.GetValues(typeof(DrinkEffect));
            DesiredDrinkEffect = (DrinkEffect)v.GetValue(Random.Range(1, v.Length - 2));
        }

        var u = Enum.GetValues(typeof(GlassType));
        DesiredGlassType = (GlassType)u.GetValue(Random.Range(0, u.Length));
        var f = Enum.GetValues(typeof(FruitType));
        DesiredFruitType = (FruitType)f.GetValue(Random.Range(0, f.Length));
    }

    void FixedUpdate()
    {
        ClientState nextState = CurrentState.RunState();

        if (nextState != null)
        {
            CurrentState = nextState;
        }
        CheckWaiting();


        CheckWalking();
        CheckWaiving(); 
    }

    private void CheckWaiting()
    {
        if(!IsWaiting && CurrentState is WaitForDrink)
        {
            IsWaiting = true;
            bubble.gameObject.SetActive(true);
            if (useIcons)
            {
                bubble.SetIcon(DrinkEffectMap.Instance.effectIcons[(int)DesiredDrinkEffect]);
            } 
            else
            {
                bubble.SetText(questsSource.GetRandomQuestText(DesiredDrinkEffect));
            }
            Spawner.ZoneScript.EnableHolograms(DesiredGlassType, DesiredFruitType);
            RecipeBook.Instance.AddRecipe(DesiredDrinkEffect, this);
        }
        else if(IsWaiting && CurrentState is not WaitForDrink)
        {
            bubble.gameObject.SetActive(false);
            IsWaiting = false;
        }
    }

    public void Begone()
    {
        _drinkWaiting.DrinkEffect = (DrinkEffect)(2137);
        _drinkWaiting.Continue = true;
        Spawner.ZoneScript.DisableHolograms();
        RecipeBook.Instance.RemoveRecipe(DesiredDrinkEffect, this);
        if (!useCustomDrinkEffect) GameManager.Instance.DecrementQuota(10);
    }

    private void CheckWalking()
    {
        float speed = _agent.velocity.magnitude;
        if (!_isWalking && speed > 0.1f)
        {
            Animator.SetBool("isWalking", true);
            _isWalking = true;
        }
        else if(_isWalking && speed <= 0.1f)
        {
            Animator.SetBool("isWalking", false);
            _isWalking = false;
        }
    }

    private void CheckWaiving()
    {
        if (!_isWaving && IsWaiting)
        {
            StartCoroutine(WavingCoroutine());
            _isWaving = true;
        }
        else if(_isWaving && !IsWaiting)
        {
            Animator.SetBool("isWaving", false);
            _isWaving = false;
        }
    }

    private IEnumerator WavingCoroutine()
    {
        Animator.SetBool("isWaving", true);
        var wait = new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
        // Wait for the waving animation to finish
        yield return wait;
        Animator.SetBool("isWaving", false);
    }

    public void Drink(DrinkEffect effect/*, GlassType glass, FruitType fruit*/)
    {
        if (IsWaiting && DesiredDrinkEffect != DrinkEffect.EMPTY)
        {
            StartCoroutine(DrinkCoroutine(effect));
        }
    }

    private IEnumerator DrinkCoroutine(DrinkEffect effect)
    {
        if (!Animator.enabled)
        {
            Animator.enabled = true;
        }
        bubble.gameObject.SetActive(false);
        RecipeBook.Instance.RemoveRecipe(DesiredDrinkEffect, this);
        Animator.SetBool("isDrinking", true);
        var wait = new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
        // Wait for the drinking animation to finish
        yield return wait;
        Animator.SetBool("isDrinking", false);

        if (currentGlass != null)
        {
            GlassFiller filler = currentGlass.GetComponent<GlassFiller>();
            if (filler != null)
            {
                filler.currentFillAmount = 0f; // Empty the glass
                currentGlass.GetComponent<GlassTracker>().OnDrinkConsumed();
            }
        }

        if (!useCustomDrinkEffect)
        {
            CalculatePoints(effect);
        }
        _drinkWaiting.DrinkEffect = effect;
        _drinkWaiting.Continue = true;
    }

    public void GrabGlass()
    {
        if (currentGlass && GlassSocket && !_isGlassInHand)
        {
            currentGlass.transform.SetParent(GlassSocket);
            
            currentGlass.transform.localPosition = Vector3.zero;
            currentGlass.transform.localRotation = Quaternion.identity;

            Rigidbody glassRb = currentGlass.GetComponent<Rigidbody>();
            if (glassRb != null)
            {
                glassRb.isKinematic = true;
            }
            _isGlassInHand = true;
        }
    }

    public void ReleaseGlass()
    {
        if (currentGlass && _isGlassInHand)
        {
            currentGlass.transform.SetParent(null);
            Rigidbody glassRb = currentGlass.GetComponent<Rigidbody>();
            if (glassRb != null)
            {
                glassRb.isKinematic = false;
            }

            _isGlassInHand = false;
        }
    }

    private void CalculatePoints(DrinkEffect effect/*, GlassType glass, FruitType fruit*/)
    {
        if (DesiredDrinkEffect == effect)
        {
            GameManager.Instance.IncrementQuota(20);
        }
        else
        {
            GameManager.Instance.DecrementQuota(10);
        }

        // if (DesiredGlassType == glass)
        // {
        //     GameManager.Instance.IncrementQuota(5);
        // }
        // else
        // {
        //     GameManager.Instance.DecrementQuota(5);
        // }

        // if (DesiredFruitType == fruit)
        // {
        //     GameManager.Instance.IncrementQuota(10);
        // }
        // else
        // {
        //     GameManager.Instance.DecrementQuota(5);
        // }
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        mainCollider.enabled = !isRagdoll;
        Animator.enabled = !isRagdoll;
        _agent.enabled = !isRagdoll;

        foreach (Rigidbody r in Joints)
        {
            r.isKinematic = !isRagdoll;
        }
    }

    public void StiffenRagdoll()
    {
        SoftJointLimit newLimit = new SoftJointLimit();
        newLimit.limit = 0;
        foreach(CharacterJoint joint in CharacterJoints)
        {
            joint.swing1Limit = newLimit;
            joint.swing2Limit = newLimit;
            joint.highTwistLimit = newLimit;
            joint.lowTwistLimit = newLimit;
        }
    }

    public void DissableGravity()
    {
        foreach(Rigidbody rb in Joints)
        {
            rb.useGravity = false;
        }
    }

    public void FreezeRotation()
    {
        foreach (Rigidbody rb in Joints)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void DeleteYourself()
    {
        Spawner?.NotifyClientFinished();
        Destroy(gameObject);
    }
}


public abstract class Quests
{
    public abstract string GetRandomQuestText(DrinkEffect effect);
}

public class NormalQuests : Quests
{
    private static string[][] texts = new string[][]
    {
        new string[]/*Combution*/{
            "Ugh I'm done with those bland and boring drinks! Give me something with more spice would ya? ",
            "Ugh I didn't know **Scaldra Fruits** were so bitter! Quick give me something strong to burn out this terrible taste! ",
            "It�s cold out in the void. Got anything to warm my bones? ",
            "Spent too long in cryosleep. Need something to bring me back to life. ",
            "I seek something that steams� like old Earth tea. ",
            "Surprise me with something that burns slow and steady. ",
            "I could use a warm touch in this sterile place. ",
            "Something above standard atmospheric temperature, perhaps? "
        },
        new string[]/*Freeze*/{
            "It's so stuffy in here! Do you have anything that can help? ",
            "Man I'm melting� Could you help me? ",
            "Ow! I thcalded my tong real bad... Can you helph? ",
            "Let�s go with something that bites� frost first. ",
            "A drink with entropy. Preferably below room temp. "
        },
        new string[]/*Live*/{
            "Got anything that tastes like rebirth?",
            "Make it brecht, make it bubble � I want to feel alive again. ",
            "Need a drink that kicks like life support coming back online. ",
            "Give me something bubbly and alive!",
            "*Yawn* That was a long mission, I'm beat. Gimme something to liven up! ",
            "Simulate vitality: carbonated, colorful, with a euphoric subroutine. "
        },
        new string[]/*Oil*/{
            "I crave something unrefined. Viscous. Black. ",
            "I require something with industrial-grade bitterness. ",
            "Been elbow-deep in engine guts. I need something to match the mood. ",
            "Give me something that smells like a machine bay � but smoother. ",
            "Something dark, thick, and flammable. Like home. "
        },
        new string[]/*Matter*/{
            "Something with substance. If it doesn�t crawl a little, I don�t want it. ",
            "Gimme something heavy. I want it to glug when it hits the glass. ",
            "Non-Newtonian preferred. I want to feel the viscosity shift. ",
            "A fluid with personality. Resistance. Texture. ",
            "Pour me a drink that warps the light a little. ",
            "Something that clings to memory. A drink with weight. ",
            "Make it dense. Make it difficult. I want to earn the swallow. ",
            "Give me something heavy. I want to really feel the weight of the glass. "
        },
        new string[]/*Explosion*/{
            "Pour me something unstable. I want to feel it fight back. ",
            "Give me something that tastes like a weapons test gone right. ",
            "Let it be bright, wild, and barely contained. Like the moment before a star dies. ",
            "I seek a drink that whispers� then screams. ",
            "I want something with kick. Real kick. Like reactor-core cascade. ",
            "I need a drink that punches harder than a missile. ",
            "I�m in the mood for a controlled detonation� "
        },
        new string[]/*Anihilation*/{
            "Pour me a drink that disappears like unpaid docking fees. ",
            "I don�t wanna taste it. I want it gone before I know I drank it. ",
            "I need something that leaves no trace � in the glass or in me. ",
            "I�m not here. Never was. Just serve me something that matches. ",
            "Something clean. Erasing. Like a signature wiped. ",
            "Let it be like falling into a black star. Final. Silent. ",
            "I want a drink that takes something with it when it leaves. "
        },
        new string[]/*Gravity Lift*/{
            "I�ve been dragging all day � got anything to lighten the load? ",
            "I want to rise � not burn, just drift. ",
            "Something that lifts the soul� or whatever I�ve got left of one. ",
            "A drink that forgets weight. That�s what I need. ",
            "Something like freefall, but in a glass. ",
            "I�d like something� unburdened. Something with altitude. "
        },
        new string[]/*TRANSPARENCY*/{
            "INVIS! "
        },
        new string[]/*CLONE*/{
            "CLONE! "
        },
        new string[]/*RAVE*/{
            "CARAMELDANSEN! "
        },
        new string[]/*POSSESION*/{
            "POSSESION! "
        },
        new string[]/*OVERGROWTH*/{
            "OVERGROWTH! "
        },
        new string[]/*PORTAL*/{
            "PORTAL! "
        },
        new string[]/*NEGATIVE*/{
            "NEGATIVE! "
        },
        new string[]/*APLAUSE*/{
            "APLAUSE! "
        },
        new string[]/*FIREFLIES*/{
            "FIREFLIES! "
        },
        new string[]/*FROSTFLIES*/{
            "FROSTFLIES! "
        },
        new string[]/*DISSOLVE*/{
            "DISSOLVE! "
        },
        new string[]/*METEOR*/{
            "METEOR! "
        },
        new string[]/*TABLE_GLITCH*/{
            "TABLE_GLITCH! "
        },
        new string[]/*GLITCH*/{
            "GLITCH! "
        }
    };
    
    public override string GetRandomQuestText(DrinkEffect effect)
    {
        int textIterations = texts[(int)(effect) - 1].Length;
        int index = UnityEngine.Random.Range(0, textIterations);
        return texts[(int)(effect) - 1][index];
    }
}

