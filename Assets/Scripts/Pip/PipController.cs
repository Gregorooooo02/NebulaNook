using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipController : MonoBehaviour
{
    public PipState CurrentState;

    private Rigidbody[] Joints;
    private CharacterJoint[] CharacterJoints;

    public Animator animator;
    public Collider mainCollider;
    public Rigidbody mainBody;
    public XRGrabInteractable interactable;

    public GameObject Icon;
    public Material IconMaterial;

    private void Start()
    {
        Joints = GetComponentsInChildren<Rigidbody>();
        CharacterJoints = GetComponentsInChildren<CharacterJoint>();

        //ToggleRagdoll(false);
    }

    private void FixedUpdate()
    {
        PipState nextState = CurrentState.RunState();
        if (nextState != null)
        {
            CurrentState = nextState;
        }
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        mainCollider.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;

        interactable.enabled = !isRagdoll;

        foreach (var joint in Joints)
        {
            if(joint == mainBody) continue;
            joint.isKinematic = !isRagdoll;
        }
        if(isRagdoll) mainBody.isKinematic = true;
    }

    public void StiffenRagdoll()
    {
        SoftJointLimit newLimit = new SoftJointLimit();
        newLimit.limit = 0;
        foreach (CharacterJoint joint in CharacterJoints)
        {
            joint.swing1Limit = newLimit;
            joint.swing2Limit = newLimit;
            joint.highTwistLimit = newLimit;
            joint.lowTwistLimit = newLimit;
        }
    }

    public void DissableGravity()
    {
        foreach (Rigidbody rb in Joints)
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

    public void Drink(DrinkEffect effect)
    {
        if(CurrentState is IdlePIP)
        {
            IdlePIP idle = (IdlePIP)CurrentState;
            idle.SetDrinkEffect(effect);
            IconMaterial.SetTexture("_BaseMap", DrinkEffectMap.Instance.effectIcons[(int)effect]);
            Instantiate(Icon,transform.position,Quaternion.identity);
        }
    }
}
