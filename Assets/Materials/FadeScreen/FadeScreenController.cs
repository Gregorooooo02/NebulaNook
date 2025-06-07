using System.Collections;
using UnityEngine;

public class FadeScreenController : MonoBehaviour
{
    public static FadeScreenController Instance { get; private set; }
    public bool IsFadingComplete;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Material fadeMaterial;

    private float FadeTime
    {
        set
        {
            fadeMaterial.SetFloat("_FadeTime", value);
        }
    }

    private void Awake()
    {
        Instance = this;
        FadeIn();
    }

    public void FadeIn() => StartCoroutine(Interpolate(1.0f, 0.0f));
    public void FadeOut() => StartCoroutine(Interpolate(0.0f, 1.0f));

    private IEnumerator Interpolate(float from, float to)
    {
        IsFadingComplete = false;
        float current = from;

        for (float t = 0; current != to; t += Time.deltaTime * speed)
        {
            current = Mathf.Clamp01(Mathf.SmoothStep(from, to, t));
            FadeTime = current;
            yield return null;
        }
        IsFadingComplete = true;
    }
}
