using UnityEngine;

public class Explode : ClientState
{
    public Rigidbody MainBone;
    public float explosionForce;
    public float explosionRadius;
    public float TimeToDisapear;
    private float _currentTime = 0;

    private bool triggered = false;

    public GameObject ExplosionEffect;

    public override ClientState RunState()
    {
        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                return this;
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            Instantiate(ExplosionEffect, transform);
            MainBone.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            triggered = true;
        }
        return this;
    }
}
