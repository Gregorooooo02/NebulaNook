using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GlassGrabState : MonoBehaviour
{
    public bool IsGrabbed;
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        grabInteractable.selectExited.AddListener((_) => IsGrabbed = false);
    }
}
