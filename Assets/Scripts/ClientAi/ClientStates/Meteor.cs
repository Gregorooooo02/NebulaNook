using System.Collections;
using UnityEngine;

public class Meteor : ClientState
{
    public GameObject MeteorPrefab;
    public float meteorForce;
    public float explosionForce;
    public float explosionRadius;
    [Min(0.0f)]
    public float explosionDistance;

    private Transform SpawnPoint;

    private bool triggered = false;

    public GameObject Parent;
    public float initialDelay;

    public Rigidbody Head;
    public GameObject ExplosionPrefab;

    public float finalDelay;

    private void Start()
    {
        SpawnPoint = Controller.Spawner.MeteorSpawnPoint;
    }

    public override ClientState RunState()
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
        GameObject meteor = Instantiate(MeteorPrefab,SpawnPoint.position,Quaternion.identity);
        Rigidbody rb = meteor.GetComponent<Rigidbody>();
        Controller.mainCollider.enabled = false;
        while (meteor != null && rb != null)
        {
            rb.AddForce((transform.position - SpawnPoint.transform.position).normalized * meteorForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        Controller.ToggleRagdoll(true);
        Vector3 explosionPosition = transform.position - ((transform.position - SpawnPoint.transform.position).normalized * explosionDistance);
        GameObject exposionObject = Instantiate(ExplosionPrefab, explosionPosition, Quaternion.identity);
        Head.AddExplosionForce(explosionForce,explosionPosition, explosionRadius);

        yield return new WaitForSeconds(finalDelay);
        Destroy(exposionObject);
        Destroy(Parent);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Meteor")
        {
            Destroy(other.gameObject);
        }
    }
}
