using UnityEngine;

public class TriggerBlack : MonoBehaviour
{
    public GameObject ToActivate;

    private void OnTriggerEnter(Collider other)
    {
        var component = other.gameObject.GetComponent<Blackhole_expand>();
        if (component != null)
        {
            ToActivate?.SetActive(true);
        }
    }

    public void Activate()
    {
        ToActivate?.SetActive(true);
    }
}
