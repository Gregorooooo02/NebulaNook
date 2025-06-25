using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlassController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] public float fillSpeed = 0.5f;
    [SerializeField, Range(0, 1f)] private float startingFill = 0f;
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private float explosionForce = 500;

    [HideInInspector]
    public float currentFillAmount = 0f;
    public DrinkEffect drinkEffect;

    [HideInInspector]
    public bool wasServed = false;


    private void Awake()
    {
        currentFillAmount = Mathf.Clamp01(startingFill);
        liquidRenderer.sharedMaterial = new Material(liquidRenderer.sharedMaterial);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
        rb = GetComponent<Rigidbody>();
    }

    public void Fill(float delta, DrinkEffect pouredDrink, Color color)
    {
        if (drinkEffect != DrinkEffect.EMPTY && pouredDrink != drinkEffect)
        {
            var explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity, null);

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            
            foreach (Collider col in colliders)
            {
                Rigidbody colRb = col.GetComponent<Rigidbody>();
                if (colRb != null && colRb != rb)
                {
                    colRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
            
            Destroy(gameObject);
        }

        currentFillAmount = Mathf.Clamp(currentFillAmount + delta * fillSpeed, 0f, 1f);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);

        if (currentFillAmount >= 1.0f) return;
        drinkEffect = pouredDrink;

        liquidRenderer.sharedMaterial.SetColor("_TopColor", color);
        liquidRenderer.sharedMaterial.SetColor("_SideColor", color * 0.8f);
    }

    public Color GetLiquidColor()
    {
        return liquidRenderer.sharedMaterial.GetColor("_TopColor");
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