using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] _AudioChannels _audioType;
    [Tooltip("true => automatically adds _playAudio event to the button")]
    [SerializeField] bool _playOnButtonClick;
    [SerializeField] bool _useSavedAudio;
    [SerializeField] bool _playOneShot = false;
    [SerializeField] bool _playOnAwake = false;
    [SerializeField, Range(0, 1)] float _volume = 1;

    [SerializeField, ConditionalField(nameof(_useSavedAudio))]
    _SavedSounds _audioName;
    [SerializeField, ConditionalField(nameof(_useSavedAudio), true)]
    AudioClip _audio;

    private void Start()
    {
        if (_playOnButtonClick)
        {
            if (gameObject.TryGetComponent<Button>(out Button button))
                button.onClick.AddListener(_PlayAudio);
            else
                Debug.LogError("attach button component to " +
                    gameObject.name + " or deactivate _playOnButtonClick boolean");
        }
        if (_playOnAwake)
            _PlayAudio();
    }
    public void _PlayAudio()
    {
        if (_useSavedAudio)
            AudioManager._instance._PlayAudio(_audioType, _audioName, _playOneShot, _volume);
        else
            AudioManager._instance._PlayAudio(_audioType, _audio, _playOneShot, _volume);
    }
}
