using System.Collections.Generic;
using UnityEngine;

public class GradientColorizer : MonoBehaviour
{
    [Header("Objects to color")]
    public List<GameObject> objects;

    [Header("Gradient Colors")]
    public Color startColor = Color.red;
    public Color endColor = Color.yellow;

    void Start()
    {
        ApplyGradientColors();
    }

    public void ApplyGradientColors()
    {
        if (objects == null || objects.Count == 0) return;

        int count = objects.Count;

        for (int i = 0; i < count; i++)
        {
            float t = (count == 1) ? 0f : (float)i / (count - 1);
            Color currentColor = Color.Lerp(startColor, endColor, t);

            Renderer renderer = objects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = currentColor;
            }
        }
    }
}