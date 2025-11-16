using UnityEngine;
using System.Collections;

public class CameraMenu : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    [SerializeField] private float moveSpeed = 6f;   // سرعت ثابت و نرم
    [SerializeField] private int status = 0;

    public TitleChanger title;

    private bool waitingForFade = false;

    private void LateUpdate()
    {
        Vector3 targetPos = positions[status];

        // حرکت نرم بدون شتاب‌های عجیب
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // اگر رسیدیم به هدف → فید انجام بده
        if (waitingForFade && Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            waitingForFade = false;
            title.DoFadeNow();
        }
    }

    public void PlusStatus()
    {
        if (status < positions.Length - 1)
            status++;

        waitingForFade = true;
        title.SetNextText("تنظیمات");
    }

    public void ManfiStatus()
    {
        if (status > 0)
            status--;

        waitingForFade = true;
        title.SetNextText("مسیر حافظه");
    }
}
