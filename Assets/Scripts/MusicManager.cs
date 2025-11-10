using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioSource audioSource;
    private bool SFXMuted = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Play()
    {
        if (!SFXMuted)
        {
            Instantiate(audioPrefab);
        }
    }
    public void OnMuteMusicButtonClick()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
    public void OnMuteSFXButtonClick()
    {
        SFXMuted = !SFXMuted;
    }
}
