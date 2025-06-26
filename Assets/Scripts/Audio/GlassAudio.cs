using System.Collections;
using UnityEngine;

public class GlassAudio : MonoBehaviour
{
    public AudioSource GlassAudioSource;
    public AudioClip[] PickUpClips;
    public AudioClip[] DropClips;
    public AudioClip[] PourClips;

    [Header("Collision Settings")]
    [SerializeField] private float minImpactForce = 0.5f;
    [SerializeField] private float collisionCooldown = 1f;

    private bool hasPlayedFirstDropSound = false;
    private float lastCollisionTime = 0f;
    private Rigidbody rb;

    private bool isCurrentlyPouring = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
            GlassAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }

    public void PlayPickUpSound()
    {
        if (PickUpClips.Length > 0)
        {
            int randomIndex = Random.Range(0, PickUpClips.Length);
            GlassAudioSource.PlayOneShot(PickUpClips[randomIndex]);
        }
    }

    public void PlayDropSound()
    {
        if (DropClips.Length > 0)
        {
            int randomIndex = Random.Range(0, DropClips.Length);
            GlassAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }

    public void StartPourSound()
    {
        if (isCurrentlyPouring) return;
        isCurrentlyPouring = true;
        GlassAudioSource.Stop();

        if (PourClips.Length > 0)
        {
            GlassAudioSource.PlayOneShot(PourClips[0]);
            StartCoroutine(PlayPourLoop());
        }
    }

    public void EndPourSound()
    {
        if (!isCurrentlyPouring) return;
        isCurrentlyPouring = false;
        StopAllCoroutines();
        GlassAudioSource.Stop();

        if (PourClips.Length > 2)
        {
            GlassAudioSource.PlayOneShot(PourClips[2]);
        }
    }

    private IEnumerator PlayPourLoop()
    {
        if (PourClips.Length > 0)
        {
            yield return new WaitForSeconds(PourClips[0].length);
        }

        if (isCurrentlyPouring && PourClips.Length > 1)
        {
            GlassAudioSource.clip = PourClips[1];
            GlassAudioSource.loop = true;
            GlassAudioSource.Play();
        }
    }
}
