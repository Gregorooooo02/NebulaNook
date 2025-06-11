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
    private bool didCollideAfterGrab = false;
    [SerializeField] private float timeToTriggerStandup;
    private Vector3 pipInitialPosition;

    public bool isOutsideBarZone = false;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (didCollideAfterGrab && currentTime > timeToTriggerStandup && !IsGrabbed)
        {
            followPlayer();
        }

    }

    public void ResetPosition()
    {
        print(transform.position);
        transform.position = pipInitialPosition;
    }

    public void TriggerFollowing()
    {
        currentTime = 0;
        didCollideAfterGrab = true;

        if (isOutsideBarZone)
        {
            isOutsideBarZone = false;
            ResetPosition();
        }
    }

    private void Awake()
    {
        pipInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        pipInteractable.selectExited.AddListener((_) => {
            IsGrabbed = false;
            didCollideAfterGrab = false;
        });
        pipInitialPosition = transform.position;
        pipInitialPosition.y += 0.1f;
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
