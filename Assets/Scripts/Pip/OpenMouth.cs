using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OpenMouth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PipSpawner pipSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (pipSpawner == null)
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (pipSpawner == null)
        {
            pipSpawner = FindAnyObjectByType<PipSpawner>();
        }

        if (pipSpawner.PipInstance != null && other.tag == "Glass")
        {
            animator.SetBool("GlassInCollider", false);
        }
        
    }
}
