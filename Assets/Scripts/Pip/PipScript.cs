using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipScript : MonoBehaviour
{
    [SerializeField] public bool IsGrabbed;
    [SerializeField] private XRGrabInteractable pipInteractable;
    private float currentTime = 0;
    [SerializeField] private float timeToTriggerStandup;
    private Vector3 pipInitialPosition;
    private Vector3 lastPosition;
    private bool isOutsideBarZone = false;

    void Update()
    {
        if (IsGrabbed || isPositionDifferent())
        {
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        lastPosition = transform.position;

        if (currentTime > timeToTriggerStandup && !IsGrabbed)
        {
            followPlayer();
        }
    }

    void OnTriggerExit(Collider other)
    {
/*        if (other.tag == "PipBarCollider")
        {
            isOutsideBarZone = true;
        }
        else
        {
            if (isOutsideBarZone)
            {
                isOutsideBarZone = false;
                resetPosition();
            }
        }*/
    }

    public void resetPosition()
    {
        transform.position = pipInitialPosition;
        GetComponent<Animator>().SetBool("GlassInCollider", false);
    }

    private void Awake()
    {
        pipInteractable.selectEntered.AddListener((_) => IsGrabbed = true);
        pipInteractable.selectExited.AddListener((_) => IsGrabbed = false);
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

    private bool isPositionDifferent()
    {
        return Math.Abs(transform.position.x - lastPosition.x) > 0.01
            && Math.Abs(transform.position.y - lastPosition.y) > 0.01
            && Math.Abs(transform.position.z - lastPosition.z) > 0.01;
    }
}
