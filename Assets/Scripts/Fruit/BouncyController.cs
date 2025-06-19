using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BouncyController : XRGrabInteractable
{
    [Header("Bouncy Settings")]
    [SerializeField] private string jumpPointTag = "JumpPoint";
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float slipOutDelay = 0.5f;
    [SerializeField] private float slipOutForce = 2f;

    private bool shouldSlipOut = false;

    [Header("Animation")]
    [SerializeField] private float squashAmount = 0.3f;
    [SerializeField] private float squashDuration = 0.2f;

    [Header("Editor Testing")]
    [SerializeField] private bool enableEditorTesting = false;
    [SerializeField] private bool testJumpingInEditor = false;

    [Header("Collision Settings")]
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string playerTag = "Player";

    private Collider fruitCollider;
    private bool collisionsDisabled = false;

    private bool isJumping = false;
    private bool hasBeenGrabbedOnce = false;
    private bool isNormalFruit = false;
    private Vector3 originalScale;
    private Rigidbody rb;
    private FruitController fruitController;
    private List<Transform> availableJumpPoints = new List<Transform>();
    private int lastJumpPointIndex = -1;

    protected override void Awake()
    {
        base.Awake();
        interactionManager = FindAnyObjectByType<XRInteractionManager>();
        rb = GetComponent<Rigidbody>();
        fruitController = GetComponent<FruitController>();
        fruitCollider = GetComponent<Collider>();
        fruitController.enabled = false;
        originalScale = transform.localScale;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearDamping = 1f;
        }

        FindJumpPoints();
    }

    private void FindJumpPoints()
    {
        GameObject[] jumpPoints = GameObject.FindGameObjectsWithTag(jumpPointTag);

        availableJumpPoints.Clear();

        foreach (GameObject obj in jumpPoints)
        {
            availableJumpPoints.Add(obj.transform);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (isNormalFruit) return;
        if (!hasBeenGrabbedOnce)
        {
            hasBeenGrabbedOnce = true;
            shouldSlipOut = true;
            StopJumping();
            StartCoroutine(SlipOutCoroutine());
        }
        else
        {
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.WakeUp();
            }

            StopJumping();
            StopAllCoroutines();
            transform.localScale = originalScale;
            shouldSlipOut = false;
            BecomeNormalFruit();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (isNormalFruit)
        {
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.WakeUp();
            }
            return;
        }

        shouldSlipOut = false;

        if (hasBeenGrabbedOnce && !isJumping)
        {
            StartCoroutine(DelayedStartJumping());
        }
    }

    private IEnumerator SlipOutCoroutine()
    {
        yield return new WaitForSeconds(slipOutDelay);

        if (shouldSlipOut && isSelected)
        {
            if (firstInteractorSelecting != null)
            {
                interactionManager.SelectExit(firstInteractorSelecting, this);
            }

            if (rb != null)
            {
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * slipOutForce, ForceMode.Impulse);
            }
        }

        shouldSlipOut = false;
    }

    private IEnumerator DelayedStartJumping()
    {
        yield return new WaitForSeconds(0.2f);
        if (!isSelected && !isNormalFruit)
        {
            StartJumping();
        }
    }

    private void BecomeNormalFruit()
    {
        isNormalFruit = true;
        isJumping = false;
        StopAllCoroutines();

        EnablePlayerCollisions();

        if (fruitController != null)
        {
            fruitController.enabled = true;
        }

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0.5f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (rb.IsSleeping())
            {
                rb.WakeUp();    
            }
        }
    }

    private void StartJumping()
    {
        if (availableJumpPoints.Count == 0 || isJumping || isNormalFruit) return;
        isJumping = true;

        DisablePlayerCollisions();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.linearDamping = 0f;
        }

        StartCoroutine(JumpCoroutine());
    }

    private void StopJumping()
    {
        isJumping = false;
        StopAllCoroutines();

        EnablePlayerCollisions();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearDamping = 0.5f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.WakeUp();
        }
    }

    private void DisablePlayerCollisions()
    {
        if (collisionsDisabled) return;
        CharacterController playerController = FindObjectsByType<CharacterController>(FindObjectsSortMode.None)[0];

        if (playerController.CompareTag(playerTag))
        {
            Physics.IgnoreCollision(fruitCollider, playerController, true);
        }

        collisionsDisabled = true;
    }

    private void EnablePlayerCollisions()
    {
        if (!collisionsDisabled) return;
        CharacterController playerController = FindObjectsByType<CharacterController>(FindObjectsSortMode.None)[0];

        if (playerController.CompareTag(playerTag))
        {
            Physics.IgnoreCollision(fruitCollider, playerController, false);
        }

        collisionsDisabled = false;
    }

    private IEnumerator JumpCoroutine()
    {
        while (isJumping && !isNormalFruit)
        {
            Transform targetPoint = GetRandomJumpPoint();
            if (targetPoint == null)
            {
                yield return new WaitForSeconds(jumpCooldown);
                continue;
            }

            yield return StartCoroutine(PerformJump(targetPoint.position));
            yield return new WaitForSeconds(jumpCooldown);
        }
    }

    private Transform GetRandomJumpPoint()
    {
        if (availableJumpPoints.Count == 0) return null;
        if (availableJumpPoints.Count == 1) return availableJumpPoints[0];

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, availableJumpPoints.Count);
        }
        while (randomIndex == lastJumpPointIndex && availableJumpPoints.Count > 1);

        lastJumpPointIndex = randomIndex;
        return availableJumpPoints[randomIndex];
    }

    private IEnumerator PerformJump(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float jumpTime = distance / jumpSpeed;

        yield return StartCoroutine(SquashAndStretch(true));

        float elapsed = 0f;

        if (rb != null)
        {
            rb.Sleep();
        }

        while (elapsed < jumpTime && isJumping)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpTime;

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
            float heightOffset = Mathf.Sin(t * Mathf.PI) * maxJumpHeight;
            currentPosition.y += heightOffset;

            transform.position = currentPosition;
            yield return null;
        }

        if (isJumping)
        {
            transform.position = targetPosition;

            if (rb != null)
            {
                rb.WakeUp();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            yield return StartCoroutine(SquashAndStretch(false));
        }
    }

    private IEnumerator SquashAndStretch(bool isSquashing)
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale;

        if (isSquashing)
        {
            targetScale = new Vector3(
                originalScale.x * (1f + squashAmount * 0.5f),
                originalScale.y * (1f - squashAmount),
                originalScale.z * (1f + squashAmount * 0.5f)
            );
        }
        else
        {
            targetScale = originalScale;
        }

        float elapsed = 0f;

        while (elapsed < squashDuration && !isNormalFruit)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / squashDuration;

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        if (!isNormalFruit)
        {
            transform.localScale = targetScale;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!enableEditorTesting || Application.isPlaying) return;
        if (testJumpingInEditor && !isJumping)
        {
            StartEditorTest();
        }
        else if (!testJumpingInEditor && isJumping)
        {
            StopEditorTest();
        }
    }

    private void StartEditorTest()
    {
        if (availableJumpPoints.Count == 0)
        {
            FindJumpPoints();
        }

        hasBeenGrabbedOnce = true;
        isJumping = true;

        if (rb != null)
        {
            rb.mass = 3f;
        }

        StartCoroutine(JumpCoroutine());
    }

    private void StopEditorTest()
    {
        isJumping = false;
        StopAllCoroutines();

        if (rb != null)
        {
            rb.mass = 1f;
        }

        transform.localScale = originalScale;
    }
