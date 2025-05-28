using UnityEngine;

public class Blackhole_expand : MonoBehaviour
{
    public float ExpansionSpeed;

    public float ExpansionTime;
    private float _currentTime;

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        Vector3 newScale = gameObject.transform.localScale;
        newScale.x += ExpansionSpeed * delta;
        newScale.y += ExpansionSpeed * delta;
        newScale.z += ExpansionSpeed * delta;
        gameObject.transform.localScale = newScale;

        _currentTime += delta;
        if(_currentTime > ExpansionTime)
        {
            Destroy(gameObject);
        }
    }
}
