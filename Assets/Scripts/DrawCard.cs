using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
public class DrawCard : MonoBehaviour
{

    public Sprite[] m_Background;
    public BoxCollider2D m_Box;
    SpriteRenderer m_SpriteRender;
    public bool isFirstDrawCard = true;

    [Header("Border")]
    public GameObject m_Border;
    private List<int> ListCardResult = new List<int>();
    private List<int> ListCardDrawed = new List<int>();
    private Vector2 OriginPosition;
    private Vector2 DrawPosition;
    [Header("CardBack")]
    public Sprite[] CardBacks;
    private int CardBackIndex =0;
    private UnityAction ResetCardDrawDone;

    void Awake()
    {
        m_SpriteRender = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
        transform.localScale = new Vector3(GameData.CARD_SCALE, GameData.CARD_SCALE / 9 * 10, 1);
        Vector3 pos = Compute.GetInstance().GetPositionOrigin();
        pos.x -= GameData.CARD_WIDTH/2;
        pos.z = -(GameData.NUMBER_CARD + 1);
        transform.position = pos;
        OriginPosition = Compute.m_Instance.GetPositionOrigin();
        DrawPosition = Compute.m_Instance.GetPositionDraw();
        m_Box = GetComponent<BoxCollider2D>();
        ResetCardDrawDone += OnResetCardComplete;
        //DrawDone += HandleOnDrawDone();
    }

    private void OnResetCardComplete()
    {
        m_Box.enabled = false;
        LeanTween.delayedCall(0.4f, () => { m_Box.enabled = true; });
    }

    void OnMouseDown()
    {
        Table.GetInstance().DrawCard();
    }

    public void ResetCardDraw()
    {
        m_SpriteRender.sprite = CardBacks[CardBackIndex];
    }

    private void HandleOnDrawDone()
    {

    }

    public void ShowHintDraw()
    {
        m_Border.SetActive(true);
    }

    public void TurnOffShowHint()
    {
        m_Border.SetActive(false);
    }

    public void EndDraw()
    {
        m_SpriteRender.sprite = m_Background[1];
        m_SpriteRender.color = new Color(1, 1, 1, 0.2f);
    }

    public void InitCardDraw(List<int> list)
    {
        //ListCardDraw.Clear();
        ListCardResult.Clear();
        ListCardDrawed.Clear();
        //ListCardDraw.AddRange(list);
        ListCardResult.AddRange(list);
    }

    public void RemoveCard(Card[] Cards, int value)
    {
        ListCardDrawed.Remove(value);
        //DebugListCardDrawEd();
        //RemoveCardInList(ListCardResult, value);
        SetUpCardDraw(Cards);
        SortCardDraw(Cards);
    }

