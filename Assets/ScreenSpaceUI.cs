using System.Collections;
using TMPro;
using UnityEngine;

public class ScreenSpaceUI : MonoBehaviour
{
    public static ScreenSpaceUI instance;

    public TextMeshProUGUI text;

    public Color PositiveColor;
    public Color NegativeColor;

    private Color currentColor;

    public Gradient alphaChange;

    public float animationDuration;
    private float currentAnimationTime = 0;

    private void Start()
    {
        instance = this;
    }

    public void PositiveChange(int amount)
    {
        currentColor = PositiveColor;
        text.text = "+" + amount;
        StartCoroutine("Animate");
    }

    public void NegativeChange(int amount)
    {
        currentColor = NegativeColor;
        text.text = "-" + amount;
        StartCoroutine("Animate");
    }

    IEnumerator Animate()
    {
        currentAnimationTime = 0;
        while(currentAnimationTime < animationDuration)
        {
            currentAnimationTime = Mathf.Min(currentAnimationTime + Time.fixedDeltaTime,animationDuration);
            float currentPer = currentAnimationTime / animationDuration;

            float alpha = alphaChange.Evaluate(currentPer).a;

            currentColor.a = alpha;
            text.color = currentColor;

            yield return new WaitForFixedUpdate();
        }
    }

}
