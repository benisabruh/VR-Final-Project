using UnityEditor;
using UnityEngine;

namespace MysteryStudio.EasyCollider
{
    [CustomEditor(typeof(AddCollidersToMeshes))]
    public class AddCollidersToMeshesEditor : Editor
    {
        private SerializedProperty colliderTypeProp;
        private SerializedProperty applyToChildrenProp;
        private SerializedProperty convexProp;
        private SerializedProperty showColliderPreviewProp;
        private SerializedProperty selectedTagProp;
        private SerializedProperty selectedLayerProp;

        private void OnEnable()
        {
            colliderTypeProp = serializedObject.FindProperty("colliderType");
            applyToChildrenProp = serializedObject.FindProperty("applyToChildren");
            convexProp = serializedObject.FindProperty("convex");
            showColliderPreviewProp = serializedObject.FindProperty("showColliderPreview");
            selectedTagProp = serializedObject.FindProperty("selectedTag");
            selectedLayerProp = serializedObject.FindProperty("selectedLayer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the collider type dropdown
            EditorGUILayout.PropertyField(colliderTypeProp, new GUIContent("Collider Type"));

            // Draw the "Apply to Children" toggle
            EditorGUILayout.PropertyField(applyToChildrenProp, new GUIContent("Apply to Children"));

            // Conditionally show the convex option for MeshCollider
            if (colliderTypeProp.enumValueIndex == (int)AddCollidersToMeshes.ColliderType.MeshCollider)
            {
                EditorGUILayout.PropertyField(convexProp, new GUIContent("Convex"));
            }

            // Draw the "Show Collider Preview" toggle
            EditorGUILayout.PropertyField(showColliderPreviewProp, new GUIContent("Show Collider Preview"));

            // Draw the tag dropdown
            selectedTagProp.stringValue = EditorGUILayout.TagField("Tag", selectedTagProp.stringValue);

            // Draw the layer dropdown
            selectedLayerProp.intValue = EditorGUILayout.LayerField("Layer", selectedLayerProp.intValue);

            // Apply changes to the serialized object
            serializedObject.ApplyModifiedProperties();

            // Add the buttons
            if (GUILayout.Button("Add Colliders"))
            {
                ((AddCollidersToMeshes)target).AddColliders();
            }

            if (GUILayout.Button("Remove Colliders for This Object"))
            {
                ((AddCollidersToMeshes)target).RemoveCollidersForThisObject();
            }

            // Calculate the number of colliders to remove for child objects
            int totalColliders = 0;
            GameObject targetObject = ((AddCollidersToMeshes)target).gameObject;

            // Get all child objects
            Transform[] childTransforms = targetObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in childTransforms)
            {
                // Skip the parent object itself
                if (child != targetObject.transform)
                {
                    totalColliders += child.GetComponents<Collider>().Length;
                }
            }

            if (GUILayout.Button($"Remove Colliders for Child Objects ({totalColliders} colliders)"))
            {
                ((AddCollidersToMeshes)target).RemoveCollidersForAllObjects();
            }
        }
    }
}