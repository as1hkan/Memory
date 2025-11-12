using UnityEngine;
using UnityEditor;

public class GradientColorizerEditor : EditorWindow
{
    private Color startColor = Color.red;
    private Color endColor = Color.yellow;

    [MenuItem("Tools/Gradient Colorizer")]
    public static void ShowWindow()
    {
        GetWindow<GradientColorizerEditor>("Gradient Colorizer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Gradient Settings", EditorStyles.boldLabel);

        startColor = EditorGUILayout.ColorField("Start Color", startColor);
        endColor = EditorGUILayout.ColorField("End Color", endColor);

        GUILayout.Space(10);

        if (GUILayout.Button("Apply Gradient To Selection"))
        {
            ApplyGradientToSelection();
        }
    }

    private void ApplyGradientToSelection()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0) return;

        int count = selectedObjects.Length;

        for (int i = 0; i < count; i++)
        {
            float t = (count == 1) ? 0f : (float)i / (count - 1);
            Color currentColor = Color.Lerp(startColor, endColor, t);

            Renderer rend = selectedObjects[i].GetComponent<Renderer>();
            if (rend != null)
            {
                Undo.RecordObject(rend, "Apply Gradient Color");
                rend.material.color = currentColor;
            }
        }
    }
}
