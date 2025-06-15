using System.Collections;
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
/*        if (triggered)
        {
            if (_currentTime < TimeToDisapear)
            {
                _currentTime += Time.fixedDeltaTime;
                return this;
            }
            Controller.Spawner?.NotifyClientFinished();
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            GetComponentInParent<ClientController>().ToggleRagdoll(true);
            Instantiate(ExplosionEffect, transform);
            MainBone.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            triggered = true;
        }*/

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
        Controller.ToggleRagdoll(true);
        MainBone.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        yield return new WaitForSeconds(TimeToDisapear);
        Controller.Spawner?.NotifyClientFinished();
        Destroy(gameObject.transform.parent.gameObject);
    }
}
