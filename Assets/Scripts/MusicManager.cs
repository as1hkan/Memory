using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Music Settings")]
    public AudioSource audioSource;
    public List<AudioClip> musicList;   // ⬅ اینجا چندتا آهنگ اضافه می‌کنی
    private int lastIndex = -1;

    [Header("SFX Settings")]
    public GameObject audioPrefab;
    private bool SFXMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        if (musicList.Count > 0)
            PlayNextMusic();   // شروع پخش رندوم
    }

    void PlayNextMusic()
    {
        if (musicList.Count == 0) return;

        int newIndex;

        // جلوگیری از تکراری پخش شدن
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
        while (audioSource.isPlaying)
            yield return null;

        PlayNextMusic();
    }


    // ------- SFX -------
    public void Play()
    {
        if (!SFXMuted)
            Instantiate(audioPrefab);
    }

    public void OnMuteMusicButtonClick()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        else
            PlayNextMusic();
    }

    public void OnMuteSFXButtonClick()
    {
        SFXMuted = !SFXMuted;
    }
}
