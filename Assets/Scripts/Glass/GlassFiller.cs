using UnityEngine;

public class GlassFiller : MonoBehaviour
{
    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] private float fillSpeed = 0.2f;

    private float currentFillAmount = 0f;

    public void Fill(float delta) {
        currentFillAmount = Mathf.Clamp(currentFillAmount + delta * fillSpeed, 0f, 1f);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
    }
}
