using System.Collections;
using UnityEngine;

public class RavePIP : PipState
{
    public float colorChangeSpeed;
    public float glowIncreaseTime;
    public float glowIncreaseSpeed;

    private float glow = 2.0f;

    private float _timer;
    private float currentHue = 0;

    public Renderer[] renderers;

    private Color _color;

    private bool triggered = false;

    public float duration;

    public override PipState RunState()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine("ExecuteEffect");
        }
        return this;
    }


    IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(initialDelay);
        _color = Color.HSVToRGB(0, 1, 1);
        foreach (var renderer in renderers)
        {
            renderer.material = new Material(renderer.material);
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", _color * glow);
        }
        while(_timer < duration)
        {
            if(_timer < glowIncreaseTime)
            {
                glow += glowIncreaseSpeed * Time.fixedDeltaTime;
            }

            currentHue += colorChangeSpeed * Time.fixedDeltaTime;
            if (currentHue > 1.0f) currentHue = 1.0f - currentHue;

            _color = Color.HSVToRGB(currentHue, 1, 1);

            foreach (var renderer in renderers)
            {
                renderer.material.SetColor("_EmissionColor", _color * glow);
            }

            _timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(controller.transform.parent.gameObject);
        PipSpawner.Instance?.SpawnPip();
    }
}
