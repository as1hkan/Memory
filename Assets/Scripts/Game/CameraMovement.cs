using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Animation Positions")]
    [SerializeField] private Vector3[] positions;
    [SerializeField] private int status = 0;

    [Header("Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 followOffset = new Vector3(0, 3.2f, -3.2f);

    private void LateUpdate()
    {
        if (player == null) return;

        // 🔹 موقعیت پایه = بازیکن + offset (زاویه ثابت)
        Vector3 baseFollowPos = player.position + followOffset;

        // 🔹 اگر positions تعریف شده بود، اون تغییر رو اضافه کن
        Vector3 animOffset = Vector3.zero;
        if (positions.Length > 0)
            animOffset = positions[status];

        // 🔹 موقعیت نهایی
        Vector3 targetPos = baseFollowPos + animOffset;

        // 🔹 حرکت نرم دوربین به هدف
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // 🔹 زاویه‌ی ثابت — از rotation فعلی استفاده کن (هیچ LookAt یا تغییر زاویه‌ای انجام نده)
        // یعنی همون زاویه‌ای که در Inspector تنظیم کردی، مثلاً X=36, Y=-45
    }

    public void PlusStatus()
    {
        if (status < positions.Length - 1)
            status++;
    }

    public void ManfiStatus()
    {
        if (status > 0)
            status--;
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
    }
}