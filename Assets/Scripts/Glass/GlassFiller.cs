using System;
using System.Collections.Generic;
using UnityEngine;

public class GlassFiller : MonoBehaviour
{
    [SerializeField] private Renderer liquidRenderer;
    [SerializeField] private float fillSpeed = 0.2f;
    [SerializeField, Range(0, 1f)] private float startingFill = 0f;

    [HideInInspector]
    public float currentFillAmount = 0f;

    [SerializeField] private float[] fillAmounts = new float[6];
    [SerializeField] private float DrinkMovementAmount = 2.0f;
    [SerializeField] private float minimumDrinkAmount = 0.05f;
    [SerializeField] private float minimumDrinkDistance = 0.05f;

    [HideInInspector]
    public bool wasServed = false;

    private void Awake()
    {
        currentFillAmount = Mathf.Clamp01(startingFill);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
    }

    public void Fill(float delta, DrinkEffect pouredDrink) {
        currentFillAmount = Mathf.Clamp(currentFillAmount + delta * fillSpeed, 0f, 1f);
        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);

        if (currentFillAmount >= 1.0f) return;
        fillAmounts[(int)pouredDrink] += delta * fillSpeed;

        Color currentColor = new Color(0, 0, 0);
        for (int i = 0; i < DrinkEffectMap.Instance.colorTable.Length; i++)
        {
            currentColor += (DrinkEffectMap.Instance.colorTable[i] * (fillAmounts[i] / currentFillAmount));
        }
        liquidRenderer.sharedMaterial.SetColor("_TopColor", currentColor);
        liquidRenderer.sharedMaterial.SetColor("_SideColor", currentColor * 0.8f);
    }

    public DrinkEffect GetFinalDrinkEffect()
    {
        //Calculate drink point
        Vector3 resultPoint = Vector3.zero;
        resultPoint.y += fillAmounts[0] * DrinkMovementAmount;
        resultPoint.x += fillAmounts[1] * DrinkMovementAmount;
        resultPoint.x -= fillAmounts[2] * DrinkMovementAmount;
        resultPoint.z -= fillAmounts[3] * DrinkMovementAmount;
        resultPoint.z += fillAmounts[4] * DrinkMovementAmount;
        resultPoint.y -= fillAmounts[5] * DrinkMovementAmount;

        if(currentFillAmount < minimumDrinkAmount) return DrinkEffect.EMPTY; // Check if enough drink was poured
        if(resultPoint.magnitude < minimumDrinkDistance) return DrinkEffect.WATER; // Check if result is water

        resultPoint += DrinkEffectMap.MapObject.transform.position;

        //Check for effect
        Collider[] colliders = DrinkEffectMap.MapObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.Contains(resultPoint) && collider.gameObject.TryGetComponent<DrinkEffectMapValues>(out DrinkEffectMapValues component))
            {
                return component.Effect;
            }
        }

        return DrinkEffect.MATTER; // If everything fails keel over :D
    }

    public float Drain(float amount) {
        float prev = currentFillAmount;
        currentFillAmount = Mathf.Clamp01(currentFillAmount - amount);

        const float eps = 0.01f;
        if (currentFillAmount < eps) {
            currentFillAmount = 0f;
        }

        float diffMul = 1.0f - ((prev - currentFillAmount) / prev);
        for(int i = 0;i < fillAmounts.Length; i++)
        {
            fillAmounts[i] *= diffMul;
        }

        liquidRenderer.sharedMaterial.SetFloat("_Fill", currentFillAmount);
        return prev - currentFillAmount;
    }

    public float GetFillAmount() {
        return currentFillAmount;
    }
}

public static class ColliderExtension
{
    public static bool Contains(this Collider collider, Vector3 point)
    {
        Vector3 direction = collider.bounds.center - point;
        Ray ray = new Ray(point, direction);

        bool contains = !collider.Raycast(ray,out var hit,direction.magnitude);

        return contains;
    }
}
