using System;
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
    ENLARGEMENT,
    SHRINKING,
    SLOWNESS,
    QUICKNESS
}

public enum QuestSourceType
{
    HUMAN,
    ROBOT
}

public class ClientController : MonoBehaviour
{
    public ClientState CurrentState;
    public DrinkEffect DesiredDrinkEffect = DrinkEffect.COMBUTION;
    
    public SpeechBubble bubble;

    public bool IsWaiting = false;

    private NavMeshAgent _agent;
    private Animator _animator;
    public CapsuleCollider mainCollider;

    private bool _isWalking = false;
    private bool _isWaving = false;

    private WaitForDrink _drinkWaiting;

    public QuestSourceType SourceType = QuestSourceType.HUMAN;
    private Quests questsSource;

    private Rigidbody[] Joints;
    private CharacterJoint[] CharacterJoints;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _drinkWaiting = GetComponentInChildren<WaitForDrink>();

        var v = Enum.GetValues(typeof(DrinkEffect));
        DesiredDrinkEffect = (DrinkEffect)v.GetValue(Random.Range(1, v.Length));
        DesiredDrinkEffect = DrinkEffect.MATTER;

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
            bubble.SetText(questsSource.GetRandomQuestText(DesiredDrinkEffect));     
        }
        else if(IsWaiting && CurrentState is not WaitForDrink)
        {
            bubble.gameObject.SetActive(false);
            IsWaiting = false;
        }
    }

    private void CheckWalking()
    {
        float speed = _agent.velocity.magnitude;
        if (!_isWalking && speed > 0.1f)
        {
            _animator.SetBool("isWalking", true);
            _isWalking = true;
        }
        else if(_isWalking && speed <= 0.1f)
        {
            _animator.SetBool("isWalking", false);
            _isWalking = false;
        }
    }

    private void CheckWaiving()
    {
        if (!_isWaving && IsWaiting)
        {
            _animator.SetBool("isWaving", true);
            _isWaving = true;
        }
        else if(_isWaving && !IsWaiting)
        {
            _animator.SetBool("isWaving", false);
            _isWaving = false;
        }
    }

    public void Drink(DrinkEffect effect)
    {
        if (IsWaiting && DesiredDrinkEffect == effect)
        {
            _drinkWaiting.DrinkEffect = effect;
            _drinkWaiting.Continue = true;
        }
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        mainCollider.enabled = !isRagdoll;
        _animator.enabled = !isRagdoll;
        _agent.enabled = !isRagdoll;

        foreach(Rigidbody r in Joints)
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
            "It’s cold out in the void. Got anything to warm my bones? ",
            "Spent too long in cryosleep. Need something to bring me back to life. ",
            "I seek something that steams… like old Earth tea. ",
            "Surprise me with something that burns slow and steady. ",
            "I could use a warm touch in this sterile place. ",
            "Something above standard atmospheric temperature, perhaps? "
        },
        new string[]/*Freeze*/{
            "It's so stuffy in here! Do you have anything that can help? ",
            "Man I'm melting… Could you help me? ",
            "Ow! I thcalded my tong real bad... Can you helph? ",
            "Let’s go with something that bites… frost first. ",
            "A drink with entropy. Preferably below room temp. "
        },
        new string[]/*Live*/{
            "Got anything that tastes like rebirth?",
            "Make it brecht, make it bubble — I want to feel alive again. ",
            "Need a drink that kicks like life support coming back online. ",
            "Give me something bubbly and alive!",
            "*Yawn* That was a long mission, I'm beat. Gimme something to liven up! ",
            "Simulate vitality: carbonated, colorful, with a euphoric subroutine. "
        },
        new string[]/*Oil*/{
            "I crave something unrefined. Viscous. Black. ",
            "I require something with industrial-grade bitterness. ",
            "Been elbow-deep in engine guts. I need something to match the mood. ",
            "Give me something that smells like a machine bay — but smoother. ",
            "Something dark, thick, and flammable. Like home. "
        },
        new string[]/*Matter*/{
            "Something with substance. If it doesn’t crawl a little, I don’t want it. ",
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
            "I seek a drink that whispers… then screams. ",
            "I want something with kick. Real kick. Like reactor-core cascade. ",
            "I need a drink that punches harder than a missile. ",
            "I’m in the mood for a controlled detonation… "
        },
        new string[]/*Anihilation*/{
            "Pour me a drink that disappears like unpaid docking fees. ",
            "I don’t wanna taste it. I want it gone before I know I drank it. ",
            "I need something that leaves no trace — in the glass or in me. ",
            "I’m not here. Never was. Just serve me something that matches. ",
            "Something clean. Erasing. Like a signature wiped. ",
            "Let it be like falling into a black star. Final. Silent. ",
            "I want a drink that takes something with it when it leaves. "
        },
        new string[]/*Gravity Lift*/{
            "I’ve been dragging all day — got anything to lighten the load? ",
            "I want to rise — not burn, just drift. ",
            "Something that lifts the soul… or whatever I’ve got left of one. ",
            "A drink that forgets weight. That’s what I need. ",
            "Something like freefall, but in a glass. ",
            "I’d like something… unburdened. Something with altitude. "
        },
        new string[]/*ENLARGEMENT*/{
            "BIG! "
        },
        new string[]/*SHRINKING*/{
            "SMALL! "
        },
        new string[]/*SLOWNESS*/{
            "SLOW! "
        },
        new string[]/*QUICKNESS*/{
            "FAST! "
        }
    };

    public override string GetRandomQuestText(DrinkEffect effect)
    {
        int textIterations = texts[(int)(effect) - 1].Length;
        int index = UnityEngine.Random.Range(0, textIterations);
        return texts[(int)(effect) - 1][index];
    }
}

