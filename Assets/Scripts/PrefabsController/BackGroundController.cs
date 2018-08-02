using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{

    public List<Sprite> BG;
    public GameObject Template_BG, ParentItemBG;
    public List<BackGroundValue> BGItemController;
    public SceneState Scene = SceneState.Main;
    public bool IsPlayGame = false;

    public CanvasGroup BackGround;
    private int PreBackGround = 0;
    public Sprite Lock;

    public void ShowBG(bool isplaygame = false)
    {
        //Debug.LogError(isplaygame);
        IsPlayGame = isplaygame;
        BackGround.alpha = 1;
        BackGround.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        CheckUnlock();
    }

    public void HideBG()
    {
        BackGround.alpha = 0;
        BackGround.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    void Start()
    {
        InitCardBack();
        PreBackGround = SceneManager.instance.GetBackGround();
        if (BGItemController.Count > PreBackGround)
        {
            BGItemController[PreBackGround].IsCheckedCard(true);
        }
    }

    void CheckUnlock()
    {
        int maxBack = SceneManager.instance.GetBGNum();
        for (int i = 0; i < BGItemController.Count; i++)
        {
            if (i > maxBack)
            {
                BGItemController[i].BG.sprite = Lock;
            }
            else
            {
                BGItemController[i].BG.sprite = BG[i];
            }
        }
    }

    public void OnBackClick()
    {

        AudioController.instance.PlayButton();
        HideBG();
        SceneManager.instance.MainSettingController.ShowMainSetting(IsPlayGame);
    }

    void InitCardBack()
    {
        for (int i = 0; i < BG.Count; i++)
        {
            var card = BG[i];
            var obj = GameObject.Instantiate(Template_BG) as GameObject;
            obj.transform.SetParent(ParentItemBG.transform);
            obj.transform.localScale = Vector2.one;
            obj.SetActive(true);
            var control = obj.GetComponent<BackGroundValue>();
            if (control != null)
            {
                control.IndexCard = i;
                control.BG.sprite = BG[i];
                BGItemController.Add(control);
            }
        }
    }

    public void OnCardBackClick(GameObject cardItem)
    {
        //Debug.LogError(IsPlayGame);
        AudioController.instance.PlayButton();
        var control = cardItem.GetComponent<BackGroundValue>();
        if (control != null && control.IndexCard <= SceneManager.instance.GetBGNum())
        {
            BGItemController[PreBackGround].IsCheckedCard(false);
            control.IsCheckedCard(true);
            PreBackGround = control.IndexCard;
            GameControl.Instance.SetBackGround(PreBackGround);
        }
    }
}
