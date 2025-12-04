using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public TextButtonColor musicButtonText;
    public TextButtonColor sfxButtonText;

    [Header("Music Settings")]
    public AudioSource audioSource;
    public List<AudioClip> musicList;
    private int lastIndex = -1;
    public bool musicMuted = false;

    [Header("SFX Settings")]
    public GameObject audioPrefab;
    public bool SFXMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        // Load saved states
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        SFXMuted = PlayerPrefs.GetInt("SfxMuted", 0) == 1;

        if (!musicMuted && musicList.Count > 0)
            PlayNextMusic();

        FindButtons();
        UpdateButtonColors();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindButtons();
        UpdateButtonColors();
    }

    private void FindButtons()
    {
        TextButtonColor[] found = FindObjectsOfType<TextButtonColor>(true);


        if (found.Length >= 1) musicButtonText = found[0];
        if (found.Length >= 2) sfxButtonText = found[1];
    }


    void PlayNextMusic()
    {
        if (musicMuted) return;
        if (musicList.Count == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, musicList.Count);
        }
        while (newIndex == lastIndex && musicList.Count > 1);

        lastIndex = newIndex;

        audioSource.clip = musicList[newIndex];
        audioSource.Play();

        StartCoroutine(WaitForMusicEnd());
    }

    IEnumerator WaitForMusicEnd()
    {
        while (audioSource.isPlaying && !musicMuted)
            yield return null;

        if (!musicMuted)
            PlayNextMusic();
    }

    public void OnMuteMusicButtonClick()
    {
        musicMuted = !musicMuted;

        PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);
        PlayerPrefs.Save();

        if (musicMuted)
            audioSource.Stop();
        else
            PlayNextMusic();

        UpdateButtonColors();
    }


    public void Play()
    {
        if (!SFXMuted)
            Instantiate(audioPrefab);
    }

    public void OnMuteSFXButtonClick()
    {
        SFXMuted = !SFXMuted;

        PlayerPrefs.SetInt("SfxMuted", SFXMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonColors();
    }

    void UpdateButtonColors()
    {
        if (musicButtonText != null)
            musicButtonText.SetState(!musicMuted);
        if (sfxButtonText != null)
            sfxButtonText.SetState(!SFXMuted);
    }
}
