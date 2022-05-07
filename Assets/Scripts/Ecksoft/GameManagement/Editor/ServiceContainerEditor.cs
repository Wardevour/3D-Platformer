namespace Ecksoft.GameManagement {

    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ServiceContainer))]
    public class ServiceContainerEditor : Editor {

        ServiceContainer realTarget;
        SerializedObject realTargetSO;
        SerializedProperty keysSP;
        SerializedProperty valuesSP;

        string new_key = "New Service";

        void OnEnable() {
            realTarget = (ServiceContainer)target;
            realTargetSO = new SerializedObject(realTarget);

            keysSP = realTargetSO.FindProperty("_keys");
            valuesSP = realTargetSO.FindProperty("_values");
        }

        public override void OnInspectorGUI() {
            realTargetSO.Update();

            EditorGUILayout.BeginVertical();

            new_key = EditorGUILayout.TextField("Service Name", new_key);

            int currentIndex = keysSP.arraySize;
            if (GUILayout.Button("Add")) {
                keysSP.InsertArrayElementAtIndex(currentIndex);
                valuesSP.InsertArrayElementAtIndex(currentIndex);

                keysSP.GetArrayElementAtIndex(currentIndex).stringValue = new_key;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();

            for (int i = 0; i < System.Math.Min(keysSP.arraySize, valuesSP.arraySize); i++) {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.PropertyField(keysSP.GetArrayElementAtIndex(i), true);
                EditorGUILayout.PropertyField(valuesSP.GetArrayElementAtIndex(i), true);
                if (GUILayout.Button("Remove")) {
                    keysSP.DeleteArrayElementAtIndex(i);
                    valuesSP.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(); EditorGUILayout.Space();
            }

            realTargetSO.ApplyModifiedProperties();
        }
    }
}