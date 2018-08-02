using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFaceController : MonoBehaviour
{

    public List<Sprite> CardBack;
    public GameObject TemplateCardBack, ParentItemCardBack;
    public List<CardFaceValue> CardBackItemController;
    public SceneState Scene = SceneState.Main;
    public bool IsPlayGame = false;

    public CanvasGroup CardBackController;
    private int PreCardBack = 0;

    public List<Sprite> Face1 = new List<Sprite>();
    public List<Sprite> Face2 = new List<Sprite>();
    public List<Sprite> Face3 = new List<Sprite>();
    public List<Sprite> Face4 = new List<Sprite>();
    public List<Sprite> Face5 = new List<Sprite>();
    public List<Sprite> Face6 = new List<Sprite>();
    public List<Sprite> Face7 = new List<Sprite>();
    public List<Sprite> Face8 = new List<Sprite>();
    public List<Sprite> Face9 = new List<Sprite>();
    public List<Sprite> Face10 = new List<Sprite>();
    public List<Sprite> Face11 = new List<Sprite>();
    public List<Sprite> Face12 = new List<Sprite>();
    public List<Sprite> Face13 = new List<Sprite>();
    public List<Sprite> Face14 = new List<Sprite>();
    public List<Sprite> Face15 = new List<Sprite>();
    public List<Sprite> Face16 = new List<Sprite>();

    public Sprite Lock;

    public void ShowCardPack(bool isplaygame = false)
    {
        IsPlayGame = isplaygame;
        CardBackController.alpha = 1;
        CardBackController.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        CheckUnlock();
    }

    public void HideCardPack()
    {
        CardBackController.alpha = 0;
        CardBackController.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    void Start()
    {
        InitCardBack();
        PreCardBack = SceneManager.instance.GetCardFace();
        if (CardBackItemController.Count > PreCardBack)
        {
            CardBackItemController[PreCardBack].IsCheckedCard(true);
        }
    }

    //public void OnBackClick()
    //{
    //    AudioController.instance.PlayButton();
    //    HideCardPack();
    //    MainSceneController.Instance.MainSettingController.ShowMainSetting();
    //}

    public void OnBackClick()
    {
        AudioController.instance.PlayButton();
        HideCardPack();
        SceneManager.instance.MainSettingController.ShowMainSetting(IsPlayGame);

    }

    void InitCardBack()
    {
        for (int i = 0; i < CardBack.Count; i++)
        {
            var card = CardBack[i];
            var obj = GameObject.Instantiate(TemplateCardBack) as GameObject;
            obj.transform.SetParent(ParentItemCardBack.transform);
            obj.transform.localScale = Vector2.one;
            obj.SetActive(true);
            var control = obj.GetComponent<CardFaceValue>();
            if (control != null)
            {
                control.IndexCard = i;
                control.CardBack.sprite = CardBack[i];
                CardBackItemController.Add(control);
            }
        }
    }

    void CheckUnlock()
    {
        int maxBack = SceneManager.instance.GetCardFaceNum();
        for (int i = 0; i < CardBackItemController.Count; i++)
        {
            if (i > maxBack)
            {
                CardBackItemController[i].CardBack.sprite = Lock;
            }
            else
            {
                CardBackItemController[i].CardBack.sprite = CardBack[i];
            }
        }
    }

    public void OnCardBackClick(GameObject cardItem)
    {
        AudioController.instance.PlayButton();
        var control = cardItem.GetComponent<CardFaceValue>();
        if (control != null && control.IndexCard <= SceneManager.instance.GetCardFaceNum())
        {
            CardBackItemController[PreCardBack].IsCheckedCard(false);
            control.IsCheckedCard(true);
            PreCardBack = control.IndexCard;
            GameControl.Instance.SetCardFace(PreCardBack);
            Table.m_Instance.ChangeCardFace(PreCardBack);
        }
    }

    public List<Sprite> GetCurrentCardFace()
    {
        int index = GameControl.Instance.GetCardFace();
        if (index == 0)
        {
            return Face1;
        }
        else if (index == 1)
        {
            return Face2;
        }
        else if (index == 2)
        {
            return Face3;
        }
        else if (index == 3)
        {
            return Face4;
        }
        else if (index == 4)
        {
            return Face5;
        }
        else if (index == 5)
        {
            return Face6;
        }
        else if (index == 6)
        {
            return Face7;
        }
        else if (index == 7)
        {
            return Face8;
        }
        else if (index == 8)
        {
            return Face9;
        }
        else if (index == 9)
        {
            return Face10;
        }
        else if (index == 10)
        {
            return Face11;
        }
        else if (index == 11)
        {
            return Face12;
        }
        else if (index == 12)
        {
            return Face13;
        }
        else if (index == 13)
        {
            return Face14;
        }
        else if (index == 14)
        {
            return Face15;
        }
        else
        {
            return Face16;
        }
    }
}
