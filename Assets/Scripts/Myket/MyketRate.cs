using UnityEngine;

public class MyketRatePrompt : MonoBehaviour
{
    private const string PlayCountKey = "menu_open_count";
    private const int ShowEvery = 5;
    private const string PackageName = "com.ashkan.memorylane";

#if UNITY_ANDROID

    void Start()
    {
        int count = PlayerPrefs.GetInt(PlayCountKey, 0);
        count++;
        PlayerPrefs.SetInt(PlayCountKey, count);
        PlayerPrefs.Save();

        Debug.Log($"✅ کاربر برای بار {count} وارد منو شد.");

        // وقتی به مضرب ۵ رسید، مایکت باز شود
        if (count % ShowEvery == 0)
        {
            OpenMyketComment();
        }
    }

    private void OpenMyketComment()
    {
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

            Debug.Log("📣 باز کردن صفحه‌ی امتیاز و نظر در مایکت...");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ خطا در باز کردن مایکت برای امتیاز: " + e.Message);
        }
    }
#endif
}