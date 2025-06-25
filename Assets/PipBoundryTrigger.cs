using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipBoundryTrigger : MonoBehaviour
{
    [SerializeField] private float graceTimer = 0.5f;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pip"))
        {
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)return;

            if(other.TryGetComponent<PipScript>(out PipScript pip))
            {
                //Debug.Log("Starting coroutine!");
                StartCoroutine(DelayedDestroy(other.gameObject, graceTimer));
            }
        }
    }

    IEnumerator DelayedDestroy(GameObject pip, float delay)
    {
        yield return new WaitForSeconds(delay);

        if(pip!= null)
        {
            XRGrabInteractable grabInteractable = pip.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                //Debug.Log("Is selected!");
                yield break;
            }

            Collider pipCollider = pip.GetComponent<Collider>();
            if (pipCollider != null && GetComponent<Collider>().bounds.Intersects(pipCollider.bounds))
            {
                //Debug.Log("Is inside!");
                yield break;
            }

            if (pip.TryGetComponent<PipScript>(out PipScript pipComponent))
            {
                //Debug.Log("Reset position");
                pipComponent.resetPosition();
            }
            
        }
    }
}
