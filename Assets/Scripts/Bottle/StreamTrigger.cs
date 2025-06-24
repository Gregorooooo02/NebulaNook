using UnityEngine;

public class StreamTrigger : MonoBehaviour
{
    [SerializeField] private float fillSpeed = 0.2f;
    [SerializeField] private DrinkEffect streamEffect;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Glass")
        {
            GlassController glassController = other.GetComponent<GlassController>();
            LineRenderer streamLineRenderer = GetComponentInParent<LineRenderer>();
            print("sdasd");
            if (glassController != null && streamLineRenderer != null)
            {
                glassController.Fill(fillSpeed * Time.deltaTime, streamEffect, streamLineRenderer.endColor);
            }
        }
        else
        {
            GlassFiller glassFiller = other.GetComponent<GlassFiller>();
            if (glassFiller != null)
            {
                glassFiller.Fill(fillSpeed * Time.deltaTime, streamEffect);
            }
        }
    }
}
