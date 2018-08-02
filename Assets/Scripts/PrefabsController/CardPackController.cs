using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPackController : MonoBehaviour
{
    public List<Sprite> CardBack;
    public GameObject TemplateCardBack, ParentItemCardBack;
    public List<CardBackValue> CardBackItemController;
    public SceneState Scene;
    public bool IsPlayGame = false;

    public CanvasGroup CardBackController;
    private int PreCardBack = 0;
    public Sprite Lock;

    public void ShowCardPack(bool isplaygame = false)
    {
        //Debug.LogError(isplaygame);
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
        PreCardBack = SceneManager.instance.GetCardBack();
        if (CardBackItemController.Count > PreCardBack)
        {
            CardBackItemController[PreCardBack].IsCheckedCard(true);
            //Table.m_Instance.cardBackIndex = PreCardBack;
        }
    }

    void CheckUnlock()
    {
        int maxBack = SceneManager.instance.GetCardPackNum();
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

    //public void OnBackClick()
    //{
    //    AudioController.instance.PlayButton();
    //    HideCardPack();
    //    bool playgame = SceneManager.instance.SettingController.IsPlayGame;
    //    SceneManager.instance.SettingController.ShowSetting(playgame);
    //}

    public void OnBackClick()
    {
        AudioController.instance.PlayButton();
        HideCardPack();
        //if (!IsPlayGame)
        SceneManager.instance.MainSettingController.ShowMainSetting(IsPlayGame);
        //else
        //    SceneManager.instance.PlayGameController.ShowPlayGame();

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
            var control = obj.GetComponent<CardBackValue>();
            if (control != null)
            {
                control.IndexCard = i;
                control.CardBack.sprite = CardBack[i];
                CardBackItemController.Add(control);
            }
        }
    }

    public void OnCardBackClick(GameObject cardItem)
    {
        AudioController.instance.PlayButton();
        var control = cardItem.GetComponent<CardBackValue>();
        if (control != null && control.IndexCard <= SceneManager.instance.GetCardPackNum())
        {
            CardBackItemController[PreCardBack].IsCheckedCard(false);
            control.IsCheckedCard(true);
            PreCardBack = control.IndexCard;
            GameControl.Instance.SetCardBack(PreCardBack);
            Table.m_Instance.ChangeCardBack(PreCardBack);
        }
    }
}
