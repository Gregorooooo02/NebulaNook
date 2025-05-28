using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandController : MonoBehaviour
{
    [SerializeField] InputActionReference gripInputAction;
    [SerializeField] InputActionReference triggerInputAction;

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

        AnimateGrip();
        AnimateTrigger();
    }

    private void AnimateGrip()
    {
        gripValue = gripInputAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

    private void AnimateTrigger()
    {
        triggerValue = triggerInputAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
    }
}
