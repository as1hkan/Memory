using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
    void Update()
    {
        // وقتی کلید P رو فشار بدی، از صحنه عکس می‌گیره
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot("icon.png", 1);
            Debug.Log("📸 Screenshot saved to project folder!");
        }
    }
}
