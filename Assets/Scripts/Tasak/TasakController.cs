using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TasakController : MonoBehaviour
{
    [Header("Tasak Settings")]
    [SerializeField] private GameObject blade;
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        blade.SetActive(true);
        TutorialManager.Instance?.NotifyCleaverPicked();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        blade.SetActive(false);
    }
}
