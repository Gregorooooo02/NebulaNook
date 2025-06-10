using UnityEngine;

public class GizmoDraw : MonoBehaviour
{
    [SerializeField] private bool drawGizmo = true;

    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoSize = 0.5f;

    void OnDrawGizmos()
    {
        // Draw a simple wireframe cube at the object's position
        if (drawGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * gizmoSize);
        }
    }
}
