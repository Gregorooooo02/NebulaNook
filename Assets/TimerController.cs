using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float[] thresholds;
    public Color[] ThresholdsColors;

    public Gradient colorThresholds;

    public float timerDuration;

    private float timerScale;
    private float currentValue = 1.0f;

    public Slider slider;
    public Image image;

    private int currentThresholdIndex = 0;

    private void Start()
    {
        timerScale = 1.0f / timerDuration;
    }

    private void FixedUpdate()
    {
        if(currentValue == 0.0f) return;
        currentValue -= timerScale * Time.fixedDeltaTime;
        currentValue = Mathf.Clamp01(currentValue);

        slider.value = currentValue;

        image.color = colorThresholds.Evaluate(currentValue);

        /*if(currentThresholdIndex >= thresholds.Length) return;

        if (thresholds[currentThresholdIndex] >= currentValue)
        {
            image.color = ThresholdsColors[currentThresholdIndex];
            currentThresholdIndex++;
        }*/
    }
}
