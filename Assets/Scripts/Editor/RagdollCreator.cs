using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    public static class RagdollCreator
    {
        [MenuItem("Tools/Generate Ragdoll")]
        static void GenerateRagdoll()
        {
            var root = Selection.activeTransform;
            if (!root) { Debug.LogError("No root transform selected!"); return; }
            CreateRagdollRecursive(root, null, 0);
            Debug.Log("Ragdoll created successfully!");
        }

        static void CreateRagdollRecursive(Transform root, Rigidbody parentRb, int depth)
        {
            // bool shouldAddCollider = depth % 2 == 0;

            Rigidbody rb;
            if (root.gameObject.GetComponent<Rigidbody>() == null)
            {
                rb = root.gameObject.AddComponent<Rigidbody>();
            }
            else
            {
                rb = root.gameObject.GetComponent<Rigidbody>();
            }

            rb.mass = 1f;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            if (/*shouldAddCollider &&*/ root.childCount > 0)
            {
                var child = root.GetChild(0);
                float length = Vector3.Distance(root.position, child.position);
                float radius = length * 0.05f;

                CapsuleCollider col;
                if (root.gameObject.GetComponent<CapsuleCollider>() == null)
                {
                    col = root.gameObject.AddComponent<CapsuleCollider>();
                }
                else
                {
                    col = root.gameObject.GetComponent<CapsuleCollider>();
                }

                col.radius = radius;
                col.height = Mathf.Max(length, radius) / 30f;
                col.direction = 1; // Y-axis

                Vector3 midWorld = Vector3.Lerp(root.position, root.GetChild(0).position, 0.5f);
                col.center = root.InverseTransformPoint(midWorld);
            }

            if (parentRb != null)
            {
                CharacterJoint joint;
                if (root.gameObject.GetComponent<CharacterJoint>() == null)
                {
                    joint = root.gameObject.AddComponent<CharacterJoint>();
                }
                else
                {
                    joint = root.gameObject.GetComponent<CharacterJoint>();
                }
                joint.connectedBody = parentRb;

                joint.swingAxis = Vector3.forward;
                joint.axis = Vector3.up;
                joint.autoConfigureConnectedAnchor = true;

                joint.projectionDistance = 0.1f;
                joint.projectionAngle = 180f;

                joint.enableProjection = true;
                joint.enablePreprocessing = false;
            }

            foreach (Transform child in root)
            {
                CreateRagdollRecursive(child, rb, depth + 1);
            }
        }
    }
}
