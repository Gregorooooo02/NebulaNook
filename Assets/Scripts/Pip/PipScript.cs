using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipScript : MonoBehaviour
{
    [SerializeField] public bool IsGrabbed;
    [SerializeField] private XRGrabInteractable pipInteractable;
    [SerializeField] private GameObject playerObject;
    private float currentTime = 0;
    public bool didCollideAfterGrab = false;
    [SerializeField] private float timeToTriggerStandup;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (didCollideAfterGrab && currentTime > timeToTriggerStandup && !IsGrabbed)
        {
            followPlayer();
        }

    }

    public void TriggerFollowing()
    {
        currentTime = 0;
        didCollideAfterGrab = true;
    }

    private void Awake()
    {
        pipInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        pipInteractable.selectExited.AddListener((_) => {
            IsGrabbed = false;
            didCollideAfterGrab = false;
        });
    }

    private void followPlayer()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 atPlayer = cameraPos - transform.position;
        atPlayer.y = 0;

        Quaternion rotation = Quaternion.LookRotation(atPlayer);
        transform.rotation = rotation;
        
    }
}
