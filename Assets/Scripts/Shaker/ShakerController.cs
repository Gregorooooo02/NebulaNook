using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShakerController : MonoBehaviour
{
    [SerializeField] private Transform snapTarget;
    [SerializeField] private GameObject bottomPart;
    [SerializeField] private GameObject topPart;
    [SerializeField] private float snapDistance;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isAttached = true;

    void Awake()
    {
        rb = topPart.GetComponent<Rigidbody>();
        grabInteractable = topPart.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        AttachToBottom();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (isAttached)
        {
            DetachFromBottom();
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        StartCoroutine(PostReleaseCheck());
    }

    IEnumerator PostReleaseCheck()
    {
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;

        float dist = Vector3.Distance(topPart.transform.position, snapTarget.position);
        if (dist <= snapDistance)
        {
            AttachToBottom();
        }
    }

    void DetachFromBottom()
    {
        isAttached = false;
        topPart.transform.SetParent(null, true);
        rb.isKinematic = false;
    }

    void AttachToBottom()
    {
        rb.isKinematic = true;
        topPart.transform.position = snapTarget.position;
        topPart.transform.rotation = bottomPart.transform.rotation;
        topPart.transform.SetParent(bottomPart.transform, true);
        isAttached = true;
    }
}