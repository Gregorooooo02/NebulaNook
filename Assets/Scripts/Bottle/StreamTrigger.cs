using UnityEngine;

public class StreamTrigger : MonoBehaviour
{
    [SerializeField] private float fillSpeed = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        GlassFiller glassFiller = other.GetComponent<GlassFiller>();
        if (glassFiller != null)
        {
            glassFiller.Fill(fillSpeed * Time.deltaTime);
        }
    }
}
