using UnityEngine;

public class Blackhole : ClientState
{
    public GameObject BlackHole;
    public AudioSource BlackHoleAudioSource;
    private bool triggered = false; 
    public override ClientState RunState()
    {
        if (!triggered)
        {
            Instantiate(BlackHole, transform);
            BlackHoleAudioSource.Play();
            triggered = true;
        }
        return this;
    }
}
