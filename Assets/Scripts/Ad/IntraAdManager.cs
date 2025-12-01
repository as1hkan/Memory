using UnityEngine;
using AdiveryUnity;
using System.Collections;

public class IntraAdManager : MonoBehaviour
{
    private const string APP_ID = "ca1ae04f-fc02-43d2-89ca-993e1f455c35";
    private const string PLACEMENT_INTER_ID = "612750f2-6344-4bbf-831d-58753cf9e174";

    [Tooltip("زمان انتظار بین تبلیغ‌ها بر حسب ثانیه (پیش‌فرض: 300 = 5 دقیقه)")]
    [SerializeField] private float _adInterval = 300f;

    private bool _isReloading = false;

    private void Start()
    {
        Debug.Log("[Adivery] Initializing...");
        Adivery.Configure(APP_ID);

        // آماده‌سازی اولین تبلیغ
        Debug.Log("[Adivery] Preparing interstitial ad...");
        Adivery.PrepareInterstitialAd(PLACEMENT_INTER_ID);

        // شروع حلقه نمایش با فاصله زمانی
        StartCoroutine(ShowAdLoop());
    }

    private IEnumerator ShowAdLoop()
    {
        while (true)
        {
            Debug.Log($"[Adivery] Waiting {_adInterval} seconds before next ad...");
            yield return new WaitForSeconds(_adInterval);

            if (Adivery.IsLoaded(PLACEMENT_INTER_ID))
            {
                Debug.Log("[Adivery] Showing ad now!");
                Adivery.Show(PLACEMENT_INTER_ID);
                StartCoroutine(ReloadAfterDelay(5f));
            }
            else
            {
                Debug.Log("[Adivery] Ad not ready, preparing new ad...");
                Adivery.PrepareInterstitialAd(PLACEMENT_INTER_ID);
            }
        }
    }

    private IEnumerator ReloadAfterDelay(float delay)
    {
        if (_isReloading) yield break;
        _isReloading = true;

        yield return new WaitForSeconds(delay);
        Debug.Log("[Adivery] Reloading ad after show...");
        Adivery.PrepareInterstitialAd(PLACEMENT_INTER_ID);

        _isReloading = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
