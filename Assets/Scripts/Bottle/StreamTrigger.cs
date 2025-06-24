using UnityEngine;

public class StreamTrigger : MonoBehaviour
{
    [SerializeField] private float fillSpeed = 0.2f;
    public DrinkEffect streamEffect;

    private void OnTriggerStay(Collider other)
    {
        GlassFiller glassFiller = other.GetComponent<GlassFiller>();
        if (glassFiller != null)
        {
            glassFiller.Fill(fillSpeed * Time.deltaTime, streamEffect);
        }

        PipAlcoholic pipMouth = other.GetComponent<PipAlcoholic>();
        if(pipMouth != null)
        {
            pipMouth.Trigger(streamEffect);
        }
    }
}
