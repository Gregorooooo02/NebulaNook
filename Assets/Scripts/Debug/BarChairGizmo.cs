using UnityEngine;

[ExecuteInEditMode]
public class BarChairGizmo : MonoBehaviour
{
    public enum GizmoType
    {
        Chair,
        Exit
    }
    public Color GizmoColor = Color.white;
    public GizmoType type = GizmoType.Chair;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        switch (type)
        {
            case GizmoType.Chair:
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
                break;
            case GizmoType.Exit:
                Gizmos.DrawSphere(Vector3.zero, 1.0f);
                break;
        } 
    }
}
