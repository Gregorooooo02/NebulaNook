using UnityEngine;
using UnityEngine.AI;

public class Transparency : ClientState
{
    public float transitionTime;
    private float _currentTime;

    private Color[] _colors;
    public Material[] transparentMaterials;
    public Renderer[] Meshes;

    public float targetAlphaPercent;
    private float[] TargetAlphas;
    private float[] StartAlphas;

    public GameObject Parent;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    private bool effectDone = false;
    private bool first = true;

    private void Start()
    {
        _colors = new Color[transparentMaterials.Length];
        StartAlphas = new float[transparentMaterials.Length];
        TargetAlphas = new float[transparentMaterials.Length];
        for (int i = 0; i < transparentMaterials.Length; i++)
        {
            _colors[i] = transparentMaterials[i].GetColor("_BaseColor");
            StartAlphas[i] = _colors[i].a;
            TargetAlphas[i] = _colors[i].a * targetAlphaPercent;
        }
    }

    public override ClientState RunState()
    {
        if (!effectDone)
        {
            ChangeAlphas();
            _currentTime += Time.fixedDeltaTime;
            if (_currentTime >= transitionTime)
            {
                effectDone = true;
            }
            return this;
        } 
        if (_isWalking)
        {
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            Agent.SetDestination(Controller.Spawner.Exit.transform.position);
            _isWalking = true;
        }
        return this;
    }


    private void ChangeAlphas()
    {
        float currentTimePer = _currentTime / transitionTime;
        for(int i = 0;i < transparentMaterials.Length; i++)
        {
            if (first)
            {
                Meshes[i].material = new Material(transparentMaterials[i]);
            }
            _colors[i].a = Mathf.Lerp(StartAlphas[i], TargetAlphas[i], currentTimePer);
            Meshes[i].material.SetColor("_BaseColor", _colors[i]);
        }
        first = false;
    }

}
