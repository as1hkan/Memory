using UnityEngine;
using DG.Tweening;

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
        title.SetNextText("تنظیمات");
        title.DoFadeNow();

        transform.DOMove(settingsPos, moveDuration)
                 .SetEase(Ease.InOutSine);
  }

    public void GoBack()
    {
        inSettings = false;
        title.SetNextText("مسیر حافظه");
        title.DoFadeNow();

        transform.DOMove(mainMenuPos, moveDuration)
                 .SetEase(Ease.InOutSine);
    }
}
