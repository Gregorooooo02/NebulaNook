using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TopPartController : MonoBehaviour
{
    [SerializeField] private Transform snapTarget;
    [SerializeField] private GameObject bottomPart;
    [SerializeField] private float snapDistance;
    [SerializeField] private GlassFiller glassFiller;
    [SerializeField] private GlassPourController glassPourController;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    public bool isAttached = true;

    private ShakerAudio topAudio;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        AttachToBottom();

        topAudio = GetComponent<ShakerAudio>();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (isAttached)
        {
            enableLiquidInteraction();
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
            disableLiquidInteraction();
            AttachToBottom();
        }
    }

    void DetachFromBottom()
    {
        TutorialManager.Instance?.NotifyShakerOpened();
        isAttached = false;
        transform.SetParent(null, true);
        rb.isKinematic = false;

        topAudio?.PlayDetachSound();
    }

    void AttachToBottom()
    {
        TutorialManager.Instance?.NotifyShakerClosed();
        rb.isKinematic = true;
        transform.position = snapTarget.position;
        transform.rotation = bottomPart.transform.rotation;
        transform.SetParent(bottomPart.transform, true);
        isAttached = true;

        topAudio?.PlayAttachSound();
    }

    void disableLiquidInteraction()
    {
        glassFiller.fillSpeed = 0.0f;
        if (glassPourController.IsPouring)
        {
            glassPourController.StopStream();
        }
        glassPourController.enabled = false;
    }

    void enableLiquidInteraction()
    {
        glassFiller.fillSpeed = 0.5f;
        glassPourController.enabled = true;
    }
}
