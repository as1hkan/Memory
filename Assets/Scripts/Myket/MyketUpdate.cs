using UnityEngine;

public class MyketUpdater : MonoBehaviour
{
    void Start()
    {
        // 👇 خودکار در شروع بازی اجرا می‌شود
        CheckForUpdate();
    }

    private void CheckForUpdate()
    {
#if UNITY_ANDROID
        try
        {
            string packageName = "com.ashkan.memorylane"; // شناسه پکیج بازی‌ات
            string url = "myket://check-update?id=" + packageName;

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("parse", url);

                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW", uri);
                currentActivity.Call("startActivity", intent);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ خطا در بررسی آپدیت مایکت: " + e.Message);
        }
#endif
    }
}
