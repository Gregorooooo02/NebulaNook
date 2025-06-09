using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipScript : MonoBehaviour
{
    [SerializeField] public bool IsGrabbed;
    [SerializeField] private XRGrabInteractable pipInteractable;
    [SerializeField] private GameObject playerObject;
    private float currentTime = 0;
    private bool didStandUp = false;
    [SerializeField] private float timeToStandUp;

    void Update()
    {
        currentTime += Time.deltaTime;
        standUp();
    }

    private void Awake()
    {
        pipInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        pipInteractable.selectExited.AddListener((_) => IsGrabbed = false);
    }

    public void TriggerStandUp()
    {
        currentTime = 0;
        didStandUp = false;
    }

    private void standUp()
    {
        if (currentTime >= timeToStandUp && !didStandUp && !IsGrabbed)
        {
            gameObject.transform.LookAt(new Vector3(0, 0, 0));
            didStandUp = true;
        }
    }
}
