using UnityEngine;
using TMPro;

public class TextButtonColor : MonoBehaviour
{
    public TextMeshProUGUI txt;

    public Color onColor = Color.white;     // روشن
    public Color offColor = Color.gray;     // خاموش

    private void Awake()
    {
        if (txt == null)
            txt = GetComponent<TextMeshProUGUI>();
    }

    public void SetState(bool isOn)
    {
        if (txt != null)
            txt.color = isOn ? onColor : offColor;
        else
            Debug.LogError("text null");
    }
}
