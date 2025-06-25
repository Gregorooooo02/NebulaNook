using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OpenMouth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PipSpawner pipSpawner;

    public PipScript pipScript;

    public Collider DrinkDetector;
    public Collider StreamCollider;
    public Collider MainCollider;
    public Rigidbody mainBody;

    private void Start()
    {
        Physics.IgnoreCollision(DrinkDetector, MainCollider);
    }


    void OnTriggerEnter(Collider other)
    {
/*        if (pipSpawner == null)
        {
            pipSpawner = FindAnyObjectByType<PipSpawner>();
        }

        if (pipSpawner.PipInstance != null && other.tag == "Glass")
        {
            bool isPipGrabbed = pipSpawner.PipInstance.GetComponent<PipScript>().IsGrabbed;
            bool isGlassGrabbed = other.GetComponent<GlassGrabState>().IsGrabbed;

            if ((isPipGrabbed && isGlassGrabbed) || (!isPipGrabbed && isGlassGrabbed))
            {
                animator.SetBool("GlassInCollider", true);
            }
        }*/

        if(other.TryGetComponent<GlassGrabState>(out GlassGrabState state))
        {
            if(state.IsGrabbed && !pipScript.IsGrabbed)
            {
                animator.SetBool("GlassInCollider", true);
                if(DrinkDetector) DrinkDetector.enabled = true;
                if(StreamCollider) StreamCollider.enabled = false;
                if (mainBody) mainBody.constraints = RigidbodyConstraints.FreezePosition;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
/*        if (pipSpawner == null)
        {
            pipSpawner = FindAnyObjectByType<PipSpawner>();
        }

        if (pipSpawner.PipInstance != null && other.tag == "Glass")
        {
            animator.SetBool("GlassInCollider", false);
        }*/

        if(other.TryGetComponent<GlassGrabState>(out GlassGrabState state))
        {
            animator.SetBool("GlassInCollider", false);
            if (DrinkDetector) DrinkDetector.enabled = false;
            if (StreamCollider) StreamCollider.enabled = true;
            if (mainBody) mainBody.constraints = RigidbodyConstraints.None;
        }
        
    }
}
