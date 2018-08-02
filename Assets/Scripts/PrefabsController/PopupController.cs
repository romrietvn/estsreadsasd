using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public CanvasGroup Alpha;
    public GameObject Stars;
    public GameObject Panel;
    public GameObject[] Star;
    public GameObject TopScore, Arcade;
    public static PopupController instance;
    public Button Retry;
    public List<Sprite> Number = new List<Sprite>();
    public List<Sprite> Number1 = new List<Sprite>();
    public List<Sprite> Number2 = new List<Sprite>();
    public List<Image> TimeScore = new List<Image>();
    public List<Image> BestScore = new List<Image>();
    public Image Record;
    public List<Image> LevelText = new List<Image>();
    public GameObject Level;

    public List<Image> TimeScore2 = new List<Image>();
    public List<Image> BestScore1 = new List<Image>();
    public List<Image> BestScore2 = new List<Image>();
    public List<Image> BestScore3 = new List<Image>();
    public int MaxScore = 0;

    void Awake()
    {
        instance = this;
        MaxScore = PlayerPrefs.GetInt("BEST_SCORE");
    }

    public void ShowPopup(int time, bool isNormal = true)
    {
        Alpha.alpha = 1;
        Alpha.blocksRaycasts = true;
        Panel.gameObject.SetActive(true);
        gameObject.transform.localPosition = new Vector2(0, 150);
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            ShowPopUpModeArcade(time, isNormal);
        }
        else
        {
            ShowPopUpModeClassis(time, SceneManager.instance.GetTopScore());
        }
    }

    void ShowPopUpModeArcade(int time, bool isNormal)
    {
        Arcade.SetActive(true);
        TopScore.SetActive(false);
        Level.SetActive(true);
        foreach (var item in LevelText)
        {
            item.sprite = null;
        }

        Stars.SetActive(true);
        LeanTween.rotateX(Stars, 1f, 0.15f);
        Retry.gameObject.SetActive(true);
        if (time < 2 * GameData.TIMERATE)
        {
            Star[0].gameObject.SetActive(true);
            Star[1].gameObject.SetActive(true);
            Star[2].gameObject.SetActive(true);
        }
        else if (time < 3 * GameData.TIMERATE)
        {
            Star[0].gameObject.SetActive(true);
            Star[1].gameObject.SetActive(true);
        }
        else
        {
            Star[0].gameObject.SetActive(true);
        }
        int currentLevel = SceneManager.instance.CurrentLevel + 1;
        if (!isNormal)
        {
            currentLevel -= 1;
        }
        var temp = currentLevel.ToString();
        for (int i = 0; i < temp.Length; i++)
        {
            var ind = int.Parse(temp[i].ToString());
            LevelText[i].sprite = Number[ind];
            LevelText[i].SetNativeSize();
        }

        if (currentLevel >= SceneManager.instance.MapNumber * SceneManager.instance.LevelPerMap)
            currentLevel = SceneManager.instance.MapNumber * SceneManager.instance.LevelPerMap;
        if (currentLevel % SceneManager.instance.NumberLevelShowPopUp == 0 && currentLevel > 0)
            SceneManager.instance.PopUpRateController.ShowPopUpRate();

        var best = time;
        //var data = SceneManager.instance.GetMapData(SceneManager.instance.CurrentLevel);
        var data = MaxScore;
        if (time < data || data == 0)
        {
            Record.color = new Color(1, 1, 1, 1);
            PlayerPrefs.SetInt("BEST_SCORE", best);
            MaxScore = best;
        }
        else
        {
            Record.color = new Color(1, 1, 1, 0);
            best = data;
        }

        var min = time / 60;
        var sec = time % 60;
        TimeScore[0].sprite = min < 10 ? Number[0] : Number[min / 10];
        TimeScore[1].sprite = Number[min % 10];
        TimeScore[2].sprite = sec < 10 ? Number[0] : Number[sec / 10];
        TimeScore[3].sprite = Number[sec % 10];

        var min2 = data / 60;
        var sec2 = data % 60;
        BestScore[0].sprite = min2 < 10 ? Number[0] : Number[min2 / 10];
        BestScore[1].sprite = Number[min2 % 10];
        BestScore[2].sprite = sec2 < 10 ? Number[0] : Number[sec2 / 10];
        BestScore[3].sprite = Number[sec2 % 10];
    }

    void ShowPopUpModeClassis(int time, List<int> topscore)
    {
        Level.SetActive(false);
        Retry.gameObject.SetActive(false);
        Arcade.SetActive(false);
        TopScore.SetActive(true);
        int count = topscore.Count;
        for (int i = 0; i < count; i++)
        {
            if (topscore[i] != 10000)
            {
                var best = topscore[i];
                var min2 = best / 60;
                var sec2 = best % 60;
                if (i == 0)
                {
                    BestScore1[0].sprite = min2 < 10 ? Number1[0] : Number1[min2 / 10];
                    BestScore1[1].sprite = Number1[min2 % 10];
                    BestScore1[2].sprite = sec2 < 10 ? Number1[0] : Number1[sec2 / 10];
                    BestScore1[3].sprite = Number1[sec2 % 10];
                    BestScore1[4].color = new Color(1, 1, 1, 1);
                }
                else if (i == 1)
                {
                    BestScore2[0].sprite = min2 < 10 ? Number1[0] : Number1[min2 / 10];
                    BestScore2[1].sprite = Number1[min2 % 10];
                    BestScore2[2].sprite = sec2 < 10 ? Number1[0] : Number1[sec2 / 10];
                    BestScore2[3].sprite = Number1[sec2 % 10];
                    BestScore2[4].color = new Color(1, 1, 1, 1);
                }
                else
                {
                    BestScore3[0].sprite = min2 < 10 ? Number1[0] : Number1[min2 / 10];
                    BestScore3[1].sprite = Number1[min2 % 10];
                    BestScore3[2].sprite = sec2 < 10 ? Number1[0] : Number1[sec2 / 10];
                    BestScore3[3].sprite = Number1[sec2 % 10];
                    BestScore3[4].color = new Color(1, 1, 1, 1);
                }
            }
        }

        var min = time / 60;
        var sec = time % 60;
        TimeScore2[0].sprite = min < 10 ? Number2[0] : Number2[min / 10];
        TimeScore2[1].sprite = Number2[min % 10];
        TimeScore2[2].sprite = sec < 10 ? Number2[0] : Number2[sec / 10];
        TimeScore2[3].sprite = Number2[sec % 10];

        // SceneManager.instance.PlayGameController.SaveGame();
    }

    public void HidePopup()
    {
        Alpha.alpha = 0;
        Alpha.blocksRaycasts = false;
        Panel.gameObject.SetActive(true);
        Stars.gameObject.SetActive(false);
        Stars.transform.eulerAngles = new Vector3(0, 0, 0);
        TopScore.gameObject.SetActive(false);
        int count = Star.Length;
        for (int i = 0; i < count; i++)
        {
            Star[i].gameObject.SetActive(false);
        }
        gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public void OnButtonRetryClick()
    {
        AudioController.instance.PlayWinGame(false);
        AudioController.instance.PlayButton();
        HidePopup();
        if (SceneManager.instance.PlayGameController.IsSave && SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            SceneManager.instance.PlayGameController.IsSave = false;
            SceneManager.instance.CurrentLevel -= 1;
            SceneManager.instance.CurrentMapData = SceneManager.instance.GetMapData(SceneManager.instance.CurrentLevel);
        }
        Table.m_Instance.AnimationWin();

        //SceneManager.instance.ShowIntertitialBanner();
    }

    public void OnButtonNextClick()
    {
        AudioController.instance.PlayWinGame(false);
        AudioController.instance.PlayButton();
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            SceneManager.instance.PlayGameController.RunWin();
        }
        else
        {
            //SceneManager.instance.MapController.NextLevel();
            SceneManager.instance.PlayGameController.PopupController.HidePopup();
            Table.m_Instance.AnimationWin();
        }
        //SceneManager.instance.ShowIntertitialBanner();
    }

    public void OnMenuClick()
    {
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            SceneManager.instance.PlayGameController.RunWin();
        }
        SceneManager.instance.PlayGameController.OnButtonBackClick();
    }

}
