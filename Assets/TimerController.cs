using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public ClientController controller;

    public Gradient colorThresholds;

    public float timerDuration;

    private float timerScale;
    private float currentValue = 1.0f;

    public Slider slider;
    public Image image;

    private int currentThresholdIndex = 0;

    private bool endTriggered = false;

    private void Start()
    {
        timerScale = 1.0f / timerDuration;
    }

    private void FixedUpdate()
    {
        if(currentValue == 0.0f && endTriggered) return;
        else if(currentValue == 0.0f && !endTriggered)
        {
            controller.Begone();
            endTriggered = true;
        }
        currentValue -= timerScale * Time.fixedDeltaTime;
        currentValue = Mathf.Clamp01(currentValue);

        slider.value = currentValue;

        image.color = colorThresholds.Evaluate(currentValue);
    }


}
