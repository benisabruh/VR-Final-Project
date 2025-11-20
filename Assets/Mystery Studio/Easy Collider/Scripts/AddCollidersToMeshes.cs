using UnityEngine;
using UnityEditor;

namespace MysteryStudio.EasyCollider
{
    [ExecuteInEditMode]
    public class AddCollidersToMeshes : MonoBehaviour
    {
        public enum ColliderType
        {
            MeshCollider,
            BoxCollider,
            CapsuleCollider,
            SphereCollider,
            WheelCollider,
            TerrainCollider
        }

        public ColliderType colliderType = ColliderType.MeshCollider;

        [SerializeField]
        private bool applyToChildren = true;

        [SerializeField]
        private bool convex = false;

        [SerializeField]
        private bool showColliderPreview = false;

        [SerializeField]
        private string selectedTag = "Untagged";

        [SerializeField]
        private int selectedLayer = 0;

        [ContextMenu("Add Colliders")]
        public void AddColliders()
        {
            if (applyToChildren)
            {
                AddCollidersToObjectAndChildren(gameObject);
            }
            else
            {
                AddColliderToObject(gameObject);
            }

            Debug.Log("Colliders added successfully!");
        }

        [ContextMenu("Remove Colliders for This Object")]
        public void RemoveCollidersForThisObject()
        {
            int collidersRemoved = RemoveCollidersFromObject(gameObject);
            Debug.Log($"Removed {collidersRemoved} colliders from this object.");
        }

        [ContextMenu("Remove Colliders for All Objects")]
        public void RemoveCollidersForAllObjects()
        {
            int totalCollidersRemoved = RemoveCollidersFromObjectAndChildren(gameObject);
            Debug.Log($"Removed {totalCollidersRemoved} colliders from this object and its children.");
        }

        private int RemoveCollidersFromObject(GameObject obj)
        {
            Collider[] colliders = obj.GetComponents<Collider>();
            int count = colliders.Length;

            foreach (Collider collider in colliders)
            {
                DestroyImmediate(collider);
            }

            return count;
        }

        private int RemoveCollidersFromObjectAndChildren(GameObject parent)
        {
            int count = 0;

            // Remove colliders from the parent object
            count += RemoveCollidersFromObject(parent);

            // Remove colliders from all children
            foreach (Transform child in parent.transform)
            {
                count += RemoveCollidersFromObjectAndChildren(child.gameObject);
            }

            return count;
        }

        private void AddCollidersToObjectAndChildren(GameObject parent)
        {
            AddColliderToObject(parent);

            foreach (Transform child in parent.transform)
            {
                AddCollidersToObjectAndChildren(child.gameObject);
            }
        }

        private void AddColliderToObject(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            Mesh mesh = GetMesh(meshFilter, skinnedMeshRenderer);

            if (mesh == null)
            {
                return;
            }

            Collider[] existingColliders = obj.GetComponents<Collider>();
            foreach (Collider collider in existingColliders)
            {
                DestroyImmediate(collider);
            }

            switch (colliderType)
            {
                case ColliderType.MeshCollider:
                    MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = mesh;
                    meshCollider.convex = convex;
                    break;

                case ColliderType.BoxCollider:
                    BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
                    boxCollider.size = mesh.bounds.size;
                    boxCollider.center = mesh.bounds.center;
                    break;

                case ColliderType.CapsuleCollider:
                    CapsuleCollider capsuleCollider = obj.AddComponent<CapsuleCollider>();
                    Bounds bounds = mesh.bounds;
                    capsuleCollider.height = bounds.size.y;
                    capsuleCollider.radius = Mathf.Max(bounds.size.x, bounds.size.z) / 2;
                    capsuleCollider.center = bounds.center;
                    break;

                case ColliderType.SphereCollider:
                    SphereCollider sphereCollider = obj.AddComponent<SphereCollider>();
                    sphereCollider.radius = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) / 2;
                    sphereCollider.center = mesh.bounds.center;
                    break;

                case ColliderType.WheelCollider:
                    WheelCollider wheelCollider = obj.AddComponent<WheelCollider>();
                    wheelCollider.radius = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.z) / 2;
                    wheelCollider.suspensionDistance = mesh.bounds.size.y / 2;
                    break;

                case ColliderType.TerrainCollider:
                    Debug.LogWarning("TerrainCollider is not supported for regular meshes.");
                    break;
            }

            // Set the tag and layer
            obj.tag = selectedTag;
            obj.layer = selectedLayer;
        }

        private Mesh GetMesh(MeshFilter meshFilter, SkinnedMeshRenderer skinnedMeshRenderer)
        {
            if (meshFilter != null)
            {
                return meshFilter.sharedMesh;
            }
            else if (skinnedMeshRenderer != null)
            {
                Mesh mesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(mesh);
                return mesh;
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            if (!showColliderPreview || colliderType == ColliderType.TerrainCollider)
                return;

            Gizmos.color = Color.green;

            if (applyToChildren)
            {
                DrawPreviewForHierarchy(gameObject);
            }
            else
            {
                DrawPreviewForObject(gameObject);
            }
        }

        private void DrawPreviewForHierarchy(GameObject parent)
        {
            DrawPreviewForObject(parent);

            foreach (Transform child in parent.transform)
            {
                DrawPreviewForHierarchy(child.gameObject);
            }
        }

        private void DrawPreviewForObject(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            Mesh mesh = GetMesh(meshFilter, skinnedMeshRenderer);

            if (mesh == null)
            {
                return;
            }

            Bounds bounds = mesh.bounds;
            Matrix4x4 originalMatrix = Gizmos.matrix;
            Gizmos.matrix = obj.transform.localToWorldMatrix;

            switch (colliderType)
            {
                case ColliderType.BoxCollider:
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                    break;

                case ColliderType.SphereCollider:
                    float sphereRadius = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2;
                    Gizmos.DrawWireSphere(bounds.center, sphereRadius);
                    break;

                case ColliderType.CapsuleCollider:
                    DrawWireCapsule(bounds.center, bounds.size);
                    break;

                case ColliderType.MeshCollider:
                    Gizmos.DrawWireMesh(mesh);
                    break;
            }

            Gizmos.matrix = originalMatrix;
        }

        private void DrawWireCapsule(Vector3 center, Vector3 size)
        {
            float radius = Mathf.Max(size.x, size.z) / 2;
            float height = size.y;

            Vector3 top = center + Vector3.up * (height / 2 - radius);
            Vector3 bottom = center - Vector3.up * (height / 2 - radius);

            // Draw side lines
            Gizmos.DrawLine(top + Vector3.right * radius, bottom + Vector3.right * radius);
            Gizmos.DrawLine(top - Vector3.right * radius, bottom - Vector3.right * radius);
            Gizmos.DrawLine(top + Vector3.forward * radius, bottom + Vector3.forward * radius);
            Gizmos.DrawLine(top - Vector3.forward * radius, bottom - Vector3.forward * radius);

            // Draw top and bottom hemispheres
            DrawWireHemisphere(top, radius);
            DrawWireHemisphere(bottom, radius);
        }

        private void DrawWireHemisphere(Vector3 center, float radius)
        {
            int segments = 12;
            float angleIncrement = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * angleIncrement * Mathf.Deg2Rad;
                float nextAngle = (i + 1) * angleIncrement * Mathf.Deg2Rad;

                Vector3 start = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Vector3 end = center + new Vector3(Mathf.Cos(nextAngle), 0, Mathf.Sin(nextAngle)) * radius;

                Gizmos.DrawLine(start, end);
            }
        }
    }
}