using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayGameController : MonoBehaviour
{
    public PopupController PopupController;
    public CanvasGroup PlayGame;
    public Button m_Hint;
    public Button m_Undo;
    public Button m_Setting;
    public Button m_NewGame;
    public Text MoveText;
    public Text LevelText;
    public Text TimeText;
    public Button Win;
    public Button Lose;
    public Button Back;
    public GameObject LosePopUp;
    public GameObject PausePopUp;
    public GameObject Rate;
    public GameObject TextEnd;
    public GameObject Auto;
    //-------------------
    public Camera CameraMain;

    public Image ReceiveReward;
    public List<Sprite> ListReward = new List<Sprite>();

    public GameObject Holder;
    public List<GameObject> Effect;

    public bool isPlayGame = false;
    private int m_MoveCount;
    public int MoveCount
    {
        get { return m_MoveCount; }
        set
        {
            m_MoveCount = value;
            ShowMove(m_MoveCount);
        }
    }

    private int m_Time;
    public int TimePlay
    {
        get { return m_Time; }
        set
        {
            m_Time = value;
            ShowTime(m_Time);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowWin();
        }
    }

    public void LoadLevelText()
    {
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            LevelText.text = "Level: " + (SceneManager.instance.CurrentLevel + 1);
        }
        else
        {
            LevelText.text = "";
        }
    }

    public void OnSettingClick()
    {
        AudioController.instance.PlayButton();
        HidePlayGame();
        SceneManager.instance.MainSettingController.ShowMainSetting(true);
    }

    public void ShowPlayGame()
    {
        PlayGame.alpha = 1;
        isPlayGame = true;
        PlayGame.blocksRaycasts = true;
        CameraMain.transform.position = new Vector3(0, 0, -500);
        this.gameObject.transform.localPosition = Vector2.zero;
        PopupController.HidePopup();
        GameControl.Instance.ChangeBG(GameControl.Instance.GetBackGround());

    }

    public void StartGame()
    {
        if (isPlayGame)
        {
            NewGame();
            return;
        }
        isPlayGame = true;
        //Debug.Log("StartGame");
        SceneManager.instance.isPlayGame = true;
        //Table.m_Instance.InitTable();
        LeanTween.delayedCall(0.6f, Table.m_Instance.CHIABAI);
        TimePlay = 0;
        //SaveGame();

    }

    public void HidePlayGame()
    {
        PlayGame.alpha = 0;
        PlayGame.blocksRaycasts = false;
        CameraMain.transform.position = new Vector3(5000, 0, -500);
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public bool IsSave = false;
    public void SaveGame()
    {
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {
            SceneManager.instance.CurrentMapData = SceneManager.instance.GetMapData(SceneManager.instance.CurrentLevel);
            if (m_Time < 2 * GameData.TIMERATE)
            {
                SceneManager.instance.CurrentMapData.Score = 1;
                SceneManager.instance.CurrentMapData.Star = 3;
            }
            else if (m_Time < 3 * GameData.TIMERATE)
            {
                SceneManager.instance.CurrentMapData.Score = 1;
                SceneManager.instance.CurrentMapData.Star = 2;
            }
            else
            {
                SceneManager.instance.CurrentMapData.Score = 1;
                SceneManager.instance.CurrentMapData.Star = 1;
            }
            IsSave = true;
            SceneManager.instance.SavePoint();
        }
        else
        {
            SceneManager.instance.SaveTopScore(m_Time);
        }
    }

    private void ShowMove(int move)
    {
        MoveText.text = string.Format("{0} : {1}", "Move", move);
    }

    private void ShowTime(int time)
    {
        TimeText.text = string.Format("{0} {1:00}:{2:00}", "Time", time / 60, time % 60);
        //if (TimePlay >= 3 * GameData.TIMERATE)
        //{
        //    TimeBar.fillAmount = 0;
        //}
        //else
        //{
        //    TimeBar.fillAmount = (3 * GameData.TIMERATE - TimePlay) / (3 * GameData.TIMERATE);
        //    //Debug.LogError(TimeBar.fillAmount);
        //    //Debug.LogError(TimePlay);
        //}

    }

    public void StartTimeCount()
    {
        InvokeRepeating("AddTime", 0f, 1f);
    }

    public void ShowTextEnd()
    {
        TextEnd.SetActive(true);
        LeanTween.delayedCall(1, () => { TextEnd.SetActive(false); });
    }

    private void AddTime()
    {
        TimePlay += 1;
        //if(TimePlay >= 3* GameData.TIMERATE)
        //{
        //    TimeBar.fillAmount = 0;
        //}
        //else
        //{
        //    TimeBar.fillAmount = (3 * GameData.TIMERATE - TimePlay) / (3 * GameData.TIMERATE);
        //    //Debug.LogError(TimeBar.fillAmount);
        //    //Debug.LogError(TimePlay);
        //}
        //if (TimePlay <= GameData.TIMERATE)
        //{
        //    TimeBar.fillAmount = 1;
        //}
        //else if (TimePlay <= 2 * GameData.TIMERATE)
        //{
        //    TimeBar.fillAmount = 0.5f;
        //}
        //else
        //{
        //    TimeBar.fillAmount = 0.02f;
        //}
    }

    public void StopTimeCount()
    {
        CancelInvoke();
    }

    public void NewGame()
    {
        forceHide = false;
        Auto.SetActive(false);
        TextEnd.SetActive(false);
        AudioController.instance.PlayButton();
        SceneManager.instance.ShowIntertitialBanner();
        PopupController.instance.HidePopup();
        HideLosePopUp();
        ActiveUI(false);
        TimePlay = 0;
        MoveCount = 0;
        LoadLevelText();
        StopTimeCount();
        
        SceneManager.Mode = PlayerPrefs.GetInt("GAME_MODE");
        GameData.TIMERATE = 120;
        Table.m_Instance.ResetTable();
 

    }

    public void ActiveHintButton(bool isShow = true)
    {
        if (isShow)
        {
            m_Hint.interactable = true;
            m_Undo.interactable = true;
        }
        else
        {
            m_Hint.interactable = false;
            m_Undo.interactable = false;
        }
    }

    public void Hint()
    {
        AudioController.instance.PlayButton();
        Table.m_Instance.ShowHint();
    }

    public void Undo()
    {
        AudioController.instance.PlayButton();
        Table.m_Instance.HandleUndo();
    }

    public void ShowWin()
    {
        CancelInvoke();
        SceneManager.instance.ClearDataGame();
        AudioController.instance.PlayWinGame();
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
        {

            var curMap = SceneManager.instance.GetMapData(SceneManager.instance.CurrentLevel);
            if (curMap.Level != 999 && curMap.Score == 0 && curMap.Level % 5 == 0)
            {
                UnlockReward(curMap);
            }
            else
            {
                PopupController.gameObject.SetActive(true);
                PopupController.ShowPopup(m_Time);
            }
        }
        else
        {
            PopupController.gameObject.SetActive(true);
            PopupController.ShowPopup(m_Time);
        }
        SaveGame();
        ActiveUI(false);
    }

    public void RunWin()
    {
        SceneManager.instance.MapController.NextLevel();
        PopupController.HidePopup();
        Table.m_Instance.AnimationWin();
        ActiveUI(false);
        //NewGame();
        //NewGame();
    }

    public void ShowLose()
    {
        Time.timeScale = 0;
        LosePopUp.gameObject.SetActive(true);
        ActiveHintButton(false);
        ActiveUI(false);
        AudioController.instance.PlayLoseGame();
    }

    public void RunLose()
    {

        AudioController.instance.PlayLoseGame(false);
        AudioController.instance.PlayButton();
        LosePopUp.gameObject.SetActive(false);
        NewGame();
    }

    public void ActiveUI(bool isPlay = true)
    {
        //Debug.LogError("ActiveUI" + " " +  isPlay);
        if (isPlay)
        {
            //StartTimeCount();
            m_Undo.interactable = true;
            m_Hint.interactable = true;
           m_NewGame.interactable = true;
        }
        else
        {
            //StopTimeCount();
            m_Undo.interactable = false;
            m_Hint.interactable = false;
            m_NewGame.interactable = false;
        }
    }

    public void OnButtonBackClick()
    {
        AudioController.instance.PlayButton();
        HidePlayGame();
        SceneManager.instance.HomeController.ShowHome(true);

    }

    public void HideLosePopUp()
    {
        Time.timeScale = 1;
        LosePopUp.SetActive(false);
    }

    public void ShowPausePopUp()
    {
        Time.timeScale = 0;
        //Table.m_Instance.BlockClick();
        PausePopUp.SetActive(true);
    }

    public void HidePausePopup()
    {
        Time.timeScale = 1;
        //Table.m_Instance.EnableClick();
        PausePopUp.SetActive(false);
    
    }

    public void ButtonHome_LoseClick()
    {
        AudioController.instance.PlayButton();
        LosePopUp.SetActive(false);
        HidePlayGame();
        SceneManager.instance.HomeController.ShowHome(true);
    }

    public void ButtonHome_PauseClick()
    {
        Time.timeScale = 1;
        AudioController.instance.PlayButton();
        HidePausePopup();
        HidePlayGame();
        //Table.m_Instance.CancelMoveAll();
        SceneManager.instance.HomeController.ShowHome(true);
    }

    public void ButtonClose()
    {
        AudioController.instance.PlayButton();
        HidePausePopup();
    }

    public void ButtonReTry_Click()
    {
        AudioController.instance.PlayButton();
        HidePausePopup();
        NewGame();
    }

    public void OnShowRate(bool isShowRate)
    {
        int index = PlayerPrefs.GetInt(SceneManager.RATE_DATA);

        if (index == 0)
        {
            int level = PlayerPrefs.GetInt(SceneManager.LEVEL_RATING);
            //Debug.LogError(level);
            if (level == 3 && isShowRate)
            {
                Rate.SetActive(true);
                Rate.transform.SetAsLastSibling();
            }
            else if (!isShowRate)
            {
                level++;
                PlayerPrefs.SetInt(SceneManager.LEVEL_RATING, level);
            }
        }
    }

    void UnlockReward(MapData curMap)
    {
        if (curMap.Level % 20 == 0)
        {
            var data = PlayerPrefs.GetString("REWARD_DATA");
            SceneManager.instance.SetCardFaceNum();
            OnShowReceive(0);
            data += (SceneManager.instance.CurrentLevel + 1) + "-" + SceneManager.instance.GetCardFaceNum() * 998;
            bool canUnlockBg = SceneManager.instance.GetBGNum() <= SceneManager.instance.MAX_BG;
            bool canUnlockCp = SceneManager.instance.GetCardPackNum() <= SceneManager.instance.MAX_CARD_PACK;

            if (canUnlockBg)
            {
                SceneManager.instance.SetBGNum();
                data += "-" + SceneManager.instance.GetBGNum() * 999;
            }

            if (canUnlockCp)
            {
                SceneManager.instance.SetCardPackNum();
                data += "-" + SceneManager.instance.GetCardPackNum() * 1000;
            }
            data += ";";
            PlayerPrefs.SetString("REWARD_DATA", data);
        }
        else if (curMap.Level % 5 == 0)
        {
            CheckOtherReward(curMap);
        }
    }

    private void CheckOtherReward(MapData curMap)
    {
        bool canUnlockBg = SceneManager.instance.GetBGNum() <= SceneManager.instance.MAX_BG;
        bool canUnlockCp = SceneManager.instance.GetCardPackNum() <= SceneManager.instance.MAX_CARD_PACK;
        var data = PlayerPrefs.GetString("REWARD_DATA");
        if (canUnlockBg && canUnlockCp)
        {
            int temp = Random.Range(0, 100);
            if (temp < 50)
            {
                SceneManager.instance.SetBGNum();
                data += (SceneManager.instance.CurrentLevel + 1) + "-" + SceneManager.instance.GetBGNum() * 999 + ";";
                OnShowReceive(1);
            }
            else
            {
                SceneManager.instance.SetCardPackNum();
                data += (SceneManager.instance.CurrentLevel + 1) + "-" + SceneManager.instance.GetCardPackNum() * 1000 + ";";
                OnShowReceive(2);
            }
        }
        else if (canUnlockBg)
        {
            SceneManager.instance.SetBGNum();
            data += (SceneManager.instance.CurrentLevel + 1) + "-" + SceneManager.instance.GetBGNum() * 999 + ";";
            OnShowReceive(1);
        }
        else if (canUnlockCp)
        {
            SceneManager.instance.SetCardPackNum();
            data += (SceneManager.instance.CurrentLevel + 1) + "-" + SceneManager.instance.GetCardPackNum() * 1000 + ";";
            OnShowReceive(2);
        }
        PlayerPrefs.SetString("REWARD_DATA", data);
    }

    public void OnShowReceive(int index)
    {
        ReceiveReward.gameObject.SetActive(true);
        ReceiveReward.sprite = ListReward[index];
    }

    public void HideReward()
    {
        ReceiveReward.gameObject.SetActive(false);
        PopupController.gameObject.SetActive(true);
        PopupController.ShowPopup(m_Time, false);
        AudioController.instance.PlayWinGame();
    }

    public void CreateEffect(int index, Vector3 pos)
    {
        if (index >= 0)
        {
            GameObject temp = Instantiate(Effect[index]);
            temp.transform.SetParent(Holder.transform);
            temp.transform.position = CameraMain.WorldToScreenPoint(pos);
            temp.transform.localScale = Vector3.one;
            temp.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
                Destroy(temp);
            });
        }
    }

    public void CheckAuto()
    {
        if (forceHide)
            return;
        LeanTween.delayedCall(0.2f, () =>
        {
            Auto.SetActive(Table.m_Instance.CheckShowAuto());
        });
    }

    bool forceHide = false;
    public void OnAutoClick()
    {
        Auto.SetActive(false);
        forceHide = true;
        Table.m_Instance.AutoComplete();
    }
}