#endif

    [ContextMenu("Test Jump Sequence")]
    private void TestJumpSequence()
    {
        if (!Application.isPlaying) return;
        if (!isJumping)
        {
            hasBeenGrabbedOnce = true;
            StartJumping();
        }
        else
        {
            StopJumping();
        }
    }

    [ContextMenu("Become Normal Fruit")]
    private void TestBecomeNormal()
    {
        if (!Application.isPlaying) return;
        BecomeNormalFruit();
    }

    [ContextMenu("Reset State")]
    private void ResetState()
    {
        StopJumping();
        hasBeenGrabbedOnce = false;
        isNormalFruit = false;
        transform.localScale = originalScale;

        if (rb != null)
        {
            rb.mass = 1f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (availableJumpPoints == null) return;
        
        Gizmos.color = Color.green;
        for (int i = 0; i < availableJumpPoints.Count; i++)
        {
            if (availableJumpPoints[i] != null)
            {
                Gizmos.DrawWireSphere(availableJumpPoints[i].position, 0.3f);
                
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(
                    availableJumpPoints[i].position + Vector3.up * 0.5f, 
                    $"Point {i}"
                );
                #endif
            }
        }
        
        if (isJumping && availableJumpPoints.Count > 0)
        {
            Transform nextPoint = GetNextJumpPointForPreview();
            if (nextPoint != null)
            {
                DrawJumpArc(transform.position, nextPoint.position);
            }
        }
        
        #if UNITY_EDITOR
        string status = isNormalFruit ? "Normal Fruit" : 
                    isJumping ? "Jumping" : 
                    hasBeenGrabbedOnce ? "Ready to Jump" : "Fresh";
        
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 1f, 
            status
        );
        #endif
    }
    private Transform GetNextJumpPointForPreview()
    {
        if (availableJumpPoints.Count == 0) return null;
        if (availableJumpPoints.Count == 1) return availableJumpPoints[0];
        
        int nextIndex = (lastJumpPointIndex + 1) % availableJumpPoints.Count;
        return availableJumpPoints[nextIndex];
    }

    private void DrawJumpArc(Vector3 start, Vector3 end)
    {
        Gizmos.color = Color.yellow;
        
        int segments = 20;
        Vector3 lastPoint = start;
        
        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector3 point = Vector3.Lerp(start, end, t);
            
            float heightOffset = Mathf.Sin(t * Mathf.PI) * maxJumpHeight;
            point.y += heightOffset;
            
            Gizmos.DrawLine(lastPoint, point);
            lastPoint = point;
        }
    }
}
