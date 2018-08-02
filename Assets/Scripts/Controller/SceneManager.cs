using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class SceneManager : MonoBehaviour
{
    public Canvas MainCanvas;

    public const string USER_DATA = "USER_DATA";
    public const string TOP_SCORE = "TOP_SCORE";
    public const string BEST_SCORE = "BEST_SCORE";
    public const string RATE_DATA = "RATE_DATA";
    public const string LEVEL_RATING = "LEVEL_RATING";
    public readonly string DATA_GAME = "DATA_GAME";
    public const string CARD_BACK = "CARD_BACK";
    public readonly string CARD_FACE = "CARD_FACE";
    public readonly string BACK_GROUND = "BACK_GROUND";
    public const string BG_REWARD = "BG_REWARD";
    public const string CB_REWARD = "CB_REWARD";
    public const string CF_REWARD = "CF_REWARD";
    public const int NumberTopScore = 3;

    public HomeController HomeController;
    public MainSettingController MainSettingController;
    public CardPackController CardPackController;
    public LoadingController LoadingController;
    public PlayGameController PlayGameController;
    public BackGroundController BackGroundController;
    public CardFaceController CardFaceController;
    public MapController MapController;
    public MapBoxController MapBoxController;
    public PopUpRateController PopUpRateController;

    public int MapNumber = 5;
    public int LevelPerMap = 10;
    public int CurrentLevel = -1;
    public int NumberLevelShowPopUp = 20;
    public string RateURL = "";
    public static int Mode = 1;

    public int MAX_CARD_PACK = 30;
    public int MAX_BG = 20;

    public AdMobBanner Banner;
    public AdMobRectBanner RectBanner;
    public AdMobBannerInterstitial InterStitial;
    public float TimeShowInterstitial = 90f;
    private float _preTimeShowAds = 0f;
    bool isShowInterstitial = false;
    public bool isPlayGame = false;

    private float widthSize = 0f;

    public Dictionary<int, List<MapData>> AllMapData = new Dictionary<int, List<MapData>>();

    public int CurrentMap = 0;
    public MapData CurrentMapData;

    public GameMode m_GameMode = GameMode.Arcade;

    public static SceneManager instance;
    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        SceneManager.instance = this;
        //Debug.LogError(PlayerPrefs.GetInt(GAME_MODE));
        //InitIdAdMod();
        if (!PlayerPrefs.HasKey(RATE_DATA))
        {
            PlayerPrefs.SetInt(RATE_DATA, 0);
            PlayerPrefs.SetInt(LEVEL_RATING, 0);
        }

        if (!PlayerPrefs.HasKey(USER_DATA))
        {
            CreateNewDataUser();
        }

        SaveTopScore();
        LoadUserData();
        GoogleMobileAd.OnInterstitialClosed += OnHideIntertitialBanner;
        GoogleMobileAd.OnInterstitialLoaded += OnLoadIntertitialBannerSuccess;

    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        // Start game UI
        if (!PlayerPrefs.HasKey(DATA_GAME))
        {
            //File.WriteAllText(Application.persistentDataPath + "/" + "save.txt", "");
            PlayerPrefs.SetString(DATA_GAME, "");
            LoadingController.ShowLoading();
        }
        else
        {
            LoadDataGame();
            LoadingController.ShowLoading(true);
            PlayGameController.LoadLevelText();
        }
        Table.m_Instance.ChangeCardBack(GameControl.Instance.GetCardBack());
        HomeController.HideHome();
        MainSettingController.HideMainSetting();
        CardPackController.HideCardPack();
        BackGroundController.HideBG();
        CardFaceController.HideCardPack();
        PlayGameController.HidePlayGame();
        PopUpRateController.HidePopUpRate();
        MapController.HideMap();

            

    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            isShowInterstitial = true;
            ShowIntertitialBanner();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnButtonBackNativeClick();
        }

        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    PlayGameController.ShowLose();
        //}

        if (!isShowInterstitial)
        {
            _preTimeShowAds += Time.deltaTime;
            if (_preTimeShowAds >= TimeShowInterstitial)
            {
                _preTimeShowAds = 0;
                isShowInterstitial = true;
            }
        }
    }

    bool isLoadBanner = false;
    public void OnLoadIntertitialBannerSuccess()
    {
        isLoadBanner = true;
        Debug.Log(isLoadBanner);
    }

    public void LoadNewIntertitialBanner()
    {
        GoogleMobileAd.LoadInterstitialAd();
    }



    private bool lastBanner = false;
    public void ShowBanner(bool isNormal)
    {
        if (isNormal)
        {
            Banner.ShowBanner();
            RectBanner.HideBanner();
        }
        else
        {
            RectBanner.ShowBanner();
            Banner.HideBanner();
        }
        lastBanner = isNormal;
    }

    public void ShowIntertitialBanner()
    {
        if (isLoadBanner && isShowInterstitial)
        {
            RectBanner.HideBanner();
            Banner.HideBanner();
            InterStitial.ShowBanner();

        }
    }


    public void OnHideIntertitialBanner()
    {
        ShowBanner(lastBanner);
        isShowInterstitial = false;
        isLoadBanner = false;
        LoadNewIntertitialBanner();
    }

    public enum GameMode
    {
        Arcade,
        Classis
    }

    public enum ObjectManager
    {
        MainMenu,
        Message,
        PlayGame,
        Option,
    }

    public enum ObjectDialog
    {
        WinLose,
    }


    public enum TypeInit
    {
        Immediately,
        Delays,
    }

    private GameObject preScene;
    private GameObject preDialog;

    private void ClearData()
    {
        PlayerPrefs.DeleteKey(USER_DATA);
    }



    public void InitGameScene(ObjectManager nameObj, TypeInit typeInit)
    {
        if (preScene != null)
        {
            Destroy(preScene);
            preScene = null;
        }

        GameObject newObj = (GameObject)Instantiate(Resources.Load("Prefabs/" + nameObj.ToString()));
        newObj.transform.SetParent(gameObject.transform);
        newObj.transform.localScale = Vector3.one;
        newObj.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 1280);

        if (typeInit == TypeInit.Immediately)
        {
            newObj.transform.localPosition = Vector2.zero;
        }

        if (preScene == null)
        {
            preScene = newObj;
        }
    }

    public void InitDialog(ObjectManager nameObj, TypeInit typeInit)
    {
        if (preDialog != null)
        {
            Destroy(preDialog);
            preDialog = null;
        }

        GameObject newObj = (GameObject)Instantiate(Resources.Load("Prefabs/" + nameObj.ToString()));
        newObj.transform.SetParent(gameObject.transform);
        newObj.transform.localScale = Vector3.one;
        newObj.GetComponent<RectTransform>().sizeDelta = new Vector2(widthSize, (float)Screen.height / Screen.width * widthSize);

        if (typeInit == TypeInit.Immediately)
        {
            newObj.transform.localPosition = Vector2.zero;
        }

        if (preDialog == null)
        {
            preDialog = newObj;
        }
    }

    void CreateNewDataUser()
    {
        string userData = "";
        for (int i = 0; i < MapNumber; i++)
        {
            userData += "*";
            // => lock => star => score
            for (int j = 0; j < LevelPerMap; j++)
            {
                if (j != 0)
                {
                    userData += "+0-0-0";
                }
                else if (i == 0)
                {
                    userData += "+1-0-0";
                }
                else
                {
                    userData += "+0-0-0";
                }
            }
        }
        PlayerPrefs.SetString(USER_DATA, userData);
        //Debug.Log(userData);
    }

    void LoadUserData()
    {

        try
        {
            string dataLoader = PlayerPrefs.GetString(USER_DATA);
            var map = dataLoader.Split('*');
            for (int i = 1; i < map.Length; i++)
            {
                List<MapData> tempData = new List<MapData>();
                var level = map[i].Split('+');
                for (int j = 1; j < level.Length; j++)
                {
                    var data = level[j].Split('-');
                    MapData newLevel = new MapData(j, int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
                    tempData.Add(newLevel);
                }

                AllMapData.Add(i, tempData);
            }
            // Debug.Log(AllMapData.Count);
        }
        catch
        {
            CreateNewDataUser();
            LoadUserData();
        }

    }

    public void SavePoint()
    {
        CurrentLevel += 1;
        if (CurrentLevel >= MapNumber * LevelPerMap - 1)
        {
            CurrentLevel = MapNumber * LevelPerMap - 1;
        }
        if ((CurrentLevel - 1) / 20 == CurrentMap)
            CurrentMap += 1;
        if (AllMapData.ContainsKey(CurrentMap))
        {
            var dataList = AllMapData[CurrentMap];
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Level == CurrentMapData.Level)
                {

                    dataList[i] = CurrentMapData;
                    if (i < dataList.Count - 1)
                    {

                        dataList[i + 1].IsLock = false;
                    }
                    else if (i == dataList.Count - 1)
                    {
                        if (CurrentMap <= AllMapData.Count - 1)
                        {
                            AllMapData[CurrentMap + 1][0].IsLock = false;
                        }
                    }
                    break;
                }
            }
            AllMapData[CurrentMap] = dataList;
            SaveData();
            if (CurrentLevel > PlayerPrefs.GetInt("LevelMax"))
            {
                PlayerPrefs.SetInt("LevelMax", CurrentLevel);
            }
        }
    }

    void SaveData()
    {
        string userData = "";
        for (int i = 1; i <= AllMapData.Count; i++)
        {
            userData += "*";
            // => lock => star => score
            var data = AllMapData[i];
            //Debug.Log(data.Count);
            for (int j = 0; j < data.Count; j++)
            {

                int islock = data[j].IsLock ? 0 : 1;
                userData += "+" + islock + "-" + data[j].Star + "-" + data[j].Score;
            }
        }

        PlayerPrefs.SetString(USER_DATA, userData);


    }

    public void SaveTopScore(int time = 10000)
    {
        if (!PlayerPrefs.HasKey(TOP_SCORE + (1)))
        {

            for (int i = 0; i < NumberTopScore; i++)
            {
                PlayerPrefs.SetInt(TOP_SCORE + (i + 1), 10000);
            }
            PlayerPrefs.SetInt(BEST_SCORE, 0);
        }
        else
        {
            // Debug.Log("aaa");
            if (time < PlayerPrefs.GetInt(TOP_SCORE + 1))
            {

                PlayerPrefs.SetInt(TOP_SCORE + 3, PlayerPrefs.GetInt(TOP_SCORE + 2));
                PlayerPrefs.SetInt(TOP_SCORE + 2, PlayerPrefs.GetInt(TOP_SCORE + 1));
                PlayerPrefs.SetInt(TOP_SCORE + 1, time);

                if (time < 2 * GameData.TIMERATE)
                {
                    PlayerPrefs.SetInt(BEST_SCORE, (3 * GameData.TIMERATE - time) * 100);

                }
                else if (time < 3 * GameData.TIMERATE)
                {
                    PlayerPrefs.SetInt(BEST_SCORE, (5 * GameData.TIMERATE - time) * 50);

                }
                else
                {
                    PlayerPrefs.SetInt(BEST_SCORE, (20 * GameData.TIMERATE - time));

                }
            }
            else if (time < PlayerPrefs.GetInt(TOP_SCORE + 2))
            {
                PlayerPrefs.SetInt(TOP_SCORE + 3, PlayerPrefs.GetInt(TOP_SCORE + 2));
                PlayerPrefs.SetInt(TOP_SCORE + 2, time);
            }
            else if (time < PlayerPrefs.GetInt(TOP_SCORE + 3))
            {
                PlayerPrefs.SetInt(TOP_SCORE + 3, time);
            }


        }

        //PlayGameController.PopupController.ShowPopup(180);
    }

    public List<int> GetTopScore()
    {
        List<int> top = new List<int>();
        for (int i = 0; i < NumberTopScore; i++)
        {
            top.Add(PlayerPrefs.GetInt(TOP_SCORE + (i + 1)));
        }
        return top;
    }

    public int GetBestTime()
    {
        return PlayerPrefs.GetInt(TOP_SCORE + 1);
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt(BEST_SCORE);
    }

    public int GetCurrentLevel()
    {
        return AllMapData[CurrentMap].Count;

    }

    public void OnButtonBackNativeClick()
    {
        if (HomeController.Alpha.alpha == 1)
        {
            Application.Quit();
        }
        else if (MapController.Map.alpha == 1)
        {
            MapController.HideMap();
            if (HomeController.isPlayGame)
                HomeController.ShowHome(true);
            else
                HomeController.ShowHome();
        }
        else if (CardPackController.CardBackController.alpha == 1 || BackGroundController.BackGround.alpha == 1 || CardFaceController.CardBackController.alpha == 1)
        {
            CardPackController.HideCardPack();
            CardFaceController.HideCardPack();
            BackGroundController.HideBG();
            if (MainSettingController.IsPlayGame)
            {
                MainSettingController.ShowMainSetting(true);
            }
            else
                MainSettingController.ShowMainSetting();

        }
        else if (MainSettingController.MainSetting.alpha == 1)
        {
            //Debug.Log(HomeController.isPlayGame);
            MainSettingController.HideMainSetting();
            if (MainSettingController.IsPlayGame)
            {
                PlayGameController.ShowPlayGame();
            }
            else if (HomeController.isPlayGame)
            {
                HomeController.ShowHome(true);
            }
            else
            {
                HomeController.ShowHome();
            }
        }
        else
        {
            PlayGameController.HidePlayGame();
            HomeController.ShowHome(true);
        }
    }

    void OnApplicationQuit()
    {
        //Debug.LogError(isPlayGame);
        //if (PlayGameController.isPlayGame)
        
    }

    public void ClearDataGame()
    {
        PlayerPrefs.DeleteKey(SceneManager.instance.DATA_GAME);
    }

    void LoadDataGame()
    {
        string data =PlayerPrefs.GetString(SceneManager.instance.DATA_GAME);
        if (!string.IsNullOrEmpty(data))
        {
            //Debug.LogError(data);
            string[] datasub = data.Split('*');
            Table.m_Instance.LoadDataGame(datasub);
            CurrentLevel = int.Parse(datasub[0]);
            if (CurrentLevel < 0)
            {
                m_GameMode = GameMode.Classis;
            }
            else
            {
                SceneManager.instance.CurrentLevel = CurrentLevel;
                m_GameMode = GameMode.Arcade;
            }
        }
        int gameMode = GameControl.Instance.GetGameMode();
        if(gameMode == 1)
        {
            GameData.MODEDRAW = GameData.eModeDraw.OneCard;
        }
        else if(gameMode ==2)
        {
            GameData.MODEDRAW = GameData.eModeDraw.TwoCard;
        }
        else
        {
            GameData.MODEDRAW = GameData.eModeDraw.FourCard;
        }
    }

    public int GetCardFace()
    {
        return PlayerPrefs.GetInt(CARD_FACE);
    }


    public int GetBackGround()
    {
        return PlayerPrefs.GetInt(BACK_GROUND);
    }


    public void SetCardFace(int carFace)
    {
        if (PlayerPrefs.GetInt(CARD_FACE) != carFace)
        {
            PlayerPrefs.SetInt(CARD_FACE, carFace);
        }
    }


    public void SetBackGround(int backgorund)
    {
        if (PlayerPrefs.GetInt(BACK_GROUND) != backgorund)
        {
            PlayerPrefs.SetInt(BACK_GROUND, backgorund);
        }
    }
    public int GetCardBack()
    {
        return PlayerPrefs.GetInt(CARD_BACK);
    }

    public int GetCardPackNum()
    {
        return PlayerPrefs.GetInt(CB_REWARD);
    }

    public int GetBGNum()
    {
        return PlayerPrefs.GetInt(BG_REWARD);
    }

    public int GetCardFaceNum()
    {
        return PlayerPrefs.GetInt(CF_REWARD);
    }

    public void SetCardPackNum()
    {
        PlayerPrefs.SetInt(CB_REWARD, GetCardPackNum() + 1);
    }

    public void SetBGNum()
    {
        PlayerPrefs.SetInt(BG_REWARD, GetBGNum() + 1);
    }

    public void SetCardFaceNum()
    {
        PlayerPrefs.SetInt(CF_REWARD, GetCardFaceNum() + 1);
    }

    public MapData GetMapData(int level)
    {
        int index = 0;
        foreach (var item in AllMapData)
        {
            foreach (var map in item.Value)
            {
                if (map.Level + 20 * index == level + 1)
                    return map;
            }
            index++;
        }

        return new MapData(999, 1, 1, 1);
    }

    public void ChangeRender(bool isNormal)
    {
        if(isNormal)
        {
            MainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            MainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}
