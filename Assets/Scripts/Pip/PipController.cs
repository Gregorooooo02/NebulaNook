using UnityEngine;

public class PipController : MonoBehaviour
{
    public PipState CurrentState;


    private Rigidbody[] Joints;
    private CharacterJoint[] CharacterJoints;

    public Animator animator;

    private void Start()
    {
        Joints = GetComponentsInChildren<Rigidbody>();
        CharacterJoints = GetComponentsInChildren<CharacterJoint>();


        ToggleRagdoll(false);
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
        animator.enabled = !isRagdoll;

        foreach (var joint in Joints)
        {
            joint.isKinematic = !isRagdoll;
        }
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
}
