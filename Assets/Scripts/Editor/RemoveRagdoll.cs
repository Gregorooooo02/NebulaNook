using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    public static class RemoveRagdoll
    {
        [MenuItem("Tools/Remove Ragdoll")]
        static void DestroyRagdoll()
        {
            var root = Selection.activeTransform;
            if (!root) { Debug.LogError("No root transform selected!"); return; }
            DestroyRagdollRecursive(root, 0);
            Debug.Log("Ragdoll removed successfully!");
        }

        static void DestroyRagdollRecursive(Transform root, int depth)
        {
            if(root.gameObject.TryGetComponent<CharacterJoint>(out CharacterJoint joint))
            {
                GameObject.DestroyImmediate(joint);
            }

            if (root.gameObject.TryGetComponent<Collider>(out Collider coll))
            {
                GameObject.DestroyImmediate(coll);
            }

            if (root.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                GameObject.DestroyImmediate(rb);
            }

            foreach (Transform child in root)
            {
                DestroyRagdollRecursive(child, depth + 1);
            }
        }
    }
}
