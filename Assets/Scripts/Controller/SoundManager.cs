using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public string IS_OFF_MUSIC = "IS_OFF_MUSIC";

    public static SoundManager instance;
    void Awake()
    {
        SoundManager.instance = this;

        if(IsOnAudio())
        {
            PlayBgMusic();
        }
        else
        {
            PauseBgMusic();
        }
    }

    public void OnOffSound()
    {
        if (!IsOnAudio())
        {
            PlayBgMusic();
        }
        else
        {
            PauseBgMusic();
        }
    }

    public void PlayBgMusic()
    {
        PlayerPrefs.SetString(IS_OFF_MUSIC, "FALSE");
    }

    public void PauseBgMusic()
    {
        PlayerPrefs.SetString(IS_OFF_MUSIC, "TRUE");
    }

    public bool IsOnAudio()
    {
        if(PlayerPrefs.GetString(IS_OFF_MUSIC) == "TRUE")
        {
            return false;
        }

        return true;
    }
}
