using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    public bool IsLock;
    public int Level;
    public int Star;
    public int MapLevel;

    public List<Image> StarList;
    public List<Sprite> StarSprite = new List<Sprite>();
    public List<Sprite> Number = new List<Sprite>();
    public List<Image> LevelText = new List<Image>();
    public Image Main;
    public Sprite Block, UnBlock;
    private MapData mapData;
    public GameObject GiftBlock;
    public Image Gift, GiftBack, GiftFace, GiftBg;
    public Image StageText;

    public void InitData(MapData _mapData, int _mapLevel)
    {
        mapData = _mapData;
        Level = _mapLevel;
        Star = mapData.Star;
        IsLock = mapData.IsLock;
        Main.sprite = IsLock ? Block : UnBlock;
        if (Level % 5 == 4)
        {
            if (mapData.Score == 0 || IsLock)
            {
                Gift.color = new Color(1, 1, 1, 1);
                GiftBlock.SetActive(false);
            }
            else
            {
                GiftBlock.SetActive(true);
                GiftBg.color = new Color(1, 1, 1, 0);
                GiftFace.color = new Color(1, 1, 1, 0);
                GiftBack.color = new Color(1, 1, 1, 0);
                foreach (var item in SceneManager.instance.MapController.dataReward)
                {
                    if (item.Key == Level + 1)
                    {
                        if(item.Value.Count > 0)
                        {
                            if (item.Value.Count == 1)
                            {
                                if (item.Value[0] % 998 == 0)
                                {
                                    GiftFace.sprite = SceneManager.instance.CardFaceController.CardBack[item.Value[0] / 998];
                                }
                                else if (item.Value[0] % 999 == 0)
                                {
                                    GiftFace.sprite = SceneManager.instance.BackGroundController.BG[item.Value[0] / 999];
                                }
                                else if (item.Value[0] % 1000 == 0)
                                {
                                    GiftFace.sprite = SceneManager.instance.CardPackController.CardBack[item.Value[0] / 1000];
                                }
                                GiftFace.color = new Color(1, 1, 1, 1);
                            }
                            else if (item.Value.Count == 2)
                            {
                                if (item.Value[0] % 998 == 0)
                                {
                                    GiftBack.sprite = SceneManager.instance.CardFaceController.CardBack[item.Value[0] / 998];
                                }
                                else if (item.Value[0] % 999 == 0)
                                {
                                    GiftBack.sprite = SceneManager.instance.BackGroundController.BG[item.Value[0] / 999];
                                }
                                else if (item.Value[0] % 1000 == 0)
                                {
                                    GiftBack.sprite = SceneManager.instance.CardPackController.CardBack[item.Value[0] / 1000];
                                }

                                if (item.Value[1] % 998 == 0)
                                {
                                    GiftBg.sprite = SceneManager.instance.CardFaceController.CardBack[item.Value[1] / 998];
                                }
                                else if (item.Value[1] % 999 == 0)
                                {
                                    GiftBg.sprite = SceneManager.instance.BackGroundController.BG[item.Value[1] / 999];
                                }
                                else if (item.Value[1] % 1000 == 0)
                                {
                                    GiftBg.sprite = SceneManager.instance.CardPackController.CardBack[item.Value[1] / 1000];
                                }

                                GiftBg.color = new Color(1, 1, 1, 1);
                                GiftBack.color = new Color(1, 1, 1, 1);
                            }
                            else if (item.Value.Count == 3)
                            {
                                foreach (var reward in item.Value)
                                {
                                    if (reward % 998 == 0)
                                    {
                                        GiftFace.sprite = SceneManager.instance.CardFaceController.CardBack[reward / 998];
                                    }
                                    else if (reward % 999 == 0)
                                    {
                                        GiftBg.sprite = SceneManager.instance.BackGroundController.BG[reward / 999];
                                    }
                                    else if (reward % 1000 == 0)
                                    {
                                        GiftBack.sprite = SceneManager.instance.CardPackController.CardBack[reward / 1000];
                                    }
                                }

                                GiftBg.color = new Color(1, 1, 1, 1);
                                GiftFace.color = new Color(1, 1, 1, 1);
                                GiftBack.color = new Color(1, 1, 1, 1);
                            }
                        }
                        
                    }
                }
                Gift.color = new Color(1, 1, 1, 0);
            }
        }
        else 
        {
            GiftBlock.SetActive(false);
            Gift.color = new Color(1, 1, 1, 0);
        }

        foreach(var item in LevelText)
        {
            item.sprite = null;
        }

        if (!IsLock)
        {
            for (int i = 0; i < StarList.Count; i++)
            {
                StarList[i].color = new Color(1, 1, 1, 1);
                if (i < Star)
                {
                    StarList[i].sprite = StarSprite[i];
                }
                else
                {
                    StarList[i].sprite = StarSprite[i + 3];
                }
            }

            StageText.color = new Color(1, 1, 1, 1);
            var temp = (Level + 1).ToString();
            for (int i = 0; i < temp.Length; i++)
            {
                var ind = int.Parse(temp[i].ToString());
                LevelText[i].sprite = Number[ind];
                LevelText[i].SetNativeSize();
            }
        }
        else
        {
            StageText.color = new Color(1, 1, 1, 0);
            for (int i = 0; i < StarList.Count; i++)
            {
                StarList[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void OnThisLevelClick()
    {
        AudioController.instance.PlayButton();
        if (!IsLock)
        {

            SceneManager.instance.MapController.HideMap();
            SceneManager.instance.PlayGameController.ShowPlayGame();
            if (SceneManager.instance.CurrentLevel != Level)
            {
                SceneManager.instance.CurrentLevel = Level;
                SceneManager.instance.PlayGameController.NewGame();
            }
            SceneManager.instance.CurrentMapData = mapData;

            SceneManager.instance.MapController.isTouch = true;
        }
    }

    public void InitOnNextLevel()
    {
        //SceneManager.instance.CurrentLevel = Level;
        SceneManager.instance.CurrentMapData = mapData;
    }
}
