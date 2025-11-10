using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(play);
    }

    public void play()
    {
        MusicManager.instance.Play();
    }
}
