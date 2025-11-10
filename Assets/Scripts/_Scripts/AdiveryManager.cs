using AdiveryUnity;
using UnityEngine;
using UnityEngine.Events;

public class AdiveryManager : Singleton_Abs<AdiveryManager>
{
    const string APP_ID = "ca1ae04f-fc02-43d2-89ca-993e1f455c35";
    const string PLACEMENT_INTER_ID = "612750f2-6344-4bbf-831d-58753cf9e174";
    const string _REWARD_REVIVE_ID = "";
    const string _REWARD_COIN_ID = "";

    public static UnityAction _onRewardedAdStart;
    public static UnityAction _onRewardedAdFinish;

    UnityEvent _currentReward;
    UnityEvent _failedEvent;

    AdiveryListener listener;
    private bool _areAdsRemoved = false;

    public void Start()
    {
        DontDestroyOnLoad(transform.root);

        _LoadAdRemovalData();

        Adivery.Configure(APP_ID);
        Adivery.PrepareInterstitialAd(PLACEMENT_INTER_ID);
        Adivery.PrepareRewardedAd(_REWARD_REVIVE_ID);
        Adivery.PrepareRewardedAd(_REWARD_COIN_ID);

        listener = new AdiveryListener();
        listener.OnRewardedAdClosed += _OnRewardedClosed;

        Adivery.AddListener(listener);
    }

    public void _OnRewardedClosed(object caller, AdiveryReward reward)
    {
        _onRewardedAdFinish?.Invoke();
        if (reward.IsRewarded)
        {
            _RewardPlayer();
        }
    }
    public bool _IsIntraAdLoaded()
    {
        if (Adivery.IsLoaded(PLACEMENT_INTER_ID))
        {
            return true;
        }
        return false;
    }
    public bool _IsRewardedAdLoaded()
    {
        if (Adivery.IsLoaded(_REWARD_REVIVE_ID))
        {
            return true;
        }
        return false;
    }
    public void _ShowInterAd()
    {
        //print("Intra is " + Adivery.IsLoaded(PLACEMENT_INTER_ID));
        if (Adivery.IsLoaded(PLACEMENT_INTER_ID))
        {
            if (_areAdsRemoved) return;

            Adivery.Show(PLACEMENT_INTER_ID);
        }
    }
    public void _ShowRewardedAd(_AdTypes iType, UnityEvent iReward, UnityEvent iFailedAction = null)
    {
        _currentReward = iReward;
        _failedEvent = iFailedAction;

        if (iType == _AdTypes.revive && Adivery.IsLoaded(_REWARD_REVIVE_ID))
        {
            Adivery.Show(_REWARD_REVIVE_ID);
            _onRewardedAdStart?.Invoke();
        }
        else if (iType == _AdTypes.coin && Adivery.IsLoaded(_REWARD_COIN_ID))
        {
            Adivery.Show(_REWARD_COIN_ID);
            _onRewardedAdStart?.Invoke();
        }
        else
        {
            _failedEvent.Invoke();
        }
    }
    private void _RewardPlayer()
    {
        if (_currentReward != null)
            _currentReward?.Invoke();
        _currentReward = null;
    }

    #region Ads Removeal
    public void _RemoveAds()
    {
        _areAdsRemoved = true;
        PlayerPrefs.SetInt(A.DataKey.areAdsRemoved, A.DataKey.True);
    }
    public bool _AreAdsRemoved()
    {
        // we dont use _areAdsRemoved because it may not be loaded in time
        return PlayerPrefs.GetInt(A.DataKey.areAdsRemoved, 0) == A.DataKey.True;
    }
    private void _LoadAdRemovalData()
    {
        _areAdsRemoved =
            PlayerPrefs.GetInt(A.DataKey.areAdsRemoved, 0) == A.DataKey.True;
    }
    #endregion
    #region Types
    public enum _AdTypes
    {
        revive, coin
    }
    #endregion
}
