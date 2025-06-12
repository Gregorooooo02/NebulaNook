using System.Collections;
using UnityEngine;

public class TransparencyPIP : PipState
{
    public float transitionTime;
    private float _currentTime;

    private Color[] _colors;
    public Material[] replecementMaterials;
    public Renderer[] renderers;

    public float targetAlphaPercent;
    private float[] TargetAlphas;
    private float[] StartAlphas;

    private bool triggered = false;

    public GameObject particles;

    public float duration;

    private void Start()
    {
        _colors = new Color[replecementMaterials.Length];
        StartAlphas = new float[replecementMaterials.Length];
        TargetAlphas = new float[replecementMaterials.Length];
        for (int i = 0; i < replecementMaterials.Length; i++)
        {
            _colors[i] = replecementMaterials[i].GetColor("_BaseColor");
            StartAlphas[i] = _colors[i].a;
            TargetAlphas[i] = _colors[i].a * targetAlphaPercent;
        }
    }

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
        for (int i = 0; i < replecementMaterials.Length; i++)
        {
            renderers[i].material = new Material(replecementMaterials[i]);
        }
        particles.SetActive(true);
        while(_currentTime < transitionTime)
        {
            ChangeAlphas();
            _currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(duration);
        Destroy(controller.gameObject);
    }

    private void ChangeAlphas()
    {
        float currentTimePer = _currentTime / transitionTime;
        for (int i = 0; i < replecementMaterials.Length; i++)
        {
            _colors[i].a = Mathf.Lerp(StartAlphas[i], TargetAlphas[i], currentTimePer);
            renderers[i].material.SetColor("_BaseColor", _colors[i]);
        }
    }
}
