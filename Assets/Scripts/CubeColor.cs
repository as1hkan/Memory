using UnityEngine;

public class CubeColor : MonoBehaviour
{
    private Renderer Renderer;

    [SerializeField] private int Index;
    [SerializeField] private Color Color;
    [SerializeField] private Color InvisibleColors;


    private void Awake()
    {
        Renderer = gameObject.GetComponent<Renderer>();
    }

    public void Setup(int index)
    {
        Index = index;
    }

    public void ApplyColors(Color newColor)
    {
        Color = newColor;
        Renderer.material.color = Color;
    }

    public void Visible(bool visible)
    {
        Renderer.material.color = visible ? Color : InvisibleColors;
    }

    public void ShowOriginalColor()
    {
        Renderer.material.color = Color;
    }
}
