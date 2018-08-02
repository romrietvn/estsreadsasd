using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum SceneState
{
    Main,
    Klondike,
    Spider,
    FreeCell
}

public class MainSettingController : MonoBehaviour
{

    public static MainSettingController Instance;
    public CanvasGroup MainSetting;
    public SceneState Scene = SceneState.Main;
    public Image SoundBtn;
    public Sprite SoundOn, SoundOff;

    private Image OneDrawKlon, ThreeDrawKlon;
    public Image OneDrawSpider, TwoDrawSpider, FourDrawSpider;
    public Sprite Uncheck, Checked;

    public List<Sprite> CardFace;
    public Image CardFaceImg;
    public List<Sprite> BackGround;
    public Image BackGroundImg;
    public List<Sprite> CardPack;
    public Image CardPackImg;
    public bool IsPlayGame = false;


    public void ShowMainSetting(bool isplaygame = false)
    {
        IsPlayGame = isplaygame;
        MainSetting.alpha = 1;
        MainSetting.blocksRaycasts = true;
        CheckCardFace();
        CheckBG();
        CheckSound();
        CheckCardPack();
        //CheckUiModeKlondike();
      CheckUiModeSpider();
        transform.localPosition = new Vector2(0, 0);

    }

    public void HideMainSetting()
    {
        MainSetting.alpha = 0;
        MainSetting.blocksRaycasts = false;
        transform.localPosition = new Vector2(5000, 0);
    }

    void Awake()
    {
        Instance = this;

    }

    public void CheckSound()
    {
        bool _sound = SoundManager.instance.IsOnAudio();
        if (_sound)
        {
            SoundBtn.sprite = SoundOn;
            AudioListener.volume = 1;
            //AudioControllerSpider.instance.Mute(false);
        }
        else
        {
            SoundBtn.sprite = SoundOff;
            AudioListener.volume = 0;
            //AudioControllerSpider.instance.Mute(true);
        }
    }

    public void OnSoundClick()
    {
        AudioController.instance.PlayButton();
        SoundManager.instance.OnOffSound();
        //MainGameController.Instance.ChangeSound();
        CheckSound();
    }

    public void OnCardFaceClick()
    {
        //AudioController.instance.PlayButton();
        HideMainSetting();
        //MainSceneController.Instance.CardFaceController.ShowCardPack(IsPlayGame);
        //if (Scene == SceneState.Main)
        //{
        //    AudioController.instance.PlayButton();
        //    MainSceneController.Instance.CardFaceController.ShowCardPack(IsPlayGame);
        //}
        //else if (Scene == SceneState.FreeCell)
        //{
        //    AudioControllerFreeCell.instance.PlayButton();
        //    SceneManagerFreeCell.instance.CardFaceController.ShowCardPack(IsPlayGame);
        //}
        //else if (Scene == SceneState.Spider)
        //{
        //    AudioControllerSpider.instance.PlayButton();
        //    SceneManagerSpider.instance.CardFaceController.ShowCardPack(IsPlayGame);
        //}
        //else
        //{
        AudioController.instance.PlayButton();
        SceneManager.instance.CardFaceController.ShowCardPack(IsPlayGame);
        // }
    }

    public void OnBackGroundClick()
    {
        AudioController.instance.PlayButton();
        HideMainSetting();
        AudioController.instance.PlayButton();
        SceneManager.instance.BackGroundController.ShowBG(IsPlayGame);
    }

    public void OnCardPackClick()
    {
        AudioController.instance.PlayButton();
        HideMainSetting();
        AudioController.instance.PlayButton();
        SceneManager.instance.CardPackController.ShowCardPack(IsPlayGame);
    }

    public void OnBackClick()
    {
        //if (Scene == SceneState.Main)
        //{
        //    AudioController.instance.PlayButton();
        //    HideMainSetting();
        //    MainSceneController.Instance.MainHomeController.ShowMainHome();
        //}
        //else if (Scene == SceneState.Klondike)
        //{

        AudioController.instance.PlayButton();
        HideMainSetting();
        if (!IsPlayGame)
            SceneManager.instance.HomeController.ShowHome();
        else
            SceneManager.instance.PlayGameController.ShowPlayGame();
    }

    private void CheckCardFace()
    {
        int index = GameControl.Instance.GetCardFace();
        CardFaceImg.sprite = SceneManager.instance.CardFaceController.GetCurrentCardFace()[8];
    }

    private void CheckBG()
    {
        int index = GameControl.Instance.GetBackGround();
        BackGroundImg.sprite = SceneManager.instance.BackGroundController.BG[index];
    }

    private void CheckCardPack()
    {
        int index = GameControl.Instance.GetCardBack();
        //Debug.LogError(index);
        CardPackImg.sprite = SceneManager.instance.CardPackController.CardBack[index];
    }

    public void ChangeDrawModeKlondike(int mode)
    {
        AudioController.instance.PlayButton();
        GameControl.Instance.SetGameMode(mode);

    }

    public void CheckUiModeSpider()
    {
        int modespider = GameControl.Instance.GetGameMode();

        if (modespider == 1)
        {
            GameData.MODEDRAW = GameData.eModeDraw.OneCard;
            OneDrawSpider.sprite = Checked;
            TwoDrawSpider.sprite = Uncheck;
            FourDrawSpider.sprite = Uncheck;
        }
        else if (modespider == 2)
        {
            GameData.MODEDRAW = GameData.eModeDraw.TwoCard;
            OneDrawSpider.sprite = Uncheck;
            TwoDrawSpider.sprite = Checked;
            FourDrawSpider.sprite = Uncheck;
        }
        else
        {
            GameData.MODEDRAW = GameData.eModeDraw.FourCard;
            OneDrawSpider.sprite = Uncheck;
            TwoDrawSpider.sprite = Uncheck;
            FourDrawSpider.sprite = Checked;
        }
    }

    public void CHangeDrawModeSpider(int mode)
    {
        AudioController.instance.PlayButton();
        GameControl.Instance.SetGameMode(mode);
        CheckUiModeSpider();
    }

}
