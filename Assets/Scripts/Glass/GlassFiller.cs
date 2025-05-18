using System;
using UnityEngine;

public class GlassFiller : MonoBehaviour
{
    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] private float fillSpeed = 0.2f;
    [SerializeField, Range(0, 1f)] private float startingFill = 0f;

    private float currentFillAmount = 0f;

    private void Awake()
    {
        currentFillAmount = Mathf.Clamp01(startingFill);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
    }

    public void Fill(float delta) {
        currentFillAmount = Mathf.Clamp(currentFillAmount + delta * fillSpeed, 0f, 1f);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
    }

    public float Drain(float amount) {
        float prev = currentFillAmount;
        currentFillAmount = Mathf.Clamp01(currentFillAmount - amount);

        const float eps = 0.01f;
        if (currentFillAmount < eps) {
            currentFillAmount = 0f;
        }

        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
        return prev - currentFillAmount;
    }

    public float GetFillAmount() {
        return currentFillAmount;
    }
}
