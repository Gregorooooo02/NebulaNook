using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class AnimateHandController : MonoBehaviour
{
    [SerializeField] InputActionReference gripInputAction;
    [SerializeField] InputActionReference triggerInputAction;

    [SerializeField] private XRBaseInteractor interactor;

    private Animator handAnimator;
    private float gripValue;
    private float triggerValue;

    private void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (handAnimator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return;
        }

        bool isHolding = interactor != null && interactor.hasSelection;

        gripValue = gripInputAction.action.ReadValue<float>();
        triggerValue = triggerInputAction.action.ReadValue<float>();

        if (isHolding)
        {
            gripValue = 1f;
            triggerValue = 1f;
        }

        handAnimator.SetFloat("Grip", gripValue);
        handAnimator.SetFloat("Trigger", triggerValue);
    }
}
