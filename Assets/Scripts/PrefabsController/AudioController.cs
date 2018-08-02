using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public static AudioController instance;
    public AudioSource m_Control;
    public AudioClip[] Clip;
    private bool isMute = false;

    void Awake()
    {
        instance = this;

        if (!m_Control)
            m_Control = GetComponent<AudioSource>();

        Mute(PlayerPrefs.GetString("IS_OFF_MUSIC") == "TRUE");
    }

    public void PlaySoundSortCard()
    {
        m_Control.clip = Clip[0];
        m_Control.Play();
    }

    public void PlaySoundWrong()
    {
        m_Control.PlayOneShot(Clip[1]);
    }

    public void PlayBackGroundSound()
    {
        m_Control.clip = Clip[2];
        m_Control.loop = true;
        m_Control.Play();
    }

    public void OffBackGroundSound()
    {
        m_Control.loop = false;
        m_Control.Stop();
    }

    public void Mute(bool ismute = false)
    {
        isMute = ismute;
        if(isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void PlayWinGame(bool on = true)
    {
        if(!on)
        {
            m_Control.Stop();

        }
        else
        {
            m_Control.PlayOneShot(Clip[2]);
        }
    }

    public void PlayLoseGame(bool on = true)
    {
        if (!on)
        {
            m_Control.Stop();

        }
        else
        {
            m_Control.PlayOneShot(Clip[3]);
        }
    }

    public void PlayButton(bool on = true)
    {
        if (!on)
        {
            m_Control.Stop();

        }
        else
        {
            m_Control.PlayOneShot(Clip[4]);
        }
    }

    public void PlayAutoWin(bool on = true)
    {
        if (!on)
        {
            m_Control.Stop();

        }
        else
        {
            m_Control.PlayOneShot(Clip[5]);
        }
    }

    public void PlayShowHint(bool on = true)
    {
        if (!on)
        {
            m_Control.Stop();

        }
        else
        {
            m_Control.PlayOneShot(Clip[6]);
        }
    }
}
