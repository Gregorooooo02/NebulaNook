using System.Collections;
using UnityEngine;

public class BottleAudio : MonoBehaviour
{
    public AudioSource BottleAudioSource;
    public AudioClip[] PickUpClips;
    public AudioClip[] DropClips;
    public AudioClip[] PourClips;

    [Header("Collision Settings")]
    [SerializeField] private float minImpactForce = 2f;
    [SerializeField] private float collisionCooldown = 1f;

    private bool hasPlayedFristDropSound = false;
    private float lastCollisionTime = 0f;
    private Rigidbody rb;

    private bool isCurrentlyPouring = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OllisionEnter(Collision collision)
    {
        if (!hasPlayedFristDropSound)
        {
            PlayFirstDropSound();
            hasPlayedFristDropSound = true;
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
            BottleAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }

    public void PlayPickUpSound()
    {
        if (PickUpClips.Length > 0)
        {
            int randomIndex = Random.Range(0, PickUpClips.Length);
            BottleAudioSource.PlayOneShot(PickUpClips[randomIndex]);
        }
    }

    public void PlayDropSound()
    {
        if (DropClips.Length > 0)
        {
            int randomIndex = Random.Range(0, DropClips.Length);
            BottleAudioSource.PlayOneShot(DropClips[randomIndex]);
        }
    }

    public void StartPourSound()
    {
        if (isCurrentlyPouring) return;
        isCurrentlyPouring = true;
        BottleAudioSource.Stop();

        if (PourClips.Length > 0)
        {
            BottleAudioSource.PlayOneShot(PourClips[0]);
            StartCoroutine(PlayPourLoop());
        }
    }

    public void EndPourSound()
    {
        if (!isCurrentlyPouring) return;
        isCurrentlyPouring = false;
        StopAllCoroutines();
        BottleAudioSource.Stop();

        if (PourClips.Length > 2)
        {
            BottleAudioSource.PlayOneShot(PourClips[2]);
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
            BottleAudioSource.clip = PourClips[1];
            BottleAudioSource.loop = true;
            BottleAudioSource.Play();
        }
    }
}
