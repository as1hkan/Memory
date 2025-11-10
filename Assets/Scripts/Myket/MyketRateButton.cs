using UnityEngine;

public class MyketRateButton : MonoBehaviour
{
    private const string PackageName = "com.ashkan.memorylane";

    // این تابع رو توی دکمه‌ات از قسمت OnClick انتخاب می‌کنی
    public void OpenMyketRatePage()
    {
#if UNITY_ANDROID
        try
        {
            string url = "myket://comment?id=" + PackageName;

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("parse", url);

                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW", uri);
                currentActivity.Call("startActivity", intent);
            }

            Debug.Log("⭐ صفحه‌ی امتیاز و نظر مایکت باز شد.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ خطا در باز کردن مایکت: " + e.Message);
        }
#endif
    }
}
