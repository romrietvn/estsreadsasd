using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public Sprite m_SpriteCard;
    public GameObject m_PrefabCard;
    public Camera m_Camera;
    public SpriteRenderer m_BackGround;
    public GameObject m_PrefabCardBg;
    public Dictionary<int, int> LevelSettings;
    public List<int> Level = new List<int>();
    public List<int> Suit = new List<int>();
    public Difficulty MODE = Difficulty.Advance;
    public Sprite[] BGs;
    public readonly string CARD_FACE = "CARD_FACE";
    public readonly string BACK_GROUND = "BACK_GROUND";
    public readonly string GAME_MODE = "GAME_MODE";
    public const string CARD_BACK = "CARD_BACK";

    public enum Difficulty
    {
        Beginer,
        Intermediate,
        Advance
    }

    void Awake()
    {
        Instance = this;
        if (!m_Camera)
            m_Camera.GetComponent<Camera>();
        InitGameData();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 45;
        if (!PlayerPrefs.HasKey(CARD_BACK))
        {
            PlayerPrefs.SetInt(CARD_BACK, 0);
        }

        if (!PlayerPrefs.HasKey(CARD_FACE))
        {
            PlayerPrefs.SetInt(CARD_FACE, 0);
        }
        if (!PlayerPrefs.HasKey(BACK_GROUND))
        {
            PlayerPrefs.SetInt(BACK_GROUND, 0);
        }
        if(!PlayerPrefs.HasKey(GAME_MODE))
        {
            PlayerPrefs.SetInt(GAME_MODE, 1);
        }
    }

    void Start()
    {
        ChangeBG(GetBackGround());
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
    public void SetGameMode(int mode)
    {
        if (PlayerPrefs.GetInt(GAME_MODE) != mode)
        {
            PlayerPrefs.SetInt(GAME_MODE, mode);
        }
    }

    public int GetGameMode()
    {
        return PlayerPrefs.GetInt(GAME_MODE);
    }

    public bool IsModeOneDraw()
    {
        if (PlayerPrefs.GetInt(GAME_MODE) == 1)
        {
            return true;
        }

        return false;
    }

    public void SettingMode(bool isOneMode)
    {
        if (isOneMode)
        {
            if (!IsModeOneDraw())
            {
                PlayerPrefs.SetInt(GAME_MODE, 1);
                //if (SceneManager.instance.SettingController.IsPlayGame == true)
                //    PlayGameController.NewGame();
                GameData.MODEDRAW = GameData.eModeDraw.OneCard;
            }
        }
        else
        {
            if (IsModeOneDraw())
            {
                PlayerPrefs.SetInt(GAME_MODE, 3);
                //if (SceneManager.instance.SettingController.IsPlayGame == true)
                //    PlayGameController.NewGame();
                GameData.MODEDRAW = GameData.eModeDraw.TwoCard;
            }
        }
    }

    public void SetCardBack(int cardBack)
    {
        if (PlayerPrefs.GetInt(CARD_BACK) != cardBack)
        {
            PlayerPrefs.SetInt(CARD_BACK, cardBack);
        }
    }

    public int GetCardBack()
    {
        return PlayerPrefs.GetInt(CARD_BACK);
    }

    private void InitGameData()
    {

        GameData.SCREEN_WIDTH = Screen.width;
        GameData.SCREEN_HEIGHT = Screen.height;
        GameData.CARD_WIDTH = m_SpriteCard.bounds.size.x;
        GameData.CARD_HEIGHT = m_SpriteCard.bounds.size.y;

        GameData.CARD_SCALE = m_PrefabCard.transform.localScale.x;

        GameData.CARD_SCALE = (GameData.SCREEN_WIDTH * GameData.CARD_WIDTH * 100 * GameData.CARD_SCALE) / (800 * GameData.CARD_WIDTH * 100);
        GameData.CARD_WIDTH *= GameData.CARD_SCALE;
        GameData.CARD_HEIGHT *= GameData.CARD_SCALE / 9 * 10;
        GameData.CARD_RATIO_HEIGHT = GameData.CARD_HEIGHT / GameData.CARD_PERCENT;
        GameData.CARD_RATIO_HEIGH_UP = GameData.CARD_RATIO_HEIGHT / 3;
        GameData.CARD_RATIO_WIDTH = GameData.CARD_WIDTH / 2f;
        GameData.CARDDRAW_RATIO = GameData.CARD_WIDTH / GameData.CARDDRAW_PERCENT;
        m_PrefabCardBg.transform.localScale = new Vector3(GameData.CARD_SCALE, GameData.CARD_SCALE / 9 * 10, 1);
        m_Camera.orthographicSize = GameData.SCREEN_HEIGHT / 200;
        SetUpPositionUI();

    }

    public void ChangeBG(int indexBg)
    {
        m_BackGround.sprite = SceneManager.instance.BackGroundController.BG[indexBg];
    }

    void SetupBackground()
    {

    }

    public void SettingScreen(GameData.eScreen screen)
    {
        if (screen == GameData.eScreen.Portrait)
        {

        }
        else
        {

        }
    }

    void SetUpPositionUI()
    {
        float temp = GameData.SCREEN_WIDTH / (100 * m_BackGround.bounds.size.x);
        temp *= 1.25f;
        if (temp > 1)
            m_BackGround.gameObject.transform.localScale = new Vector3(temp, temp, 1);
    }

    public void ChangeMode(Difficulty mode)
    {
        if(mode == Difficulty.Beginer)
        {

        }
        else if(mode == Difficulty.Intermediate)
        {

        }
        else if(mode == Difficulty.Advance)
        {

        }
    }

    void OnApplicationQuit()
    {
        //Debug.LogError(isPlayGame);
        //if (SceneManager.instance.PlayGameController.isPlayGame)
        //    Table.m_Instance.SaveGame();
    }

    //void OnDisable()
    //{
    //    if (SceneManager.instance.PlayGameController.isPlayGame)
    //        Table.m_Instance.SaveGame();
    //}
}
