using UnityEngine;

public class MyketUpdater : MonoBehaviour
{
#if UNITY_ANDROID
    void Start()
    {
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
            Debug.LogError("Error: " + e.Message);
        }
    }
#endif
}
