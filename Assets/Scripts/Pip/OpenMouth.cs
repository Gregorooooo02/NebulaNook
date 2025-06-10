using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OpenMouth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PipScript pipScript;
    [SerializeField] private GlassGrabState glassGrabState;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Glass" && !pipScript.IsGrabbed && glassGrabState.IsGrabbed) 
        {
            animator.SetBool("GlassInCollider", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Glass" && !pipScript.IsGrabbed)
        {
            animator.SetBool("GlassInCollider", false);
        }
        
    }
}
