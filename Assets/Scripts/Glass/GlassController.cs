using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlassController : MonoBehaviour
{

    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] public float fillSpeed = 0.5f;
    [SerializeField, Range(0, 1f)] private float startingFill = 0f;

    [HideInInspector]
    public float currentFillAmount = 0f;
    public DrinkEffect drinkEffect;


    private void Awake()
    {
        currentFillAmount = Mathf.Clamp01(startingFill);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
    }

    public void Fill(float delta, DrinkEffect pouredDrink, Color color)
    {
        print("forntite");
        currentFillAmount = Mathf.Clamp(currentFillAmount + delta * fillSpeed, 0f, 1f);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);

        if (currentFillAmount >= 1.0f) return;
        drinkEffect = pouredDrink;

        liquidRenderer.sharedMaterial.SetColor("_TopColor", color);
        liquidRenderer.sharedMaterial.SetColor("_SideColor", color * 0.8f);
    }

    public float Drain(float amount)
    {
        float prev = currentFillAmount;
        currentFillAmount = Mathf.Clamp01(currentFillAmount - amount);

        const float eps = 0.01f;
        if (currentFillAmount < eps)
        {
            currentFillAmount = 0f;
        }

        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
        return prev - currentFillAmount;
    }
}