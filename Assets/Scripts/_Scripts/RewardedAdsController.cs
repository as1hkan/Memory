using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedAdsController : MonoBehaviour
{
    [Header("Function Settings")]
    [SerializeField] bool _showMsgBox = false;
    [SerializeField] bool _hasCoolDown = false;

    [Header("Attachments")]
    [SerializeField] AdiveryManager._AdTypes _adType;
    [SerializeField, ConditionalField(nameof(_showMsgBox))]
    MsgBoxController _msgBoxController;

    [SerializeField, ConditionalField(nameof(_hasCoolDown))] _TimerNames _timerName;
    [SerializeField, ConditionalField(nameof(_hasCoolDown))] float _adCoolDown;
    [SerializeField, ConditionalField(nameof(_hasCoolDown))] Text _adTimerText;
    [SerializeField, ConditionalField(nameof(_hasCoolDown))] GameObject _adTimerPanel;

    [SerializeField] Button _button;
    [SerializeField] _RewardTypes _rewardType;

    [Header("Events")]
    [SerializeField] bool _showEvents;
    [SerializeField, ConditionalField(nameof(_showEvents))] UnityEvent _afterWatchEvent;
    [SerializeField, ConditionalField(nameof(_showEvents))] UnityEvent _afterFailedEvent;
    [SerializeField, ConditionalField(nameof(_showEvents))] UnityEvent _onCooldownStart;
    [SerializeField, ConditionalField(nameof(_showEvents))] UnityEvent _onCooldownFinish;

    private Coroutine _updateUiRoutine;

    private void Start()
    {
        _CheckAdCoolDown();

        _afterWatchEvent.AddListener(_GetRewardAction());
        if (_showMsgBox)
            _afterWatchEvent.AddListener(() => _msgBoxController._StartMsg());

        _button.onClick.AddListener(() => AdiveryManager._instance
            ._ShowRewardedAd(_adType, _afterWatchEvent, _afterFailedEvent));
    }
    private void OnDisable()
    {
        if (_updateUiRoutine != null)
            StopCoroutine(_updateUiRoutine);
    }
    private void _CheckAdCoolDown()
    {
        if (!_hasCoolDown) return;

        if (TimeManager._instance._GetTimerRemainingSec(_timerName) > 0)
            _ActivateAdCooldownPanel(true);
    }
    private void _StartAdCooldown()
    {
        TimeManager._instance._SetNewTimer(_timerName, _adCoolDown);
        _CheckAdCoolDown();
    }
    private void _ActivateAdCooldownPanel(bool iActivation)
    {
        if (iActivation)
        {
            _updateUiRoutine = StartCoroutine(_UpdateUiEachSec());
            _onCooldownStart.Invoke();
        }
        else
        {
            if (_updateUiRoutine != null)
            {
                StopCoroutine(_updateUiRoutine);
                _updateUiRoutine = null;
            }
            _onCooldownFinish.Invoke();
        }

        _adTimerPanel.SetActive(iActivation);
        _button.interactable = !iActivation;
    }
    private UnityAction _GetRewardAction()
    {
        //if (_rewardType == _RewardTypes.revive)
        //    return () => ReviveManager._instance._ReviveAfterLose(true);

        //if (_rewardType == _RewardTypes.coin)
        //{
        //    UnityEvent iEvent = new UnityEvent();
        //    iEvent.AddListener(() => ShopManager._instance._RewardCoinForWatchingAds());
        //    iEvent.AddListener(() => _StartAdCooldown());

        //    return iEvent.Invoke;
        //}
        return null;
    }
    private IEnumerator _UpdateUiEachSec()
    {
        while (TimeManager._instance._GetTimerRemainingSec(_timerName) > 0)
        {
            _adTimerText.text = TimeManager._instance._GetStringTimerText(_timerName);
            yield return new WaitForSeconds(1);
        }
        _ActivateAdCooldownPanel(false);
    }

    public enum _RewardTypes
    {
        revive, coin
    }
}