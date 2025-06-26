using UnityEngine;

public class FollowHead : MonoBehaviour
{
    public Transform head;
    public float distanceFromHead = 1.0f;

    private void Update()
    {
        gameObject.transform.position = head.position + new Vector3(head.forward.x, head.forward.y, head.forward.z).normalized * distanceFromHead;
        gameObject.transform.rotation = Quaternion.LookRotation(head.forward, head.up);
    }
}
