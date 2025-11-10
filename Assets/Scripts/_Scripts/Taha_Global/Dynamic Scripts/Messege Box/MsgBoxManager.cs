using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Centralized system for showing interactive message boxes globally across scenes.
/// With this manager, you dont need to make multiple panels for interacting with the player.
/// you can use MsgBoxController for easier usage in the scene.
/// 
/// Key Features:
/// • Supports both confirmation (Yes/No) and notification-style messages.
/// • Automatically manages scene persistence with DontDestroyOnLoad.
/// • Supports both UnityAction and UnityEvent callbacks for flexibility.
/// </summary>

public class MsgBoxManager : Singleton_Abs<MsgBoxManager>
{
    [SerializeField] YesNoPanelController _yesNoController;
    [SerializeField] NotificationController _NotificationController;

    private void Start()
    {
        DontDestroyOnLoad(transform.root);
    }
    public void _ShowYesNoMessage(string iTitle, string iDescription, params UnityAction[] iYesActions)
    {
        _yesNoController._OpenMenu(iTitle, iDescription, iYesActions);
    }
    public void _ShowYesNoMessage(string iTitle, string iDescription, UnityEvent iYesActions)
    {
        _yesNoController._OpenMenu(iTitle, iDescription, iYesActions);
    }
    public void _ShowNotificationMessage(string iTitle)
    {
        _NotificationController._ShowNotification(iTitle);
    }
}