    public void Draw(GameData.eModeDraw mode, Card[] Cards)
    {
 
        int count = ListCardResult.Count;
        m_Box.enabled = false;
        LeanTween.delayedCall(0.4f, () => { m_Box.enabled = true; });
        if (count == 0)
        {
            Compute.m_Instance.AddUndo(-1, GameData.TOTAL_COLLUM, GameData.TOTAL_COLLUM, -1);
            //DebugListCardDrawEd();
            Reset();
            return;
        }
        if (mode == GameData.eModeDraw.OneCard)
        {
            //Debug.Log("xxx");

            int _last = count - 1;
            if (_last >= 0)
            {
                m_Box.enabled = false;
                SceneManager.instance.PlayGameController.MoveCount += 1;
                Vector3 temp = DrawPosition;
                Card _card = Cards[ListCardResult[_last]];

                int value = ListCardResult[_last];
                temp.z = -(GameData.NUMBER_CARDDRAW - _last);
                if (!_card.isFlip)
                    _card.isFlip = false;
                LeanTween.move(_card.gameObject, temp, 0.2f).
                    setEaseInCubic().setOnComplete(() => {
                        AnimationCard(Cards, value, true); 
                    });
                AudioController.instance.PlaySoundSortCard();
                ListCardDrawed.Add(value);
                ListCardResult.RemoveAt(_last);
                //Debug.Log(Cards[ListCardDrawed[_count]].transform.position);
                Compute.m_Instance.AddUndo(_card.Index, GameData.TOTAL_COLLUM, GameData.TOTAL_COLLUM, value);
                int _count = ListCardDrawed.Count - 1;
                //Debug.Log(ListCardDrawed[_count]);
                //Cards[ListCardDrawed[_count]].Index = DrawCount -1;
                SortCardDraw(Cards);
                SetUpCardDraw(Cards);
                if (count == 1)
                {
                    EndDraw();
                }

            }
        }
        else
        {
            //Debug.Log("cccc");

            int _last = count - 1;
            if (_last >= 2)
            {
                m_Box.enabled = false;
                Vector3 temp = DrawPosition;
                List<int> draw = new List<int>();
                for (int i = 0; i < 3; i++)
                {

                    int LastResult = ListCardResult.Count - 1;
                    Card _card = Cards[ListCardResult[LastResult]];
                    int value = ListCardResult[LastResult];
                    temp.z = -(GameData.NUMBER_CARDDRAW - LastResult);
                    if (_card.isFlip)
                        _card.isFlip = false;
                    _card.isFlip = !_card.isFlip;
                    if (i == 2)
                        LeanTween.move(_card.gameObject, temp, 0.1f).setEaseInCubic();
                    else
                        LeanTween.move(_card.gameObject, temp, 0.1f).setEaseInCubic();
                    AudioController.instance.PlaySoundSortCard();
                    ListCardDrawed.Add(value);
                    draw.Add(value);
                    ListCardResult.RemoveAt(LastResult);
                    int _count = ListCardDrawed.Count - 1;
                    //Cards[ListCardDrawed[_count]].Index = DrawCount -1;
                    SortCardDraw(Cards);
                    SetUpCardDraw(Cards);
                    if (LastResult == 0)
                    {
                        EndDraw();
                    }

                }
                int _temp = ListCardResult.Count + 2;
                //Compute.m_Instance.AddUndo(_temp, GameData.TOTAL_COLLUM, GameData.TOTAL_COLLUM, draw);
                SceneManager.instance.PlayGameController.MoveCount += 1;

            }
            else if (_last >= 0)
            {
                m_Box.enabled = false;
                Vector3 temp = DrawPosition;
                SceneManager.instance.PlayGameController.MoveCount += 1;
                Card _card = Cards[ListCardResult[_last]];
                int value = ListCardResult[_last];
                if (_card.isFlip)
                    _card.isFlip = false;
                temp.z = temp.z = -(GameData.NUMBER_CARDDRAW - _last);
                LeanTween.move(_card.gameObject, temp, 0.2f)
                    .setEaseInCubic().setOnComplete(() => {
                        AnimationCard(Cards, value, true);
                       
                    }
                        );
                ListCardDrawed.Add(value);
                ListCardResult.RemoveAt(_last);
                Compute.m_Instance.AddUndo(_card.Index, GameData.TOTAL_COLLUM, GameData.TOTAL_COLLUM, value);
                int _count = ListCardDrawed.Count - 1;
                //Cards[ListCardDrawed[_count]].Index = DrawCount - 1;
                SortCardDraw(Cards);
                SetUpCardDraw(Cards);
                if (_last == 0)
                {
                    EndDraw();
                }
            }
        }
#if UNITY_EDITOR
        DebugListCard();
#endif
      
    }

    public void SetUpCardDraw(Card[] Cards)
    {
        int count = ListCardDrawed.Count;
        if (count > 1)
        {
            Cards[ListCardDrawed[count - 2]].TurnInteractable(false);
            int temp = ListCardDrawed[count - 1];
            Cards[temp].TurnInteractable(true);

        }
        else if (count > 0)
        {
            int temp = ListCardDrawed[count - 1];
            Cards[temp].TurnInteractable(true);
        }
    }

