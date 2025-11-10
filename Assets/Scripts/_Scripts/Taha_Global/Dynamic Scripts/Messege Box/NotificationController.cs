using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class NotificationController : MonoBehaviour
{
    const string ANIM_TRIGGER = "show";

    [SerializeField] Text _descriptionText;
    [SerializeField] Animator _anim;

    CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.blocksRaycasts = false;
    }
    public void _ShowNotification(string iDescription)
    {
        _descriptionText.text = iDescription;
        _PanelActivation();
    }
    private void _PanelActivation()
    {
        // remember to check the animator so additional triggers dont cause bugs
        _anim.SetTrigger(ANIM_TRIGGER);
    }
}
