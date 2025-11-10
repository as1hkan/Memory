using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// this script is used to make using MsgBoxManager possible in the inspector.
/// </summary>
public class MsgBoxController : MonoBehaviour
{
    [SerializeField] _AllMsgTypes _messageType;

    [Header("Functions")]
    [SerializeField] bool _autoAddToButtons = false;
    [SerializeField, ConditionalField(nameof(_autoAddToButtons))]
    UnityEngine.UI.Button _msgButton;

    [Header("Message Info")]
    [SerializeField] string _title;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo), TextArea]
    string _description;

    [SerializeField, ConditionalEnum(nameof(_messageType), (int)_AllMsgTypes.yesNo)]
    UnityEvent _confirmEvent;
    UnityAction _lastAddedAction;

    private void Start()
    {
        if (_autoAddToButtons)
            _msgButton.onClick.AddListener(_StartMsg);
    }
    private void OnDisable()
    {
        if (_confirmEvent != null && _lastAddedAction != null)
        {
            _confirmEvent.RemoveListener(_lastAddedAction);
            _lastAddedAction = null;
        }
    }

    /// <summary>
    /// The method opens the msg box.
    /// you can call this via events in the inspector as well.
    /// </summary>
    public void _StartMsg()
    {
        #region Editor Only
#if UNITY_EDITOR
        if (MsgBoxManager._instance == null)
        {
            Debug.LogError("There is no MsgBoxManager in the scene");
            return;
        }
#endif
        #endregion

        if (_messageType == _AllMsgTypes.notification)
            MsgBoxManager._instance._ShowNotificationMessage(_title);
        else if (_messageType == _AllMsgTypes.yesNo)
            MsgBoxManager._instance._ShowYesNoMessage(_title, _description, _confirmEvent.Invoke);
    }

    /// <summary>
    /// this used for managing the yes action event.
    /// you can add a new events or override the current yes action events;
    /// </summary>
    public void _AddEvent(UnityAction iAction, bool iRemoveOtherEvents = false)
    {
        if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

        _confirmEvent.AddListener(iAction);
    }

    /// <summary>
    /// this used for managing the yes action event.
    /// you can add a new events or override the current yes action events;
    /// </summary>
    public void _AddEvent(UnityEvent iAction, bool iRemoveOtherEvents = false)
    {
        if (iRemoveOtherEvents) _confirmEvent.RemoveAllListeners();

        _confirmEvent.AddListener(iAction.Invoke);
    }

    /// <summary>
    /// this is a simple system to do something with confirmation.
    /// 
    /// this temporarily override the current yes action events and only call the 
    ///     event passed as the parameter.
    /// 
    /// sample: when you want to start the next level, instead of calling the _StartNextLevel,
    ///     call msgBoxController._AskForConfirmation(_StartNextLevel);
    /// </summary>
    public void _AskForConfirmation(UnityEvent iEvent)
    {
        _lastAddedAction = iEvent.Invoke;
        _confirmEvent.AddListener(_lastAddedAction);

        _StartMsg();
    }

    /// <summary>
    /// this is a simple system to do something with confirmation.
    /// 
    /// this temporarily override the current yes action events and only call the 
    ///     event passed as the parameter.
    /// 
    /// sample: when you want to start the next level, instead of calling the _StartNextLevel,
    ///     call msgBoxController._AskForConfirmation(_StartNextLevel);
    /// </summary>
    public void _AskForConfirmation(UnityAction iEvent)
    {
        _lastAddedAction = iEvent;
        _confirmEvent.AddListener(_lastAddedAction);

        _StartMsg();
    }
    public enum _AllMsgTypes
    {
        notification, yesNo
    }
}
