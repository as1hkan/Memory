using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraMenu : MonoBehaviour
{
    [Header("Camera Positions")]
    [SerializeField] private Vector3 mainMenuPos;
    [SerializeField] private Vector3 settingsPos;

    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.8f;

    [Header("Intro Drop Settings")]
    [SerializeField] private float introHeight = 12f;
    [SerializeField] private float introDuration = 2f;

    public TitleChanger title;

    private bool inSettings = false;

    private void Start()
    {
        DoIntroDrop();
    }

    private void DoIntroDrop()
    {
        Vector3 start = mainMenuPos + Vector3.up * introHeight;

        transform.position = start;

        transform.DOMove(mainMenuPos, introDuration)
                 .SetEase(Ease.OutCubic);
    }

    public void GoToSettings()
    {
        inSettings = true;

        // حرکت دوربین اجرای فوری
        transform.DOMove(settingsPos, moveDuration)
                 .SetEase(Ease.InOutSine);

        // 2 ثانیه بعد متن تغییر کند
        StartCoroutine(ChangeTitleDelayed("تنظیمات", 1.5f));
    }

    public void GoBack()
    {
        inSettings = false;

        transform.DOMove(mainMenuPos, moveDuration)
                 .SetEase(Ease.InOutSine);

        StartCoroutine(ChangeTitleDelayed("مسیر حافظه", 1.5f));
    }

    private IEnumerator ChangeTitleDelayed(string nextTitle, float delay)
    {
        yield return new WaitForSeconds(delay);

        title.SetNextText(nextTitle);
        title.DoFadeNow();
    }
}
