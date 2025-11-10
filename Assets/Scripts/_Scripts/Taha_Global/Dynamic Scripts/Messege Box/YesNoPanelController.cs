using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class YesNoPanelController : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] Button _exitButton;
    [SerializeField] Button _cancelButton;
    [SerializeField] Button _confirmButton;
    [SerializeField] Text _title;
    [SerializeField] Text _description;

    CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _exitButton.onClick.AddListener(_CloseMenu);
        _cancelButton.onClick.AddListener(_CloseMenu);
    }
    public void _OpenMenu(string iTitle, string iDescription, params UnityAction[] iYesActions)
    {
        _ActivateMenu(true);

        _title.text = iTitle;
        _description.text = iDescription;

        _confirmButton.onClick.RemoveAllListeners();
        foreach (UnityAction action in iYesActions)
        {
            _confirmButton.onClick.AddListener(action);
        }
        _confirmButton.onClick.AddListener(_CloseMenu);
    }
    public void _OpenMenu(string iTitle, string iDescription, UnityEvent iYesActions)
    {
        _ActivateMenu(true);

        _title.text = iTitle;
        _description.text = iDescription;

        _confirmButton.onClick.RemoveAllListeners();

        if (iYesActions != null)
            _confirmButton.onClick.AddListener(iYesActions.Invoke);
        _confirmButton.onClick.AddListener(_CloseMenu);
    }
    public void _CloseMenu()
    {
        _ActivateMenu(false);
    }
    private void _ActivateMenu(bool iActivation)
    {
        _canvasGroup.blocksRaycasts = iActivation;
        _canvasGroup.alpha = iActivation ? 1 : 0;
        _canvasGroup.interactable = iActivation;
        gameObject.SetActive(iActivation);
    }
}
