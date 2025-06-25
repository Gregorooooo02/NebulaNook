using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JukeBoxController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] jazzTracks;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Randomize the entire array of jazz tracks
        for (int i = 0; i < jazzTracks.Length; i++)
        {
            int randomIndex = Random.Range(i, jazzTracks.Length);
            AudioClip temp = jazzTracks[i];
            jazzTracks[i] = jazzTracks[randomIndex];
            jazzTracks[randomIndex] = temp;
        }
    }

    private void Start()
    {
        // Start playing the first track
        if (jazzTracks.Length > 0)
        {
            audioSource.clip = jazzTracks[0];
            audioSource.Play();
        }
    }

    private void Update()
    {
        // Check if the audio source has finished playing the current clip
        if (!audioSource.isPlaying)
        {
            // Find the index of the currently playing clip
            int currentIndex = System.Array.IndexOf(jazzTracks, audioSource.clip);
            // If there is a next track, play it
            if (currentIndex + 1 < jazzTracks.Length)
            {
                audioSource.clip = jazzTracks[currentIndex + 1];
                audioSource.Play();
            }
        } 
        // If the audio source is not playing and there are no more tracks, loop back to the first track
        else if (audioSource.clip == jazzTracks[jazzTracks.Length - 1] && !audioSource.isPlaying)
        {
            audioSource.clip = jazzTracks[0];
            audioSource.Play();
        }
    }
}
