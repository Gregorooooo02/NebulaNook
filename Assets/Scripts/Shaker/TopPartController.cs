using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TopPartController : MonoBehaviour
{
    [SerializeField] private Transform snapTarget;
    [SerializeField] private GameObject bottomPart;
    [SerializeField] private float snapDistance;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    public bool isAttached = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

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

        float dist = Vector3.Distance(transform.position, snapTarget.position);
        if (dist <= snapDistance)
        {
            AttachToBottom();
        }
    }

    void DetachFromBottom()
    {
        isAttached = false;
        transform.SetParent(null, true);
        rb.isKinematic = false;
    }

    void AttachToBottom()
    {
        rb.isKinematic = true;
        transform.position = snapTarget.position;
        transform.rotation = bottomPart.transform.rotation;
        transform.SetParent(bottomPart.transform, true);
        isAttached = true;
    }
}
