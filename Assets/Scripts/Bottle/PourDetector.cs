using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PourDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pourOrigin;
    [SerializeField] private GameObject streamPrefab;
    [SerializeField] private XRGrabInteractable grabInteractable;

    [Header("Pouring Settings")]
    [SerializeField] public int pourThreshold = 45;

    private bool isPouring = false;
    private Stream currentStream = null;
    private BottleAudio bottleAudio;

    [SerializeField] private bool shouldParentStream = true;

    private void Awake()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);

        bottleAudio = GetComponent<BottleAudio>();
    }

    // This method is called when the object is grabbed
    // it will reset the pouring state and current stream
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isPouring = false;
        currentStream = null;
        TutorialManager.Instance?.NotifyOilPicked();
    }

    // This method is called when the object is released
    // it will stop the pouring action if it was in progress
    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (currentStream != null)
        {
            EndPour();
        }
    }

    private void Update()
    {
        if (!grabInteractable.isSelected) return;
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }

    private void StartPour()
    {
        Debug.Log("Start");
        currentStream = CreateStream();
        currentStream.BeginStream();
        bottleAudio.StartPourSound();
    }

    private void EndPour()
    {
        Debug.Log("End");
        currentStream.EndStream();
        currentStream = null;
        bottleAudio.EndPourSound();
    }

    private float CalculatePourAngle() {
        return transform.up.y * Mathf.Rad2Deg;
    }

    private Stream CreateStream() {
        GameObject streamObject;
        if (shouldParentStream)
        {
            streamObject = Instantiate(streamPrefab, pourOrigin.position, Quaternion.identity, transform);
        }
        else
        {
            streamObject = Instantiate(streamPrefab, pourOrigin.position, Quaternion.identity);
        }

        return streamObject.GetComponent<Stream>();
    }
}
