using System.Collections;
using UnityEngine;

public class NalewakAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource NalewakAudioSource;
    public AudioClip[] PourClips;

    private bool isCurrentlyPouring = false;

    public void StartPourSound()
    {
        if (isCurrentlyPouring) return;
        isCurrentlyPouring = true;
        NalewakAudioSource.Stop();

        if (PourClips.Length > 0)
        {
            NalewakAudioSource.PlayOneShot(PourClips[0]); // Pour begin
            StartCoroutine(PlayPourLoop());
        }
    }

    public void EndPourSound()
    {
        if (!isCurrentlyPouring) return;
        isCurrentlyPouring = false;
        StopAllCoroutines();
        NalewakAudioSource.Stop();

        if (PourClips.Length > 2)
        {
            NalewakAudioSource.PlayOneShot(PourClips[2]); // Pour end
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
            NalewakAudioSource.clip = PourClips[1]; // Pour loop
            NalewakAudioSource.loop = true;
            NalewakAudioSource.Play();
        }
    }
}
