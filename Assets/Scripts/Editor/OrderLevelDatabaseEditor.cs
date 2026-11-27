using UnityEditor;
using UnityEngine;

namespace InGame.Order.Editor
{
    /// <summary>
    /// OrderLevelDatabaseのInspectorに、成長曲線から各レベルの値を自動入力するボタンを追加する
    /// </summary>
    [CustomEditor(typeof(OrderLevelDatabase))]
    public class OrderLevelDatabaseEditor : UnityEditor.Editor
    {
        // 基準値（データベース本体には持たせず、Editor上で保持する）
        private float _minBaseValue = 1f;
        private float _maxBaseValue = 1f;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("自動入力設定", EditorStyles.boldLabel);
            _minBaseValue = EditorGUILayout.FloatField("最小値の基準値", _minBaseValue);
            _maxBaseValue = EditorGUILayout.FloatField("最大値の基準値", _maxBaseValue);

            EditorGUILayout.Space();
            if (GUILayout.Button("成長曲線から自動入力"))
            {
                GenerateFromCurve();
            }
        }

        private void GenerateFromCurve()
        {
            Undo.RecordObject(target, "Generate Order Level Data");
            serializedObject.Update();

            int levelCount = Mathf.Max(1, serializedObject.FindProperty("_levelCount").intValue);
            AnimationCurve minCurve = serializedObject.FindProperty("_minCurve").animationCurveValue;
            AnimationCurve maxCurve = serializedObject.FindProperty("_maxCurve").animationCurveValue;

            SerializedProperty levels = serializedObject.FindProperty("_levels");
            levels.arraySize = levelCount;

            for (int i = 0; i < levelCount; i++)
            {
                int level = i + 1;
                int min = Mathf.Max(0, Mathf.RoundToInt(_minBaseValue * minCurve.Evaluate(level)));
                int max = Mathf.Max(0, Mathf.RoundToInt(_maxBaseValue * maxCurve.Evaluate(level)));

                SerializedProperty entry = levels.GetArrayElementAtIndex(i);
                entry.FindPropertyRelative("_minTotal").intValue = min;
                entry.FindPropertyRelative("_maxTotal").intValue = max;
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}