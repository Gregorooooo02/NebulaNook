using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShakerAudio : MonoBehaviour
{
    [Header("Part Type")]
    [SerializeField] private ShakerPartType shakerPartType = ShakerPartType.BOTTOM;

    [Header("Audio Sources")]
    public AudioSource ShakerAudioSource;
    public AudioSource ShakeAudioSource;

    [Header("Basic Audio")]
    public AudioClip[] PickUpClips;
    public AudioClip[] DropClips;

    [Header("Bottom Part Audio")]
    public AudioClip[] PourClips;
    public AudioClip ShakeClip;
    public AudioClip ShakeEndClip;

    [Header("Top Part Audio")]
    public AudioClip AttachClip;
    public AudioClip DetachClip;

    [Header("Collision Settings")]
    [SerializeField] private float minImpactForce = 0.5f;
    [SerializeField] private float collisionCooldown = 1f;

    [Header("Shake Settings (Bottom Part Only)")]
    [SerializeField] private float minShakeVolume = 0.1f;
    [SerializeField] private float maxShakeVolume = 1.0f;
    [SerializeField] private float minShakePitch = 0.8f;
    [SerializeField] private float maxShakePitch = 1.5f;
    [SerializeField] private float shakeVolumeSmoothing = 5f;
    [SerializeField] private float shakeIntensityThreshold = 0.5f;
    [SerializeField] private float minLiquidForShake = 0.1f;

    private bool hasPlayedFirstDropSound = false;
    private float lastCollisionTime = 0f;
    private Rigidbody rb;
    private bool isCurrentlyPouring = false;
    private bool isCurrentlyShaking = false;
    private float currentShakeIntensity = 0f;

    private ShakeDetection shakeDetection;
    private TopPartController topPartController;
    private XRGrabInteractable grabInteractable;
    private ShakerAudio bottomAudio;
    private GlassFiller glassFiller;

    public enum ShakerPartType
    {
        BOTTOM,
        TOP
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (shakerPartType == ShakerPartType.BOTTOM)
        {
            InitializeBottomPart();
        }
        else
        {
            InitializeTopPart();
        }
    }

    private void InitializeBottomPart()
    {
        // Bottom part - komponenty są tutaj
        shakeDetection = GetComponent<ShakeDetection>();
        glassFiller = GetComponent<GlassFiller>();

        // Top part jest rodzeństwem - znajdź przez parent (shaker)
        Transform shakerParent = transform.parent; // shaker
        if (shakerParent != null)
        {
            topPartController = shakerParent.GetComponentInChildren<TopPartController>();
        }

        // Ustaw domyślne wartości dla shake audio
        if (ShakeAudioSource != null && ShakeClip != null)
        {
            ShakeAudioSource.clip = ShakeClip;
            ShakeAudioSource.loop = true;
            ShakeAudioSource.volume = 0f;
        }

        Debug.Log(topPartController);
        Debug.Log(shakeDetection);
        Debug.Log(glassFiller);
    }

    private void InitializeTopPart()
    {
        // Top part - znajdź bottom part przez parent (shaker)
        Transform shakerParent = transform.parent; // shaker
        if (shakerParent != null)
        {
            Transform bottomTransform = shakerParent.Find("bottom");
            if (bottomTransform != null)
            {
                // Znajdź komponenty w bottom part
                shakeDetection = bottomTransform.GetComponent<ShakeDetection>();
                bottomAudio = bottomTransform.GetComponent<ShakerAudio>();
                glassFiller = bottomTransform.GetComponent<GlassFiller>();
            }
        }
        
        // TopPartController jest w tym obiekcie (top)
        topPartController = GetComponent<TopPartController>();
        
    }

    private void Update()
    {
        if (shakerPartType == ShakerPartType.BOTTOM)
        {
            UpdateShakeAudio();
        }
    }

    #region Bottom Part - Shake Audio
    private void UpdateShakeAudio()
    {
        if (shakeDetection == null || topPartController == null || glassFiller == null) 
        {
            Debug.LogWarning("Missing components for shake audio");
            return;
        }

        bool hasLiquid = glassFiller.GetFillAmount() >= minLiquidForShake;
        bool shouldShake = topPartController.isAttached && grabInteractable.isSelected && hasLiquid;

        if (shouldShake)
        {
            Vector3 linearVel = rb.linearVelocity;
            Vector3 angularVel = rb.angularVelocity;
            
            var linearIntensityVec = shakeDetection.calculateLinearVelocity(Time.deltaTime);
            var angularIntensityVec = shakeDetection.calculateAngularVelocity(Time.deltaTime);
            
            float linearIntensity = linearIntensityVec.magnitude;
            float angularIntensity = angularIntensityVec.magnitude;

            float totalIntensity = (linearIntensity + angularIntensity) / 2f;
            
            currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, totalIntensity, Time.deltaTime * shakeVolumeSmoothing);
            if (currentShakeIntensity > shakeIntensityThreshold)
            {
                if (!isCurrentlyShaking)
                {
                    StartShakeSound();
                }
                UpdateShakeIntensity(currentShakeIntensity);
            }
            else
            {
                if (isCurrentlyShaking)
                {
                    StopShakeSound();
                }
            }
        }
        else
        {
            if (isCurrentlyShaking)
            {
                StopShakeSound();
            }
        }
    }

    private void StartShakeSound()
    {
        if (ShakeAudioSource != null && ShakeClip != null)
        {
            isCurrentlyShaking = true;
            ShakeAudioSource.Play();
        }
    }

    private void UpdateShakeIntensity(float intensity)
    {
        if (ShakeAudioSource != null)
        {
            float normalizedIntensity = Mathf.Clamp01(intensity / 10f);
            
            float targetVolume = Mathf.Lerp(minShakeVolume, maxShakeVolume, normalizedIntensity);
            float targetPitch = Mathf.Lerp(minShakePitch, maxShakePitch, normalizedIntensity);
            
            ShakeAudioSource.volume = targetVolume;
            ShakeAudioSource.pitch = targetPitch;
        }
    }

    private void StopShakeSound()
    {
        if (ShakeAudioSource != null)
        {
            isCurrentlyShaking = false;
            ShakeAudioSource.Stop();
            ShakeAudioSource.volume = 0f;
            ShakeAudioSource.pitch = 1f;
        }
    }

    public void PlayShakeCompleteSound()
    {
        if (shakerPartType == ShakerPartType.BOTTOM && ShakeEndClip != null)
        {
            StopShakeSound();
            ShakerAudioSource.PlayOneShot(ShakeEndClip);
        }
    }
    #endregion

    #region Bottom Part - Pour Audio
    public void StartPourSound()
    {
        if (shakerPartType != ShakerPartType.BOTTOM) return;
        if (isCurrentlyPouring) return;
        
        if (glassFiller == null || glassFiller.GetFillAmount() <= 0f)
        {
            Debug.Log("No liquid to pour");
            return;
        }
        
        isCurrentlyPouring = true;
        ShakerAudioSource.Stop();

        if (PourClips.Length > 0)
        {
            ShakerAudioSource.PlayOneShot(PourClips[0]); // PourBegin
            StartCoroutine(StartPourLoop());
        }
        
        Debug.Log("Started pour sound");
    }

    public void EndPourSound()
    {
        if (shakerPartType != ShakerPartType.BOTTOM) return;
        if (!isCurrentlyPouring) return;
        
        isCurrentlyPouring = false;
        StopAllCoroutines();
        ShakerAudioSource.Stop();

        if (PourClips.Length > 2)
        {
            ShakerAudioSource.PlayOneShot(PourClips[2]); // PourEnd
        }
        
        Debug.Log("Ended pour sound");
    }

    private IEnumerator StartPourLoop()
    {
        if (PourClips.Length > 0)
        {
            yield return new WaitForSeconds(PourClips[0].length);
        }

        if (isCurrentlyPouring && PourClips.Length > 1)
        {
            ShakerAudioSource.clip = PourClips[1]; // PourLoop
            ShakerAudioSource.loop = true;
            ShakerAudioSource.Play();
        }
    }

    public void ResetPourState()
    {
        isCurrentlyPouring = false;
        StopAllCoroutines();
        ShakerAudioSource.Stop();
        ShakerAudioSource.loop = false;
    }
    #endregion

    #region Top Part - Attach/Detach Audio
    public void PlayAttachSound()
    {
        if (shakerPartType == ShakerPartType.TOP && AttachClip != null)
        {
            ShakerAudioSource.PlayOneShot(AttachClip);
            
            // Powiadom bottom part o attach
            if (bottomAudio != null)
            {
                bottomAudio.OnTopPartAttached();
            }
        }
    }

    public void PlayDetachSound()
    {
        if (shakerPartType == ShakerPartType.TOP && DetachClip != null)
        {
            ShakerAudioSource.PlayOneShot(DetachClip);
            
            // Powiadom bottom part o detach
            if (bottomAudio != null)
            {
                bottomAudio.OnTopPartDetached();
            }
        }
    }

    // Metody callback dla bottom part
    public void OnTopPartAttached()
    {
        Debug.Log("Bottom part: Top attached");
    }

    public void OnTopPartDetached()
    {
        Debug.Log("Bottom part: Top detached");
        
        // Zatrzymaj shake jeśli był aktywny
        if (isCurrentlyShaking)
        {
            StopShakeSound();
        }
    }
    #endregion

    #region Common Audio (Both Parts)
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasPlayedFirstDropSound)
        {
            PlayFirstDropSound();
            hasPlayedFirstDropSound = true;
            return;
        }

        if (Time.time - lastCollisionTime >= collisionCooldown)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            if (impactForce >= minImpactForce)
            {
                PlayDropSound();
                lastCollisionTime = Time.time;
            }
        }
    }

    private void PlayFirstDropSound()
    {
        if (DropClips.Length > 0)
        {
            int randomIndex = Random.Range(0, DropClips.Length);
            ShakerAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }

    public void PlayPickUpSound()
    {
        if (PickUpClips.Length > 0)
        {
            int randomIndex = Random.Range(0, PickUpClips.Length);
            ShakerAudioSource.PlayOneShot(PickUpClips[randomIndex]);
        }
    }

    public void PlayDropSound()
    {
        if (DropClips.Length > 0)
        {
            int randomIndex = Random.Range(0, DropClips.Length);
            ShakerAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }
    #endregion
}
