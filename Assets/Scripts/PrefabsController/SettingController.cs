using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingController : MonoBehaviour
{
    public Image SoundBtn;
    public CanvasGroup Setting;
    public Sprite SoundOn, SoundOff;

    public Image OneDraw, TwoDraw, FourDraw;
    public Sprite Uncheck, Checked;

    public List<Sprite> CardBack;
    public Image CardBackImg;
    public bool IsPlayGame = false;

    void Start()
    {
        CheckSound();
    }

    public void CheckSound()
    {
        if (SoundManager.instance.IsOnAudio())
        {
            SoundBtn.sprite = SoundOn;
            AudioController.instance.Mute(false);
        }
        else
        {
            SoundBtn.sprite = SoundOff;
            AudioController.instance.Mute(true);
        }
    }

    public void OnSoundClick()
    {
        AudioController.instance.PlayButton();
        SoundManager.instance.OnOffSound();
        CheckSound();
    }

    public void ShowSetting(bool isPlayGame = false)
    {
        IsPlayGame = isPlayGame;
        Setting.alpha = 1;
        Setting.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        CheckCardBack();
        CheckSound();
        CheckUiMode();
    }

    public void CheckCardBack()
    {
       // int tempIndex = SceneManager.instance.GetCardBack();
        //if(CardBack.Count > tempIndex)
        //{
        //    CardBackImg.sprite = CardBack[tempIndex];
        //    Table.m_Instance.ChangeCardBack(tempIndex);
        //}
        
    }

    public void HideSetting()
    {
        Setting.alpha = 0;
        Setting.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public void OnSettingSound()
    {
        SoundManager.instance.OnOffSound();
    }

    public void SettingMode(int  mode)
    {
        AudioController.instance.PlayButton();
        //SceneManager.instance.SettingMode(mode);
        CheckUiMode();
    }

    public void CheckUiMode()
    {
        if (SceneManager.Mode == 1)
        {
            OneDraw.sprite = Checked;
            TwoDraw.sprite = Uncheck;
            FourDraw.sprite = Uncheck;
        }
        else if(SceneManager.Mode == 2)
        {
            OneDraw.sprite = Uncheck;
            TwoDraw.sprite = Checked;
            FourDraw.sprite = Uncheck;
        }
        else
        {
            OneDraw.sprite = Uncheck;
            TwoDraw.sprite = Uncheck;
            FourDraw.sprite = Checked;
        }
    }

    public void OnCardPacksClick()
    {
        AudioController.instance.PlayButton();
        HideSetting();
        SceneManager.instance.CardPackController.ShowCardPack(IsPlayGame);
    }

    public void OnBackClick()
    {
        AudioController.instance.PlayButton();
        HideSetting();
        if (!IsPlayGame)
        {
            SceneManager.instance.HomeController.ShowHome(false);
        }
        else
        {
            SceneManager.instance.PlayGameController.ShowPlayGame();
        }
    }

    public void InitMode()
    {
         if(SceneManager.Mode == 1)
        {
            GameData.MODEDRAW = GameData.eModeDraw.OneCard;
        }
         else if (SceneManager.Mode == 2)
        {
            GameData.MODEDRAW = GameData.eModeDraw.TwoCard;
        }
         else
         {
             GameData.MODEDRAW = GameData.eModeDraw.FourCard;
         }
    }

}
