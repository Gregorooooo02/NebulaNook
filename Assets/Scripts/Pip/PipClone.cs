using UnityEngine;

public class PipClone : MonoBehaviour
{
    public ParticleSystem system;
    public GameObject Model;
    public void TriggerParticles()
    {
        system.Play();
    }
}
