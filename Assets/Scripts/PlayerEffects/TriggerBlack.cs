using UnityEngine;

public class TriggerBlack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var component = other.gameObject.GetComponent<Blackhole_expand>();
        if (component != null)
        {
            gameObject.transform.position = GameoverRoom.Instance.transform.position;
        }
    }
}
