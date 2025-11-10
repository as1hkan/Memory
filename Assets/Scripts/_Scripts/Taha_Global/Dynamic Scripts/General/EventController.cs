using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// a simple and practical script to keep track of in game events.
/// 
/// useful for invoking events in animations, timelines and keeping the editor clean.
/// </summary>
public class EventController : MonoBehaviour
{
    [SerializeField, TextArea(1, 2)] string _jobDescription;
    [SerializeField] bool _hasManualEvent;
    [SerializeField] bool _invokeOnEnable;
    [SerializeField] bool _invokeOnDisable;

    [SerializeField, ConditionalField(nameof(_hasManualEvent))] UnityEvent _manualEvent;
    [SerializeField, ConditionalField(nameof(_invokeOnEnable))] UnityEvent _onEnableEvent;
    [SerializeField, ConditionalField(nameof(_invokeOnDisable))] UnityEvent _onDisableEvent;

    public void _InvokeManualEvent()
    {
        if (_hasManualEvent)
            _manualEvent.Invoke();
    }
    public void OnEnable()
    {
        if (_invokeOnEnable)
            _onEnableEvent.Invoke();
    }
    public void OnDisable()
    {
        if (_invokeOnDisable)
            _onDisableEvent.Invoke();
    }
}
