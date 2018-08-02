using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour{

    public CanvasGroup Map;
    public MapBoxController MapBox1, MapBox2;
    public GameObject[] MiniMap;
    public float TimeTween = 0.2f;
    public int _currentLevel = 0;
    public Vector3 positon;
    public int OffSet = 3;
    public bool isTouch = true;
    bool _isTouch = false;
    Vector3 Center;
    Vector3 Right;
    public Dictionary<int, List<int>> dataReward = new Dictionary<int, List<int>>();

    void Awake()
    {
        Center = MapBox1.transform.localPosition;
        Right = MapBox2.transform.localPosition;
    }

    public void ShowMap()
    {
        Map.alpha = 1;
        Map.blocksRaycasts = true;
        gameObject.transform.localPosition = Vector2.zero;
        InitData();
        ShowMiniMap();
        LeanTween.delayedCall(0.5f, () => { _isTouch = true; });
    }

    public void InitData()
    {
        dataReward.Clear();
        var data = PlayerPrefs.GetString("REWARD_DATA");
        if (!string.IsNullOrEmpty(data))
        {
            var temp = data.Split(';');
            foreach (var item in temp)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var temp2 = item.Split('-');
                    List<int> reward = new List<int>();
                    for (int i = 1; i < temp2.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(temp2[i]))
                        {
                            reward.Add(int.Parse(temp2[i]));
                        }
                    }
                    dataReward.Add(int.Parse(temp2[0]), reward);
                }
            }
        }

        int currentLevel = CheckCurrentMap();
        //Debug.Log(currentLevel);
        //Debug.Log(SceneManager.instance.AllMapData.Count);
        _currentLevel = currentLevel;
        MapBox1.InitData(currentLevel, SceneManager.instance.AllMapData[currentLevel]);
        if (currentLevel < SceneManager.instance.AllMapData.Count)
        {
            MapBox2.InitData(currentLevel + 1, SceneManager.instance.AllMapData[currentLevel + 1]);
        }
        else if(currentLevel > 1)
        {
            MapBox2.InitData(15, SceneManager.instance.AllMapData[15]);
        }
        MapBox1.transform.localPosition = Center;
        MapBox2.transform.localPosition = Right;
        SceneManager.instance.CurrentMap = _currentLevel;
    }

    public int CurrentLevelUnlock = 0;
    public int CheckCurrentMap()
    {
        CurrentLevelUnlock = SceneManager.instance.LevelPerMap - 1;
        foreach (var map in SceneManager.instance.AllMapData)
        {
            for(int i = 0; i < map.Value.Count;i++)
            {
                if (map.Value[i].Score == 0)
                {
                    CurrentLevelUnlock = i;
                    return map.Key;
                }
            }
        }
        return SceneManager.instance.AllMapData.Count - 1;
    }

    public void HideMap()
    {
        Map.alpha = 0;
        Map.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
        _isTouch = false;
        isTouch = true;
    }

    private bool isShowMap1 = true;
    private bool isBlockClick = false;
    private bool _isLeft;
    public void OnChangeMapClick(bool isRight)
    {
        if (!isBlockClick)
        {
            _isLeft = !isRight;
            isBlockClick = true;
            if (isShowMap1)
            {
                if (isRight)
                {
                    _currentLevel++;
                    MapBox2.GetComponent<RectTransform>().localPosition = Right;
                    LeanTween.moveLocalX(MapBox1.gameObject, -800f, TimeTween).setOnComplete(OnMoveComplete);
                }
                else
                {
                    _currentLevel--;
                    MapBox2.GetComponent<RectTransform>().localPosition = new Vector3(-Right.x, Right.y);
                    LeanTween.moveLocalX(MapBox1.gameObject, 800f, TimeTween).setOnComplete(OnMoveComplete);
                }
                MapBox2.InitData(_currentLevel, SceneManager.instance.AllMapData[_currentLevel]);
                LeanTween.moveLocalX(MapBox2.gameObject, 0, TimeTween).setOnComplete(OnMoveComplete);
                AudioController.instance.PlaySoundSortCard();
            }
            else
            {
                if (isRight)
                {
                    _currentLevel++;
                    MapBox1.GetComponent<RectTransform>().localPosition = Right;
                    LeanTween.moveLocalX(MapBox2.gameObject, -800f, TimeTween).setOnComplete(OnMoveComplete);
                }
                else
                {
                    _currentLevel--;
                    MapBox1.GetComponent<RectTransform>().localPosition = new Vector3(-Right.x, Right.y);
                    LeanTween.moveLocalX(MapBox2.gameObject, 800f, TimeTween).setOnComplete(OnMoveComplete);
                }
                MapBox1.InitData(_currentLevel, SceneManager.instance.AllMapData[_currentLevel]);
                LeanTween.moveLocalX(MapBox1.gameObject, 0, TimeTween).setOnComplete(OnMoveComplete);
                AudioController.instance.PlaySoundSortCard();
            }
            SceneManager.instance.CurrentMap = _currentLevel;
           // Debug.Log(_currentLevel);
            isShowMap1 = !isShowMap1;
            ShowMiniMap();
        }
    }

    private void OnMoveComplete()
    {
        isBlockClick = false;
        _isTouch = true;
    }

    public void NextLevel()
    {
        if(isShowMap1)
        {
            MapBox1.NextLevel();
        }
        else
        {
            MapBox2.NextLevel();
        }
    }

   public  void ShowMiniMap()
    {
        int count = MiniMap.Length;
        for(int i = 0; i < count; i++)
        {
            MiniMap[i].SetActive(false);
        }
        MiniMap[_currentLevel -1].SetActive(true);
    }

    public void OnBackClick()
   {
       AudioController.instance.PlayButton();
       SceneManager.instance.OnButtonBackNativeClick();
   }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentLevel > 1)
                OnChangeMapClick(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentLevel < SceneManager.instance.MapNumber)
                OnChangeMapClick(true);
        }
        if(_isTouch)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch(touch.phase)
                {
                    case TouchPhase.Began:
                        positon = touch.position;
                        break;

                    case TouchPhase.Moved:
                        break;

                    case TouchPhase.Ended:
                        if (!isTouch)
                            return;
                        else
                        {  
                            if (touch.position.x >= positon.x + OffSet)
                            {

                                if (_currentLevel > 1)
                                {
                                    _isTouch = false;
                                    OnChangeMapClick(false);
                                }
                            }
                            else if (touch.position.x < positon.x - OffSet)
                            {

                                if (_currentLevel < SceneManager.instance.MapNumber)
                                {
                                    _isTouch = false;
                                    OnChangeMapClick(true);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