    public void SortCardDraw(Card[] Cards)
    {
        //Debug.Log("SortCardDraw");
        int count = ListCardDrawed.Count - 1;
        if ((count <= 2) && (count > 0))
        {
            int _count = ListCardDrawed.Count;
            for (int i = _count - 1; i >= 0; i--)
            {
                int temp = ListCardDrawed[count - i];
                Vector3 pos = DrawPosition;
                pos.x -= i * GameData.CARDDRAW_RATIO;
                pos.z = -(count - i);
                LeanTween.move(Cards[temp].gameObject, pos, 0.2f);
            }
        }
        else if (count > 2)
        {
            for (int i = 0; i < GameData.LIMIT_CARDDRAWSHOW; i++)
            {
                int temp = ListCardDrawed[count - i];
                Vector3 pos = DrawPosition;
                pos.x -= i * GameData.CARDDRAW_RATIO;
                pos.z = -(count - i);
                LeanTween.move(Cards[temp].gameObject, pos, 0.2f);
            }
            for (int i = GameData.LIMIT_CARDDRAWSHOW; i <= count; i++)
            {
                int temp = ListCardDrawed[count - i];
                Vector3 pos = DrawPosition;
                pos.x -= 2 * GameData.CARDDRAW_RATIO;
                pos.z = -(count - i);
                LeanTween.move(Cards[temp].gameObject, pos, 0.2f);
            }

        }
        else if (count == 0)
        {
            int temp = ListCardDrawed[0];
            Vector3 pos = DrawPosition;
            pos.z = -Cards[temp].Index;
            LeanTween.move(Cards[temp].gameObject, pos, 0.2f);
        }
    }

    private void AnimationCard(Card[] Cards, int value, bool isDraw = false)
    {
        //Debug.Log(value);
        LeanTween.scaleX(Cards[value].gameObject, 0, 0.05f).setOnComplete(() => 
                FlipCard(Cards, value,isDraw)
            );
    }

    void FlipCard(Card[] Cards, int value, bool isDraw = false)
    {
        Cards[value].isFlip = !Cards[value].isFlip;
        LeanTween.scaleX(Cards[value].gameObject, GameData.CARD_SCALE, 0.05f);
    }

    public void Reset()
    {
#if UNITY_EDITOR
        DebugListCard();
#endif
        ResetCardDraw();
        m_Box.enabled = false;
        ListCardResult.Clear();
        ListCardResult.AddRange(ListCardDrawed);
        ListCardResult.Reverse();
        ListCardDrawed.Clear();
        //ResetCardDrawDone();
        Table.m_Instance.ResetCard(ListCardResult);
    }

    void DebugListCardDrawEd()
    {
        int _count = ListCardDrawed.Count;
        string abc = "";
        for (int i = 0; i < _count; i++)
        {
            abc += (ListCardDrawed[i] / 4 + 1).ToString() + " ";

        }
        //Debug.Log(DrawCount);
        Debug.Log(abc);
    }

    public void SortListCardDraw(Card[] Cards)
    {
        SortCardDraw(Cards);
        SetUpCardDraw(Cards);
    }

