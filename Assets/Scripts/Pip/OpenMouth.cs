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

        if (pipSpawner.PipInstance != null)
        {
            bool isGrabbed = pipSpawner.PipInstance.GetComponent<PipScript>().IsGrabbed;
            if (other.tag == "Glass" && !isGrabbed) 
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

        if (pipSpawner.PipInstance != null)
        {
            
            bool isGrabbed = pipSpawner.PipInstance.GetComponent<PipScript>().IsGrabbed;
            if (other.tag == "Glass" && !isGrabbed)
            {
                animator.SetBool("GlassInCollider", false);
            }
        }
        
    }
}
