using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipScript : MonoBehaviour
{
    [SerializeField] public bool IsGrabbed;
    [SerializeField] private XRGrabInteractable pipInteractable;
    
    private void Awake()
    {
        pipInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        pipInteractable.selectExited.AddListener((_) => IsGrabbed = false);
    }
}
