using System.Collections;
using UnityEngine;
using TMPro;

public class TitleChanger : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public float fadeDuration = 0.35f;

    private string nextText;

    public void SetNextText(string txt)
    {
        nextText = txt;
    }

    public void DoFadeNow()
    {
        StartCoroutine(FadeTo(nextText));
    }

    IEnumerator FadeTo(string newText)
    {
        Color c = titleText.color;

        // FADE OUT
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            titleText.color = new Color(c.r, c.g, c.b, 1 - p);
            yield return null;
        }

        // تغییر متن
        titleText.text = newText;

        // FADE IN
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            titleText.color = new Color(c.r, c.g, c.b, p);
            yield return null;
        }
    }
}
