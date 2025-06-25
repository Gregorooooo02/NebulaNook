using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FlashScript : MonoBehaviour
{
    public Light Light;
    public float Duration;
    public float MaxLightIntensity;
    public float MaxBloomIntensity;

    public AnimationCurve intesityCurve;
    public Volume Volume;

    private float current_time = 0.0f;
    private Bloom profile;

    private VolumeProfile clonedProfile;
    private Bloom cloneBloom;

    private void Start()
    {
        Volume = FindAnyObjectByType<Volume>();
        Volume.profile.TryGet(out profile);

        clonedProfile = Instantiate(Volume.profile);
        cloneBloom = Instantiate(profile);
    }

    void FixedUpdate()
    {
        if (current_time >= Duration)
        {
            clonedProfile.components[0] = cloneBloom;
            Volume.profile = clonedProfile;
            Destroy(gameObject);
        }
        current_time += Time.fixedDeltaTime;

        float t = current_time / Duration;
        float value = intesityCurve.Evaluate(t);

        Light.intensity = value * MaxLightIntensity;

        profile.intensity.value = value * MaxBloomIntensity;
    }
}
