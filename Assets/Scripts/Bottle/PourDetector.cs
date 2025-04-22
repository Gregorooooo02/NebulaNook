using UnityEngine;

public class PourDetector : MonoBehaviour
{
    [SerializeField] public int pourThreshold = 45;

    [SerializeField] private Transform pourOrigin;
    [SerializeField] private GameObject streamPrefab;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update() {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck) {
            isPouring = pourCheck;
            if (isPouring) {
                StartPour();
            } else {
                EndPour();
            }
        }
    }

    private void StartPour() {
        Debug.Log("Start");
        currentStream = CreateStream();
        currentStream.BeginStream();
    }

    private void EndPour() {
        Debug.Log("End");
        currentStream.EndStream();
        currentStream = null;
    }

    private float CalculatePourAngle() {
        return transform.up.y * Mathf.Rad2Deg;
    }

    private Stream CreateStream() {
        GameObject streamObject = Instantiate(streamPrefab, pourOrigin.position, Quaternion.identity, transform);

        return streamObject.GetComponent<Stream>();
    }
}
