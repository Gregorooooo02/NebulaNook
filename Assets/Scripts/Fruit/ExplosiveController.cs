using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ExplosiveController : XRGrabInteractable
{
    [Header("Explosive Settings")]
    [SerializeField] private float explosionThreshold = 5f;
    [SerializeField] private float maxVelocity = 8f;
    [SerializeField] private float minHeartbeatRate = 0.5f;
    [SerializeField] private float maxHeartbeatRate = 0.1f;
    
    [Header("Heartbeat Animation")]
    [SerializeField] private float pulseMagnitude = 0.3f;
    [SerializeField] private AnimationCurve pulseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [Header("Explosion")]
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private AudioClip explosionSound;
    
    [Header("Visual Feedback")]
    [SerializeField, ColorUsage(true, true)] private Color normalColor = Color.white;
    [SerializeField, ColorUsage(true, true)] private Color dangerColor = Color.red;
    [SerializeField] private float colorTransitionSpeed = 2f;
    
    private bool isGrabbed = false;
    private bool hasExploded = false;
    private Vector3 originalScale;
    private Vector3 lastPosition;
    private float currentVelocity = 0f;
    private float currentHeartbeatRate;
    private Coroutine heartbeatCoroutine;
    private Rigidbody rb;
    private Renderer objectRenderer;
    private AudioSource audioSource;
    private FruitController fruitController;

    protected override void Awake()
    {
        base.Awake();
        interactionManager = FindAnyObjectByType<XRInteractionManager>();
        rb = GetComponent<Rigidbody>();
        objectRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        fruitController = GetComponent<FruitController>();
        
        if (fruitController != null)
        {
            fruitController.enabled = false;
        }
        
        originalScale = transform.localScale;
        lastPosition = transform.position;
        currentHeartbeatRate = minHeartbeatRate;
        
        if (objectRenderer != null)
        {
            objectRenderer.material.color = normalColor;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        if (hasExploded) return;
        
        isGrabbed = true;
        lastPosition = transform.position;
        
        StartCoroutine(VelocityMonitoring());
        StartHeartbeat();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        
        if (hasExploded) return;
        
        isGrabbed = false;
        StopHeartbeat();
        StopAllCoroutines();
        
        BecomeNormalFruit();
    }

    private IEnumerator VelocityMonitoring()
    {
        while (isGrabbed && !hasExploded)
        {
            Vector3 currentPosition = transform.position;
            float distance = Vector3.Distance(currentPosition, lastPosition);
            currentVelocity = distance / Time.fixedDeltaTime;
            
            lastPosition = currentPosition;
            
            if (currentVelocity > explosionThreshold)
            {
                Explode();
                yield break;
            }
            
            UpdateHeartbeatRate();
            UpdateVisualFeedback();
            
            yield return new WaitForFixedUpdate();
        }
    }

    private void UpdateHeartbeatRate()
    {
        float velocityRatio = Mathf.Clamp01(currentVelocity / maxVelocity);
        currentHeartbeatRate = Mathf.Lerp(minHeartbeatRate, maxHeartbeatRate, velocityRatio);
    }

    private void UpdateVisualFeedback()
    {
        if (objectRenderer == null) return;
        
        float dangerRatio = Mathf.Clamp01(currentVelocity / explosionThreshold);
        Color targetColor = Color.Lerp(normalColor, dangerColor, dangerRatio);
        
        objectRenderer.material.color = Color.Lerp(
            objectRenderer.material.color, 
            targetColor, 
            Time.deltaTime * colorTransitionSpeed
        );
    }

    private void StartHeartbeat()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
        }
        heartbeatCoroutine = StartCoroutine(HeartbeatCoroutine());
    }

    private void StopHeartbeat()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
        transform.localScale = originalScale;
    }

    private IEnumerator HeartbeatCoroutine()
    {
        while (isGrabbed && !hasExploded)
        {
            float halfDuration = currentHeartbeatRate * 0.5f;
            yield return StartCoroutine(ScaleTo(originalScale * (1f + pulseMagnitude), halfDuration));
            yield return StartCoroutine(ScaleTo(originalScale, halfDuration));
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        
        while (elapsed < duration && !hasExploded)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = pulseCurve.Evaluate(t);
            
            transform.localScale = Vector3.Lerp(startScale, targetScale, curveValue);
            yield return null;
        }
        
        if (!hasExploded)
        {
            transform.localScale = targetScale;
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        
        hasExploded = true;
        StopAllCoroutines();
        
        if (isSelected && firstInteractorSelecting != null)
        {
            interactionManager.SelectExit(firstInteractorSelecting, this);
        }
        
        if (explosionParticlePrefab != null)
        {
            GameObject explosion = Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 5f);
        }
        
        if (explosionSound != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
        
        // AddExplosionForce();
        Destroy(gameObject);
    }

    private void AddExplosionForce()
    {
Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider col in colliders)
        {
            Rigidbody colRb = col.GetComponent<Rigidbody>();
            if (colRb != null && colRb != rb)
            {
                colRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    private void BecomeNormalFruit()
    {
        transform.localScale = originalScale;
        
        if (objectRenderer != null)
        {
            objectRenderer.material.color = normalColor;
        }
        
        if (fruitController != null)
        {
            fruitController.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        
        #if UNITY_EDITOR
        string info = $"Velocity: {currentVelocity:F2}\nThreshold: {explosionThreshold}\nHeartbeat: {currentHeartbeatRate:F2}s";
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, info);
        #endif
    }

    // Context Menu dla testów
    [ContextMenu("Test Explosion")]
    private void TestExplosion()
    {
        if (Application.isPlaying)
        {
            Explode();
        }
    }

    [ContextMenu("Force High Velocity")]
    private void TestHighVelocity()
    {
        if (Application.isPlaying)
        {
            currentVelocity = explosionThreshold + 1f;
        }
    }
}
