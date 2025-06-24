using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShakeDetection : MonoBehaviour
{
    private Rigidbody rigidbody;
    private XRGrabInteractable grabInteractable;
    [SerializeField] private float linearShakeThreshold;
    [SerializeField] private float angularShakeThreshold;
    [SerializeField] private float shakeDurationToResult;
    private float shakeDuration;
    private bool isGrabbed;
    private bool result = false;
    private DrinkEffect finalEffect = DrinkEffect.EMPTY;

    private Quaternion lastRotation; private Vector3 lastPosition;
    [SerializeField] private TopPartController top;
    [SerializeField] private GlassFiller glassFiller;
    private float[] lastShakerState = new float[6];
    private float lastShakerFill = 0.0f;

    void Awake()
    {
        glassFiller.fillSpeed = 0.0f;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener((_) => isGrabbed = true);
        grabInteractable.selectExited.AddListener((_) => isGrabbed = false);

        lastRotation = transform.rotation;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (isPouring())
        {
            lastShakerState = glassFiller.fillAmounts;
            lastShakerFill = glassFiller.GetFillAmount();
            shakeDuration = 0.0f;
            result = false;
            finalEffect = DrinkEffect.MATTER;
        }

        else if (isDraining())
        {
            lastShakerFill = glassFiller.GetFillAmount();
            lastShakerState = glassFiller.fillAmounts;

            if (lastShakerFill == 0.0f)
            {
                shakeDuration = 0.0f;
                result = false;
                finalEffect = DrinkEffect.EMPTY;
            }
        }


        if (isGrabbed && top.isAttached && !result)
        {
            float deltaTime = Time.deltaTime;
            if (deltaTime == 0)
            {
                return;
            }

            Vector3 linearVelocity = calculateLinearVelocity(deltaTime);
            Vector3 angularVelocity = calculateAngularVelocity(deltaTime);

            bool isLinearShaking = linearVelocity.magnitude > linearShakeThreshold;
            bool isAngularShaking = angularVelocity.magnitude > angularShakeThreshold;

            if (isLinearShaking || isAngularShaking)
            {
                shakeDuration += deltaTime;
                if (shakeDuration > shakeDurationToResult)
                {
                    result = true;
                    finalEffect = glassFiller.GetFinalDrinkEffect();
                }
            }

            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }
    }

    private Vector3 calculateLinearVelocity(float deltaTime)
    {
        return (transform.position - lastPosition) / deltaTime;
    }

    private Vector3 calculateAngularVelocity(float deltaTime)
    {
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);
        deltaRot.ToAngleAxis(out float angleDegrees, out Vector3 axis);
        if (angleDegrees > 180f) angleDegrees -= 360f;
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        Vector3 angularVelocity = axis * (angleRad / deltaTime);
        return angularVelocity;
    }

    private bool isPouring()
    {
        float current = glassFiller.GetFillAmount();
        return current > lastShakerFill;
    }

    private bool isDraining()
    {
        float current = glassFiller.GetFillAmount();
        return current < lastShakerFill;
    }
}
