// using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations;

public class StreamTrigger : MonoBehaviour
{
    [SerializeField] private float fillSpeed = 0.2f;
    public DrinkEffect streamEffect;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Glass")
        {
            GlassFiller glassFiller = other.GetComponent<GlassFiller>();
            if (glassFiller != null)
            {
                glassFiller.Fill(fillSpeed * Time.deltaTime, streamEffect);
            }
        }
        else
        {
            GlassController glassController = other.GetComponent<GlassController>();
            LineRenderer streamLineRenderer = GetComponentInParent<LineRenderer>();
            if (glassController != null && streamLineRenderer != null)
            {
                glassController.Fill(fillSpeed * Time.deltaTime, streamEffect, streamLineRenderer.startColor);
            }
        }

        PipAlcoholic pipMouth = other.GetComponent<PipAlcoholic>();
        if (pipMouth != null)
        {
            pipMouth.Trigger(streamEffect);
        }
    }
}
