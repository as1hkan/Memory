using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton_Abs<AudioManager>
{
    [Header("Default Attachments")]
    [SerializeField] AudioSource _audioChanelSFX1;
    [SerializeField] AudioSource _audioChanelSFX2;
    [SerializeField] AudioSource _audioChanelMusic;
    [SerializeField] AudioSource _audioChanelTalk;
    [SerializeField] _SavedAudiosClass[] _allSavedAudios;

    public void _PlayAudio(_AudioChannels iType, AudioClip iClip, bool _isOneShot = false, float iVolume = 1)
    {
        if (iClip == null) return;

        if (iType == _AudioChannels.SFX1)
        {
            if (_isOneShot)
            {
                _audioChanelSFX1.PlayOneShot(iClip);
                return;
            }
            _audioChanelSFX1.clip = iClip;
            _audioChanelSFX1.volume = iVolume;

            if (iClip != null)
                _audioChanelSFX1.Play();
            else
                _audioChanelMusic.Stop();
        }
        else if (iType == _AudioChannels.SFX2)
        {
            if (_isOneShot)
            {
                _audioChanelSFX2.PlayOneShot(iClip);
                return;
            }
            _audioChanelSFX2.clip = iClip;
            _audioChanelSFX2.volume = iVolume;

            if (iClip != null)
                _audioChanelSFX2.Play();
            else
                _audioChanelSFX2.Stop();
        }
        else if (iType == _AudioChannels.Music)
        {
            if (_isOneShot)
            {
                _audioChanelMusic.PlayOneShot(iClip);
                return;
            }
            _audioChanelMusic.clip = iClip;
            _audioChanelMusic.volume = iVolume;

            if (iClip != null)
                _audioChanelMusic.Play();
            else
                _audioChanelMusic.Stop();
        }
        else if (iType == _AudioChannels.Talk)
        {
            if (_isOneShot)
            {
                _audioChanelTalk.PlayOneShot(iClip);
                return;
            }
            _audioChanelTalk.clip = iClip;
            _audioChanelTalk.volume = iVolume;

            if (iClip != null)
                _audioChanelMusic.Play();
            else
                _audioChanelMusic.Stop();
        }
    }
    public void _PlayAudio(_AudioChannels iType, _SavedSounds iClipName, bool _isOneShot = false, float iVolume = 1)
    {
        for (int i = 0; i < _allSavedAudios.Length; i++)
        {
            if (_allSavedAudios[i]._audioName == iClipName.ToString())
            {
                _PlayAudio(iType, _allSavedAudios[i]._audio, _isOneShot, iVolume);
            }
        }
    }
    public void _StopGame()
    {
        _audioChanelSFX1.Stop();
        _audioChanelSFX2.Stop();
        _audioChanelTalk.Stop();
    }
    public void _ContinueGame()
    {
        if (_audioChanelSFX1.clip != null)
            _audioChanelSFX1.Play();
        if (_audioChanelSFX2.clip != null)
            _audioChanelSFX2.Play();
        if (_audioChanelTalk.clip != null)
            _audioChanelTalk.Play();
    }
    [CreateMonoButton("Make Enum")]
    public void _MakeEnum()
    {
        EnumGenerator._GenerateEnums("_SavedSounds", _allSavedAudios, nameof(_SavedAudiosClass._audioName));
        EnumGenerator._AddValueToFirst("_SavedSounds", "None");
    }

    [System.Serializable]
    public class _SavedAudiosClass
    {
        public string _audioName;
        public AudioClip _audio;
    }
}
public enum _AudioChannels
{
    SFX1, SFX2, Music, Talk
}