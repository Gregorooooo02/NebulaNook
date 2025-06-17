using UnityEngine;
using UnityEngine.AI;

public class Rave : ClientState
{
    public float waitTime;
    private bool waitDone = false;

    public NavMeshAgent Agent;
    public float MinPointDist = 0.75f;

    private bool _isWalking = false;
    
    public float colorChangeSpeed;
    public float glowIncreaseTime;
    public float glowIncreaseSpeed;

    private float glow = 2.0f;

    private float _currentTime;
    private float _timer;
    private float currentHue = 0;

    public Renderer[] renderers;

    private Color _color;

    private bool _first = true;

    public override ClientState RunState()
    {
        if (_first)
        {
            _color = Color.HSVToRGB(0, 1, 1);
            foreach (var renderer in renderers)
            {
                renderer.material = new Material(renderer.material);
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", _color * glow);
                renderer.material.SetTexture("_EmissionMap", null);
            }
            _first = false;
        }


        ChangeColor();
        if (!waitDone)
        {
            _currentTime += Time.fixedDeltaTime;
            if (_currentTime >= waitTime)
            {
                waitDone = true;
            }
            return this;
        }
        if (_isWalking)
        {
            if (Vector3.Distance(transform.position, Controller.Spawner.Exit.transform.position) <= MinPointDist)
            {
                _isWalking = false;
                Controller.Spawner?.NotifyClientFinished();
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

    private void ChangeColor()
    {
        if(_timer < glowIncreaseTime)
        {
            _timer += Time.fixedDeltaTime;
            glow += glowIncreaseSpeed * Time.fixedDeltaTime;
        }

        currentHue += colorChangeSpeed * Time.fixedDeltaTime;
        if(currentHue > 1.0f)currentHue = 1.0f - currentHue;

        _color = Color.HSVToRGB(currentHue, 1, 1);

        foreach (var renderer in renderers)
        {
            renderer.material.SetColor("_EmissionColor", _color * glow);
        }
    }

}
