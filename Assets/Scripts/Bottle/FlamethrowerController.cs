using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlamethrowerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private InputActionReference[] fireActions;

    private bool isFiring = false;
    private Laser currentLaser = null;

    private void Awake()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isFiring = false;
        currentLaser = null;
        TutorialManager.Instance?.NotifyBlowtorchPicked();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (currentLaser != null)
        {
            StopFiring();
        }
    }

    private void Update()
    {
        if (!grabInteractable.isSelected) return;

        // Check if any of the fire actions are pressed
        bool fireCheck = false;
        foreach (var action in fireActions)
        {
            // Check if the value of the action is greater than threshold
            if (action.action.ReadValue<float>() > 0.5f)
            {
                fireCheck = true;
                break; // No need to check further if one action is pressed
            }
        }

        if (isFiring != fireCheck)
        {
            isFiring = fireCheck;
            if (isFiring)
            {
                StartLaser();
            }
            else
            {
                StopFiring();
            }
        }
    }

    private void StartLaser()
    {
        Debug.Log("Starting Flamethrower");
        currentLaser = CreateLaser();
        currentLaser.BeginLaser();
        TutorialManager.Instance?.NotifyBlowtorchTriggerPulled();
    }

    private void StopFiring()
    {
        Debug.Log("Stopping Flamethrower");
        currentLaser.EndLaser();
        currentLaser = null;
    }

    private Laser CreateLaser()
    {
        GameObject laserObject = Instantiate(laserPrefab, laserOrigin.position, laserOrigin.rotation, transform);
        return laserObject.GetComponent<Laser>();
    }
}