    public void Undo(Card[] Cards, Undo undo)
    {

        int count = undo.ListIndex.Count;

        if (undo.Index == -1)
        {
            ListCardDrawed.AddRange(ListCardResult);
            ListCardDrawed.Reverse();
            ListCardResult.Clear();
            int _count = ListCardDrawed.Count;
            for (int i = 0; i < _count; i++)
            {
                Cards[ListCardDrawed[i]].Open();
            }
            SortCardDraw(Cards);
            SetUpCardDraw(Cards);
            return;
        }
        if (undo.CollumDes == GameData.TOTAL_COLLUM)
        {
            if (GameData.MODEDRAW == GameData.eModeDraw.OneCard)
            {
                //Debug.Log("xxxx");
                //if (ListCardDrawed.Count == 0)
                //    return;
                for (int i = 0; i < count; i++)
                {
                    Vector3 pos = OriginPosition;
                    int temp = undo.ListIndex[i];
                    if (i == count - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW)
                            .setOnComplete(() => Cards[temp].Default());
                    else
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    int _count = ListCardDrawed.Count - 1;
                    //Debug.Log(_count + "," + ListCardDrawed.Count);
                    ListCardResult.Add(ListCardDrawed[_count]);
                    ListCardDrawed.RemoveAt(_count);
                    SortCardDraw(Cards);
                    SetUpCardDraw(Cards);
                }
            }
            else
            {
                //Debug.Log(undo.ListCard.Count);
                for (int i = 0; i < count; i++)
                {
                    Vector3 pos = OriginPosition;
                    int temp = undo.ListIndex[i];
                    if (i == count - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW)
                            .setOnComplete(() => SetListCardDrawUndoAttri(Cards, undo));
                    else
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    int _count = ListCardDrawed.Count - 1;
                    //Debug.Log(_count);
                    ListCardResult.Add(ListCardDrawed[_count]);
                    ListCardDrawed.RemoveAt(_count);
                }
                SortCardDraw(Cards);
                SetUpCardDraw(Cards);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = DrawPosition;
                pos.z = -undo.Index;

                int temp = undo.ListIndex[i];
                Cards[temp].Collum = undo.Collum;
                Cards[temp].Index = undo.Index;
                LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                int _count = ListCardDrawed.Count - 1;
                ListCardDrawed.Add(temp);
                SortCardDraw(Cards);
                SetUpCardDraw(Cards);
            }
        }
        //DebugListCard();
        // DebugUndo(undo);
    }

    private void SetListCardDrawUndoAttri(Card[] Cards, Undo undo)
    {
        int count = undo.ListIndex.Count;

        for (int i = 0; i < count; i++)
        {
            int temp = undo.ListIndex[i];
            Cards[temp].Default();
        }
    }

    public void EnableDraw(bool isDraw = true)
    {
        m_Box.enabled = isDraw;
    }

    public List<int> GetListCardDraw()
    {
        List<int> temp = ListCardDrawed;
        temp.AddRange(ListCardResult);
        return temp;
    }

    public List<int> GetListCardDrawed()
    {
        return ListCardDrawed;
    }

    void DebugUndo(Undo undo)
    {
        // Debug.Log("DrawCount " + DrawCount);
        Debug.Log("ListCardDrawed " + ListCardDrawed.Count);
        Debug.Log("ListCardResult " + ListCardResult.Count);
        Debug.Log("Number Card Undo" + undo.ListIndex.Count);
        Debug.Log("Card Undo Index" + undo.Index);
        Debug.Log("Card Undo Collum" + undo.Collum);
        Debug.Log("Card Undo Collum Des" + undo.CollumDes);
        Debug.Log("List Card Undo" + undo.ListIndex);
    }

    void DebugListCard()
    {
        int result = ListCardResult.Count;
        string s = "Result " + result + "\n ";
        for (int i = 0; i < result; i++)
        {
            s += (ListCardResult[i] / 4 + 1) + " ";
        }
        Debug.LogError(s);
        int drawed = ListCardDrawed.Count;
        string ss = "Drawed " + drawed + "\n ";
        for (int i = 0; i < drawed; i++)
        {
            ss += (ListCardDrawed[i] / 4 + 1) + " ";
        }
        Debug.LogError(ss);
    }

    public List<int> GetListCardResult()
    {
        return ListCardResult;
    }

    public int GetLastCardDrawed()
    {
        int count = ListCardDrawed.Count - 1;
        if (count >= 0)
            return ListCardDrawed[count];
        else return -1;
    }

    public void SetCardBack(int index)
    {
        CardBackIndex = index;
        m_SpriteRender.sprite = SceneManager.instance.CardPackController.CardBack[CardBackIndex];
        //ResetCardDrawDone();
    }

    public void ShowCardEnable(List<int> enable, List<int> disable)
    {

    }

}
