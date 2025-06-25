using UnityEngine;

public class GlassPourController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GlassFiller glassFiller;
    [SerializeField] private GlassController glassController;
    [SerializeField] private GameObject streamPrefab;

    [Header("Geometry")]
    [Tooltip("Radius of the glass rim, where the stream will be spawned")]
    [SerializeField] private float rimRadius = 0.05f;
    [Tooltip("Height of the glass rim, where the stream will be spawned")]
    [SerializeField] private float rimHeight = 0.15f;

    [Header("Pour Settings")]
    [Tooltip("Minimum degrees, which the glass needs to be tilted to start pouring")]
    [SerializeField] private float minPourAngle = 15f;
    [Tooltip("Maximum degrees, which the glass can be tilted to pour")]
    [SerializeField] private float maxPourAngle = 90f;
    [Tooltip("The amount of liquid to pour per second, when fillAmount is near 0")]
    [SerializeField] private float minFlowRate = 0.1f;
    [Tooltip("The amount of liquid to pour per second, when fillAmount is near 1")]
    [SerializeField] private float maxFlowRate = 0.5f;

    private Stream currentStream = null;
    [HideInInspector] public bool IsPouring = false;
    private Transform streamTransform = null;

    void Update()
    {
        float fillAmount = glassFiller ? glassFiller.GetFillAmount() : glassController.currentFillAmount;
        float dynamicThreshold = Mathf.Lerp(maxPourAngle, minPourAngle, fillAmount);

        float angleFromUp = Vector3.Angle(transform.up, Vector3.up);
        bool shouldPour = angleFromUp > dynamicThreshold && fillAmount > 0;

        if (!shouldPour) {
            if (currentStream != null) StopStream();
            return;
        }

        float t = Mathf.InverseLerp(dynamicThreshold, maxPourAngle, angleFromUp);
        float flowRate = maxFlowRate * t * fillAmount;
        flowRate = Mathf.Max(flowRate, minFlowRate * fillAmount);

        if (glassFiller)
        {
            glassFiller.Drain(flowRate * Time.deltaTime);

            if (glassFiller.GetFillAmount() <= 0)
            {
                StopStream();
                return;
            }
        }
        else
        {
            glassController.Drain(flowRate * Time.deltaTime);

            if (glassController.currentFillAmount <= 0)
            {
                glassController.drinkEffect = DrinkEffect.EMPTY;
                StopStream();
                return;
            }
        }


        if (currentStream == null) StartStream();
        UpdateStreamTransform();
    }

    public void StartStream() {
        GameObject streamObject = Instantiate(streamPrefab, transform);
        if (glassFiller)
        {
            streamObject.GetComponentInChildren<StreamTrigger>().streamEffect = glassFiller.GetFinalDrinkEffect();    
        }
        currentStream = streamObject.GetComponent<Stream>();
        streamTransform = streamObject.transform;
        currentStream.lineRenderer.startColor = glassFiller ? glassFiller.GetLiquidColor() : glassController.GetLiquidColor();
        currentStream.lineRenderer.endColor = glassFiller ? glassFiller.GetLiquidColor() : glassController.GetLiquidColor();
        currentStream.BeginStream();
        IsPouring = true;
    }

    public void StopStream() {
        currentStream.EndStream();
        Destroy(currentStream.gameObject);
        currentStream = null;
        streamTransform = null;
        IsPouring = false;
    }

    private void UpdateStreamTransform() {
        if (currentStream == null) return;

        Vector3 worldDown = Vector3.down;
        Vector3 rimDir = Vector3.ProjectOnPlane(worldDown, transform.up).normalized;

        Vector3 worldRimPos = transform.position + transform.up * rimHeight + rimDir * rimRadius;

        streamTransform.position = worldRimPos;
        streamTransform.rotation = Quaternion.LookRotation(rimDir, transform.up);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 rimCenter = origin + transform.up * rimHeight;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, rimCenter);

        const int segs = 64;
        float angleStep = 360f / segs;
        Vector3 prevPoint = rimCenter + (transform.right * rimRadius);

        for (int i = 1; i <= segs; i++) {
            float angle = angleStep * i;
            Vector3 dir =
                transform.right * Mathf.Cos(Mathf.Deg2Rad * angle) +
                transform.forward * Mathf.Sin(Mathf.Deg2Rad * angle);
            Vector3 nextPoint = rimCenter + dir.normalized * rimRadius;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rimCenter, rimRadius * 0.05f);
    }
#endif
}
