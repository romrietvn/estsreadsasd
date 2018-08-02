using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Events;
using System.IO;

public class Table : MonoBehaviour
{
    #region SingleTon
    public static Table m_Instance;
    public static Table GetInstance()
    {
        return m_Instance;
    }
    #endregion

    public Card CardPrefab;
    public DrawCard m_CardDraw;
    public BackGround m_BG;
    public static List<int> Result = new List<int>();
    private int[] CardValues = new int[GameData.NUMBER_CARD];
    private Vector2[] PositionCollum;
    private Vector2 PositionOrigin;
    private Card[] Cards = new Card[GameData.NUMBER_CARD];
    private BackGround[] BG = new BackGround[GameData.TOTAL_COLLUM];
    private bool isDrag = false;
    private List<int> m_Hint = new List<int>();
    private event Action Chiabaixong;
    private int mode = 0;
    private List<int> ListCardFlip = new List<int>();
    private List<int> ListBgHint = new List<int>();
    public bool isShowAnimation = false;
    public bool ChiaBai = false;
    public int cardBackIndex = 0;
    private Card cardDown;
    int CountDrawCards = 0;
    public int NumberWinSuit = 0;
    int Mode = 1;
    public int NumberComplete = 0;
    int Totalcards = 0;
    //private string m_Data = "";
    void Awake()
    {
        m_Instance = this;

    }

    void Start()
    {
        //Vector3 posDraw = Cards[GameData.NUMBER_CARDSDRAW + CountDrawCards * GameData.NUMBER_COLLUM].transform.position;
        //posDraw.z = -GameData.DEPT;
        //m_CardDraw.transform.position = posDraw;
        InitTable();
        SceneManager.instance.PlayGameController.MoveCount = 0;
        Chiabaixong += HandleOndone;


        //string s = "";
        //for(int i = 0; i < Cards.Length; i++)
        //{
        //    s += Cards[i].Value + " ";
        //}
        //Debug.LogError(s);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AnimationWin();
        }
    }

    void FixedUpdate()
    {

    }

    void Restart(int numbercomplete = 0, int totalcards = 0)
    {
        ShufferCards(numbercomplete);
        Compute.GetInstance().InitData(CardValues, numbercomplete, totalcards);
        Vector3 posDraw = PositionOrigin;
        posDraw.x -= CountDrawCards * GameData.CARD_RATIO_WIDTH;
        posDraw.z = -GameData.DEPT;
        m_CardDraw.transform.position = posDraw;
    }

    public void InitTable()
    {
        //Debug.Log("xaxa");
        CloneCard();
        InitPositionInTable();

        CardsController _control = new CardsController(CardValues, GameData.MODEDRAW);
        Restart();
        int NumberCard = GameData.NUMBER_CARD;
        for (int i = 0; i < NumberCard; i++)
        {
            Cards[i].Value = CardValues[i];
            Cards[i].transform.position = PositionOrigin;
            Cards[i].IndexArray = i;
            Cards[i].isFlip = false;
            //Cards[i].ShowEnableClick(false);
        }
        ChangeCardBack(cardBackIndex);
        //m_CardDraw.EnableDraw();
        //SaveGame save = new SaveGame(Compute.m_Instance.GetAllCollums(), Compute.m_Instance.GetArrayCardFlip(), m_CardDraw.GetListCardDraw());
        //string s = JsonUtility.ToJson(save);
        //Debug.Log(s);
        //string path = Application.persistentDataPath;
        //Debug.Log(path);
        //System.IO.File.WriteAllText( path + "/SaveGame.txt", s);
    }

    void InitPositionInTable()
    {
        PositionCollum = Compute.GetInstance().GetPositionAllCollum();
        PositionOrigin = Compute.GetInstance().GetPositionOrigin();
        ClonneBG();
    }

    void ShufferCards(int numbercomplete = 0)
    {
        if (numbercomplete == 0)
        {
            CardsController _control = new CardsController(CardValues, GameData.MODEDRAW);
            //if (Mode != SceneManager.Mode)
            //{
            //    CardsController _control = new CardsController(CardValues, SceneManager.Mode);
            //    //Mode = SceneManager.Mode;
            //}
            //else
            //{
            //    CardsController _control = new CardsController(CardValues, SceneManager.Mode);
            //}
        }
        else
        {
            CardsController _control = new CardsController(CardValues, GameData.MODEDRAW, numbercomplete);
        }
        for (int i = 0; i < GameData.NUMBER_CARD; i++)
        {
            Cards[i].Value = CardValues[i];
        }
        //SaveGame();
    }

    public void DragCard(Vector2 position)
    {

        int count = Result.Count;
        if (count == 0)
            return;
        if (!isDrag)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 temp = Cards[Result[i]].transform.position;
                temp.x = position.x;
                temp.y = position.y - (i) * GameData.CARD_RATIO_HEIGHT;
                temp.z = -GameData.DEPT - i;
                Cards[Result[i]].transform.position = temp;
                //Debug.Log(temp + "Mouse " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            isDrag = true;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 temp = position;
                temp.y = position.y - (i) * GameData.CARD_RATIO_HEIGHT;
                temp.z = Cards[Result[i]].transform.position.z;
                Cards[Result[i]].transform.position = temp;
            }
        }

    }

    private void CloneCard()
    {
        int numberCard = GameData.NUMBER_CARD;
        int cardFace = GameControl.Instance.GetCardFace();
        for (int i = 0; i < numberCard; i++)
        {
            Cards[i] = Instantiate<Card>(CardPrefab);
            Cards[i].m_IndexCardFace = cardFace;
            Cards[i].transform.localScale = new Vector3(GameData.CARD_SCALE, GameData.CARD_SCALE / 9 * 10, 1);
            Cards[i].transform.parent = this.transform;
        }

    }

    private void ClonneBG()
    {

        for (int i = 0; i < GameData.TOTAL_COLLUM; i++)
        {
            BG[i] = Instantiate<BackGround>(m_BG);
            Vector3 pos = PositionCollum[i];
            pos.z = 1;
            BG[i].transform.position = pos;
            BG[i].transform.parent = this.transform;
        }
    }

    private void ResetResult()
    {
        Result.Clear();
    }

    public void Handle_OnCardEndDrag(Vector3 position, int collum, int index, int cardflip, int value, UnityAction ondone) // true
    {

        MoveCardResultToCollum(position, collum, index, cardflip, value, ondone);

    }

    public void Handle_OnCardEndDrag(Vector3 position, int index, int collum)
    {
        if (collum < GameData.TOTAL_COLLUM)
        {
            int count = Result.Count;
            //Debug.LogError(count);
            Vector3 temp = position;
            for (int i = 0; i < count; i++)
            {
                temp.y = position.y - (i) * GameData.CARD_RATIO_HEIGHT;
                temp.z = -(index + i);
                Cards[Result[i]].transform.position = temp;
                Cards[Result[i]].Index = index + i;
                AudioController.instance.PlaySoundWrong();
                //Debug.Log(position + " " + index);
            }

        }
        else
        {
            int count = Result.Count;
            //Debug.Log(count);
            Vector3 temp = position;
            for (int i = 0; i < count; i++)
            {
                temp.y = position.y - (i) * GameData.CARD_RATIO_HEIGHT;
                temp.z = -(index + i);
                //position.z = -(index + i);
                Cards[Result[i]].transform.position = temp;
                AudioController.instance.PlaySoundWrong();
                //Debug.Log(position);
            }

        }
        EnableClick();
        ResetResult();
        SaveGame();
        isDrag = false;
    }

    public void DrawCard(bool isNormal = true)
    {
        HideHint();
        // Debug.LogError(CountDrawCards);
        if (CountDrawCards > GameData.MAXDRAWS)
            return;
        SceneManager.instance.PlayGameController.ActiveUI(false);
        BlockClick();
        m_CardDraw.m_Box.enabled = false;
        int currentindex = GameData.NUMBER_CARDSDRAW + CountDrawCards * GameData.NUMBER_COLLUM;
        List<int> lenght = Compute.m_Instance.GetLenghAllCollums();
        List<int> _listCards = new List<int>();
        List<int> _listvalues = new List<int>();
        var index2 = 0;
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {

            Card _card = Cards[currentindex + i];
            _card.m_Box.enabled = false;
            _listCards.Add(currentindex + i);
            _listvalues.Add(CardValues[currentindex + i]);
            _card.Collum = i;
            _card.isFlip = true;
            if (!isNormal)
            {
                _card.ShowHint();
            }
            int index = Compute.m_Instance.GetLenghtCollum(i);
            Vector3 pos = Compute.m_Instance.PosDesDraw(i);
            // pos.y = pos.y -  GameData.CARD_RATIO_HEIGHT;
            pos.z = -50;
            //LeanTween.move(Cards[80 + i].gameObject, Vector3.zero, 0.5f + i*0.01f);
            AudioController.instance.PlaySoundSortCard();
            if (i != GameData.NUMBER_COLLUM - 1)
            {
                var timeMove = isNormal ? GameData.TIME_MOVEDRAW + i * 0.03f : GameData.TIME_MOVEDRAW + i * 0.04f;

                LeanTween.move(_card.gameObject, pos, GameData.TIME_MOVEDRAW + timeMove).setOnComplete(() =>
                {
                    _card.HideHint();
                    var time = index2 == 0 ? 0.3f : 0.2f;
                    //LeanTween.delayedCall(time, () =>
                    //{
                    //    _card.Setposition();
                    //});
                    index2++;
                });
            }
            else
            {
                var timeMove = isNormal ? GameData.TIME_MOVEDRAW + i * 0.03f : GameData.TIME_MOVEDRAW + i * 0.04f;

                LeanTween.move(_card.gameObject, pos, timeMove).setOnComplete(() =>
                    {
                        Compute.m_Instance.DrawCards(CardValues, CountDrawCards);
                        Compute.m_Instance.AddUndo(currentindex, GameData.NUMBER_COLLUM, GameData.NUMBER_COLLUM, _listCards, _listvalues);
                        for (int j = 0; j < GameData.NUMBER_COLLUM; j++)
                        {
                            Cards[currentindex + j].Index = lenght[j];
                            Cards[currentindex + j].m_Box.enabled = true;
                            if (isNormal)
                                Compute.m_Instance.ShowCardEnable(j);
                        }

                        CountDrawCards += 1;
                        //Debug.LogError(CountDrawCards);
                        if (CountDrawCards != 5)
                        {
                            Vector3 posDraw = PositionOrigin;
                            posDraw.x -= CountDrawCards * GameData.CARD_RATIO_WIDTH;
                            posDraw.z = -GameData.DEPT;
                            m_CardDraw.transform.position = posDraw;
                        }
                        m_CardDraw.m_Border.SetActive(false);
                        _card.HideHint();
                        SaveGame();
                        if (!isNormal)
                        {
                            LeanTween.delayedCall(0.2f, () =>
                            {
                                HandleUndo(false);
                                LeanTween.delayedCall(0.5f, () =>
                                {
                                    EnableClick();
                                    SceneManager.instance.PlayGameController.ActiveUI();
                                    if (_card.isFlip && _card.m_Box.enabled)
                                    {
                                        _card.Setposition();
                                    }
                                });
                            });
                        }
                        else
                        {
                            LeanTween.delayedCall(0.2f, () =>
                            {
                                EnableClick();
                                SceneManager.instance.PlayGameController.ActiveUI();
                                foreach(var item in Cards)
                                {
                                    if (item.isFlip && item.m_Box.enabled)
                                    {
                                        item.Setposition();
                                    }
                                }
                            });

                        }
                        SceneManager.instance.PlayGameController.CheckAuto();
                    });
            }
        }
    }

    void SetAttributrCollum(int collum)
    {
        int index = SumNumber(collum);
        {
            for (int i = 0; i <= collum; i++)
            {
                int value = CardValues[index + i];
                Cards[value].Collum = collum;
                Cards[value].Index = i;
                //Debug.Log(index + i);
            }
        }
    }

    private void AnimationCard(int value)
    {
        Cards[value].m_Box.enabled = false;
        LeanTween.scaleX(Cards[value].gameObject, 0, 0.05f).setOnComplete(() => FlipCard(value));
    }

    void FlipCard(int value)
    {
        if (value >= Cards.Length)
            return;
        Cards[value].isFlip = !Cards[value].isFlip;
        LeanTween.scaleX(Cards[value].gameObject, GameData.CARD_SCALE, 0.05f).setOnComplete(() =>
            {
                Cards[value].m_Box.enabled = true;
                //Debug.LogError(Cards[value].transform.position.z);
                //Result.Clear();
            });
    }

    int SumNumber(int number)
    {

        if (number == 0)
            return 0;
        if (number == 1)
            return 1;
        else
        {
            return number + SumNumber(number - 1);
        }
    }

    public void ResetTable(int numbercomplete = 0, int totalcards = 0)
    {
        BlockAllCard();
        LeanTween.cancelAll();
        HideHint();
        ChiaBai = false;
        m_CardDraw.m_Box.enabled = false;
        //CancelInvoke("CHIABAI");
        int total = GameData.NUMBER_CARD;
        NumberWinSuit = numbercomplete;
        NumberComplete = numbercomplete;
        Totalcards = totalcards;
        Compute.GetInstance().Reset();
        for (int i = 0; i < total; i++)
        {
            if (i == total - 1)
                LeanTween.move(Cards[i].gameObject, PositionOrigin, 0.1f).setOnComplete(() =>
                {
                    if (numbercomplete == 0)
                        CountDrawCards = 0;
                    else
                    {
                        switch (numbercomplete)
                        {
                            case 6:
                                CountDrawCards = 5;
                                break;
                            case 5:
                                CountDrawCards = 4;
                                break;

                            case 4:
                                CountDrawCards = 3;
                                break;

                            case 3:
                                CountDrawCards = 3;
                                break;

                            case 2:
                                CountDrawCards = 2;
                                break;

                            case 1:
                                CountDrawCards = 1;
                                break;
                            default:
                                CountDrawCards = 0;
                                break;
                        }
                    }

                    Restart(numbercomplete, totalcards);
                    m_CardDraw.isFirstDrawCard = true;
                    ResetAllCard();

                    LeanTween.delayedCall(0.5f, CHIABAI);
                });
            else
                LeanTween.move(Cards[i].gameObject, PositionOrigin, 0.1f);


        }
        //m_CardDraw.Reset();
        //Test();
    }

    void ResetAllCard()
    {
        int total = GameData.NUMBER_CARD;
        for (int i = 0; i < total; i++)
        {
            Cards[i].ResetCard();
            Cards[i].TurnInteractable(true);
        }
    }

    public void CHIABAI()
    {
        ChiaBai = true;
        float time = GameData.TIME_MOVEDRAW;
        if (NumberComplete == 0)
        {
            int[] cardFlip = Compute.m_Instance.GetArrayCardFlip();
            for (int i = 0; i < GameData.NUMBER_CARDSDRAW; i++)
            {
                int collum = i % GameData.NUMBER_COLLUM;
                int index = i / GameData.NUMBER_COLLUM;
                Vector3 des = PositionCollum[collum];
                des.z = -index;
                //Debug.LogError(des.z);
                int _cardFlip = cardFlip[collum];

                if (index <= _cardFlip)
                {
                    des.y = des.y - index * (GameData.CARD_RATIO_HEIGH_UP);
                }
                else
                {
                    des.y = des.y - _cardFlip * (GameData.CARD_RATIO_HEIGH_UP) - (index - _cardFlip) * GameData.CARD_RATIO_HEIGHT;
                }
                //des.z = -index;
                Card _card = Cards[i];
                _card.Collum = collum;
                _card.Index = index;
                _card.m_Box.enabled = false;
                AudioController.instance.PlaySoundSortCard();
                // Debug.LogError(collum + " " + PositionCollum[collum] + _card.Value);
                //Debug.LogError(_card.Value);
                if (i > 43)
                {
                    _card.isFlip = true;
                }
                if (i != GameData.NUMBER_CARDSDRAW - 1)
                    LeanTween.move(_card.gameObject, des, time);
                else
                    LeanTween.move(_card.gameObject, des, time).setOnComplete(() =>
                        {
                            for (int j = 0; j < GameData.NUMBER_CARDSDRAW; j++)
                            {
                                Cards[j].m_Box.enabled = true;
                            }
                            HandleOndone();
                        });
                time += 0.007f;
            }
        }
        else
        {
            MoveCardsComplete();
            int _index = NumberComplete * 13;
            int range = _index + Totalcards;
            int limit = range - 11;
            for (int i = _index; i < range; i++)
            {
                int collum = (i - _index) % 10;
                int index = (i - _index) / 10;
                Vector3 des = PositionCollum[collum];
                des.y = des.y - index * GameData.CARD_RATIO_HEIGHT;
                des.z = -index;
                Card _card = Cards[i];
                _card.Index = index;
                // Debug.LogError(i + " " + index + " " + _index);
                _card.Collum = collum;
                _card.m_Box.enabled = false;
                if (i > limit)
                {
                    _card.isFlip = true;
                }
                if (i != range - 1)
                    LeanTween.move(_card.gameObject, des, time);
                else
                    LeanTween.move(_card.gameObject, des, time).setOnComplete(() =>
                        {
                            for (int j = _index; j < range; j++)
                            {
                                Cards[j].m_Box.enabled = true;
                            }
                            HandleOndone();
                        });
                time += 0.007f;
            }
        }
    }

    void RemoveList(List<int> list)
    {
        int count = list.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            list.RemoveAt(i);
        }
    }

    public void HandleShowHint(List<Hint> hintdes)
    {

        if (hintdes.Count == 0)
        {
            if (CountDrawCards != 5)
            {
                int currentindex = GameData.NUMBER_CARDSDRAW + CountDrawCards * GameData.NUMBER_COLLUM;
                Cards[currentindex].ShowHint();
                m_Hint.Add(currentindex);
                DrawCard(false);
                return;
            }
            else
            {
                SceneManager.instance.PlayGameController.ActiveUI(true);
                SceneManager.instance.PlayGameController.ShowTextEnd();
                return;
                //BlockAllCard();
            }

        }
        //SceneManager.instance.PlayGameController.TimePlay += 2;
        BlockClick();
        ShowAllHint(hintdes, 0);
    }

    private void ShowHintDraw()
    {
        m_CardDraw.ShowHintDraw();
    }

    public void ShowAllHint(List<Hint> hintdes, int index)
    {
        int count = hintdes.Count;
        //Debug.LogError(CountDrawCards);
        //Debug.LogError(count);
        //Debug.LogError(hintdes[count - 1]);
        if (count == 0)
        {


        }
        if (index > count - 1)
        {
            EnableClick();
            SceneManager.instance.PlayGameController.ActiveUI(true);
            return;
        }

        List<Vector3> _default = new List<Vector3>();
        AudioController.instance.PlayShowHint();
        int numberCard = hintdes[index].ListCard.Count;

        bool isSkip = true;
        foreach (var item in hintdes[index].ListDes)
        {
            int countdes = Compute.m_Instance.GetLenghtCollum(hintdes[index].ListDes[0]);
            if (countdes != 0)
                isSkip = false;
        }

        //if (isSkip)
        //{
        //    ShowAllHint(hintdes, index + 1);
        //    return;
        //}

        for (int i = 0; i < numberCard; i++)
        {

            Card _card = Cards[hintdes[index].ListCard[i]];
            //_card.ShowHint();
            _default.Add(_card.transform.position);
            Vector3 temppos = _card.transform.position;
            temppos.z = -GameData.DEPT + 10 - i;
            _card.transform.position = temppos;
            //Vector3 posdes = hintdes[index].ListPos[0];

            int countdes = Compute.m_Instance.GetLenghtCollum(hintdes[index].ListDes[0]);
            if (countdes > 0)
            {
                Vector3 posdes = Compute.m_Instance.ComputePosition(hintdes[index].ListDes[0], 0);
                posdes.y -= i * GameData.CARD_RATIO_HEIGHT;
                posdes.z -= (GameData.TOTALCARD_INCOLLUM + i);
                int numdes = hintdes[index].ListDes.Count;
                _card.ShowHint();

                if (i == numberCard - 1)
                {
                    LeanTween.move(_card.gameObject, posdes, 0.7f)
                           .setOnComplete(() =>
                           {
                               SetDefaultCardHint(hintdes[index].ListCard, _default);
                               ShowAllHint(hintdes, index + 1);
                           });
                    m_Hint.Clear();
                    m_Hint.Add(hintdes[index].ListCard[i]);
                }
                else
                {
                    LeanTween.move(_card.gameObject, posdes, 0.7f);

                    m_Hint.Clear();
                    m_Hint.Add(hintdes[index].ListCard[i]);
                }
            }
            else
            {
                if (CountDrawCards != 5)
                {
                    int currentindex = GameData.NUMBER_CARDSDRAW + CountDrawCards * GameData.NUMBER_COLLUM;
                    Cards[currentindex].ShowHint();
                    m_Hint.Add(currentindex);
                    DrawCard(false);
                    return;
                }
                else
                {
                    BG[hintdes[index].ListDes[0]].ShowHint();
                    EnableClick();
                    SetDefaultCardHint(hintdes[index].ListCard, _default);
                    //ShowAllHint(hintdes, index + 1);
                    //m_Hint.Clear();
                    //m_Hint.Add(hintdes[index].ListCard[i]);
                    SceneManager.instance.PlayGameController.ActiveUI();
                }
            }
        }
    }

    private void SetDefaultCardHint(List<int> hint, List<Vector3> defaultpos)
    {
        int count = defaultpos.Count;
        //Debug.Log("SetDefaultCardHint");
        for (int i = 0; i < count; i++)
        {
            Cards[hint[i]].transform.position = defaultpos[i];
            Cards[hint[i]].HideHint();
        }
    }

    public void HideHint()
    {
        int count = m_Hint.Count;
        m_CardDraw.TurnOffShowHint();
        if (count > 0)
        {

            for (int j = 0; j < count; j++)
            {
                Cards[m_Hint[j]].HideHint();
            }
            m_Hint.Clear();
        }
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            BG[i].HideHint();
        }

    }

    public void ResetCard(List<int> list)
    {
        int _count = list.Count;
        LeanTween.reset();

        LeanTween.delayedCall(0f, () => { });
        //if (callbackOndone != null)
        //    for (int i = 0; i < _count; i++)
        //    {
        //        int temp = list[i];
        //        Vector3 pos = PositionOrigin;
        //        pos.z = Cards[temp].transform.position.z;
        //        Cards[temp].Default();
        //        if (i != _count - 1)
        //            LeanTween.move(Cards[temp].gameObject, pos, 0.1f);
        //        else
        //            LeanTween.move(Cards[temp].gameObject, pos, 0.1f).setOnComplete(() => callbackOndone());
        //    }
        //else
        //{
        for (int i = 0; i < _count; i++)
        {
            int temp = list[i];
            Vector3 pos = PositionOrigin;
            pos.z = 0;
            Cards[temp].Default();
            Cards[temp].transform.position = pos;
            //if(i != _count -1)
            //LeanTween.move(Cards[temp].gameObject, pos, 0.1f);
            //else
            //    LeanTween.move(Cards[temp].gameObject, pos, 0.1f).setOnComplete(() => { m_CardDraw.m_Box.enabled = true; });
            ////}
        }
        m_CardDraw.m_Box.enabled = true;
    }

    public void Undo(Undo undo, int cardflip, bool isFlip, bool isGetCollum = false, bool isFlipCollumdes = false, int cardflipdes = 0)
    {
        BlockClick();
        int count = undo.ListIndex.Count;
        if (isFlip)
            AnimationUpCard(cardflip);
        if (undo.Collum < GameData.TOTAL_COLLUM)
        {
            if (!isGetCollum)
            {
                SceneManager.instance.PlayGameController.ActiveUI(false);
                for (int i = 0; i < count; ++i)
                {
                    int temp = undo.ListIndex[i];
                    Vector3 pos = Compute.m_Instance.ComputePosition(undo.Collum, undo.ListIndex.Count);
                    pos.y -= (i) * GameData.CARD_RATIO_HEIGHT;
                    pos.z = -(undo.Index + i);
                    Cards[temp].m_Box.enabled = false;
                    //Debug.Log(i);
                    if (i != count - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    else
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW)
                            .setOnComplete(() =>
                            {
                                SetListCardUndoAttri(undo);
                                Compute.m_Instance.ShowCardEnable(undo.Collum);
                                Compute.m_Instance.ShowCardEnable(undo.CollumDes);
                                EnableClick();
                                SceneManager.instance.PlayGameController.ActiveUI(true);
                                SaveGame();
                            });
                }
            }
            else
            {
                //Debug.LogError(undo.IndexDes);
                if (isFlipCollumdes)
                    AnimationUpCard(cardflipdes);
                int lenght = undo.Lenght;
                int range = undo.ListIndex.Count - lenght;
                //Vector3 _pos = PositionCollum[undo.Collum];
                int _index = undo.Index;
                for (int i = 0; i < lenght; i++)
                {
                    int temp = undo.ListIndex[i + range];
                    Vector3 _pos = Compute.m_Instance.ComputePosition(undo.Collum, lenght);
                    _pos.y -= (i) * GameData.CARD_RATIO_HEIGHT;
                    _pos.z = -(_index + i);
                    //Debug.Log(i);
                    //Cards[temp].enabled = false;
                    if (i != lenght - 1)
                        LeanTween.move(Cards[temp].gameObject, _pos, GameData.TIME_MOVEDRAW);
                    else
                        LeanTween.move(Cards[temp].gameObject, _pos, GameData.TIME_MOVEDRAW).setOnComplete(() =>
                            {
                                SaveGame();
                            }
                                );

                }
                //Vector3 pos = PositionCollum[undo.CollumDes];
                int _indexdes = undo.IndexDes;
                for (int i = 0; i < range; i++)
                {
                    int temp = undo.ListIndex[i];
                    Vector3 pos = Compute.m_Instance.ComputePosition(undo.CollumDes, range);
                    pos.y -= (i) * GameData.CARD_RATIO_HEIGHT;
                    //pos.y -= (_indexdes + i) * GameData.CARD_RATIO_HEIGHT;
                    pos.z = -(_indexdes + i);
                    //Debug.Log(i);
                    if (i != range - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    else
                    {
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW).setOnComplete(() =>
                            {
                                SetCardsUndoAttri(undo, _indexdes);
                                Compute.m_Instance.ShowCardEnable(undo.Collum);
                                Compute.m_Instance.ShowCardEnable(undo.CollumDes);
                                EnableClick();
                                NumberWinSuit -= 1;
                                SceneManager.instance.PlayGameController.ActiveUI(true);
                                SaveGame();
                            });
                    }

                }
            }
        }
        else
        {

            CountDrawCards -= 1;
            int index = GameData.NUMBER_CARD - (5 - CountDrawCards) * 10;
            if (!isGetCollum)
            {
                for (int i = 0; i < count; ++i)
                {
                    int temp = undo.ListIndex[i];
                    Vector3 pos = PositionOrigin;
                    pos.x = PositionOrigin.x - (CountDrawCards) * GameData.CARD_RATIO_WIDTH;
                    pos.z = -(GameData.NUMBER_CARD - (index + i));
                    //for (int i = index; i < GameData.NUMBER_CARD; i++)
                    //{
                    //    // Debug.LogError(((i - index) / 10));
                    //    des.x = PositionOrigin.x - ((i - index) / 10) * GameData.CARD_RATIO_WIDTH;
                    //    des.z = -index + i;
                    //    LeanTween.move(Cards[i].gameObject, des, GameData.TIME_MOVEDRAW);
                    //}
                    //Debug.Log(i);
                    Cards[temp].m_Box.enabled = false;
                    if (i != count - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    else
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW)
                            .setOnComplete(() =>
                            {
                                Vector3 posDraw = PositionOrigin;
                                posDraw.x -= CountDrawCards * GameData.CARD_RATIO_WIDTH;
                                posDraw.z = -GameData.DEPT;
                                m_CardDraw.transform.position = posDraw;
                                SetListCardUndoAttri(undo, true);
                                for (int j = 0; j < GameData.TOTAL_COLLUM; j++)
                                {
                                    Compute.m_Instance.ShowCardEnable(j);
                                }
                                EnableClick();

                                SceneManager.instance.PlayGameController.ActiveUI(true);
                                SaveGame();
                            });
                }
            }
            else
            {

                for (int i = 0; i < 12; i++)
                {
                    int temp = undo.ListIndex[i];
                    int _index = Compute.m_Instance.GetLenghtCollum(undo.CollumDes) - 12 + i;

                    // Vector3 pos = PositionCollum[undo.CollumDes];
                    Vector3 pos = Compute.m_Instance.ComputePosition(undo.CollumDes, 12);
                    pos.y -= (i) * GameData.CARD_RATIO_HEIGHT;
                    //pos.y -= _index * GameData.CARD_RATIO_HEIGHT;
                    pos.z = -_index;
                    if (i != 11)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    else
                    {
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW).setOnComplete(() =>
                            {
                                int _count = undo.ListIndex.Count;
                                int indexundo = Compute.m_Instance.GetLenghtCollum(undo.CollumDes) - 12;
                                for (int j = 0; j < _count; ++j)
                                {
                                    int card = undo.ListIndex[i];
                                    Cards[card].isFlip = true;
                                    Cards[card].Collum = undo.Collum;
                                    Cards[card].Index = indexundo + i;
                                    Cards[card].m_Box.enabled = true;
                                }
                                for (int j = 0; j < GameData.TOTAL_COLLUM; j++)
                                {
                                    Compute.m_Instance.ShowCardEnable(j);
                                }
                                Vector3 posDraw = PositionOrigin;
                                posDraw.x -= CountDrawCards * GameData.CARD_RATIO_WIDTH;
                                posDraw.z = -GameData.DEPT;
                                m_CardDraw.transform.position = posDraw;
                                EnableClick();

                                SceneManager.instance.PlayGameController.ActiveUI(true);
                                SaveGame();
                            });
                    }

                }
                for (int i = 12; i < count; ++i)
                {
                    int temp = undo.ListIndex[i];
                    Vector3 pos = PositionOrigin;
                    pos.x = PositionOrigin.x - (CountDrawCards) * GameData.CARD_RATIO_WIDTH;
                    pos.z = -(GameData.NUMBER_CARD - (undo.Index + i));
                    //for (int i = index; i < GameData.NUMBER_CARD; i++)
                    //{
                    //    // Debug.LogError(((i - index) / 10));
                    //    des.x = PositionOrigin.x - ((i - index) / 10) * GameData.CARD_RATIO_WIDTH;
                    //    des.z = -index + i;
                    //    LeanTween.move(Cards[i].gameObject, des, GameData.TIME_MOVEDRAW);
                    //}
                    //Debug.Log(i);
                    Cards[temp].m_Box.enabled = false;
                    if (i != count - 1)
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW);
                    else
                        LeanTween.move(Cards[temp].gameObject, pos, GameData.TIME_MOVEDRAW)
                            .setOnComplete(() =>
                            {
                                SetListCardUndoAttri(undo, true, 12);
                                EnableClick();
                                NumberWinSuit -= 1;
                                SceneManager.instance.PlayGameController.ActiveUI(true);
                            });
                }
            }

            //Debug.LogError(CountDrawCards);
        }


    }

    private void SetListCardUndoAttri(Undo undo, bool isdraw = false, int index = 0)
    {
        int count = undo.ListIndex.Count;

        for (int i = index; i < count; ++i)
        {
            int card = undo.ListIndex[i];

            if (isdraw)
            {
                Cards[card].Collum = GameData.NUMBER_COLLUM + 1;
                Cards[card].Index = -1;
                Cards[card].isFlip = false;
                Cards[card].m_Box.enabled = true;
            }
            else
            {
                Cards[card].Collum = undo.Collum;
                Cards[card].Index = undo.Index + i;
                Cards[card].m_Box.enabled = true;
            }
        }
        //Debug.LogError(CountDrawCards);
    }

    private void SetCardsUndoAttri(Undo undo, int indexcollumdes, bool isGetCollum = false)
    {

        int length = undo.Lenght;
        int range = 13 - length;
        //Debug.LogError(undo.IndexDes);
        for (int i = 0; i < range; i++)
        {
            int card = undo.ListIndex[i];
            Cards[card].Collum = undo.CollumDes;
            Cards[card].Index = undo.IndexDes + i;
            Cards[card].m_Box.enabled = true;
        }

        for (int i = 0; i < length; i++)
        {
            int card = undo.ListIndex[range + i];
            Cards[card].Collum = undo.Collum;
            Cards[card].Index = undo.Index + i;
            Cards[card].m_Box.enabled = true;
        }

    }

    public void ShowHint()
    {
        SceneManager.instance.PlayGameController.ActiveHintButton(false);
        Compute.m_Instance.HandleCheckHint();
    }

    public void HandleUndo(bool isNormal = true)
    {
        Compute.m_Instance.Undo(isNormal);
    }

    private void HandleOndone()
    {
        SceneManager.instance.PlayGameController.ActiveUI();
        EnablekAllCard();
        m_CardDraw.EnableDraw();
        ChiaBai = false;
        if (NumberComplete <= 4)
        {
            int index = GameData.NUMBER_CARD - (5 - CountDrawCards) * 10;

            Vector3 des = PositionOrigin;
            for (int i = index; i < GameData.NUMBER_CARD; i++)
            {
                // Debug.LogError(((i - index) / 10));
                des.x = PositionOrigin.x - ((i - index) / 10) * GameData.CARD_RATIO_WIDTH;
                des.z = -(GameData.NUMBER_CARD - Cards[i].IndexArray);
                LeanTween.move(Cards[i].gameObject, des, GameData.TIME_MOVEDRAW);
            }
        }
        SaveGame();
        //circle.ShowCircle(Cards);
        //ShowAnimationWIn();
    }

    private void AnimationUpCard(int value)
    {
        LeanTween.scaleX(Cards[value].gameObject, 0, 0.04f).setOnComplete(() =>
        {
            bool flip = Cards[value].isFlip;
            if (flip)
                Cards[value].isFlip = !flip;
            LeanTween.scaleX(Cards[value].gameObject, GameData.CARD_SCALE, 0.04f);
        });
    }

    public bool GetStateCard(int cardValue)
    {
        if (cardValue < 0 || cardValue >= GameData.NUMBER_CARD)
            return true;
        else
            return Cards[cardValue].isFlip;
    }

    void MoveCardResultToCollum(Vector3 position, int collum, int index, int cardflip, int value, UnityAction ondone)
    {
        //for(int i = 0; i < Result.Count; i++)
        //{
        //    Debug.LogError(Result[i]);
        //}
        int count = Result.Count;
        float time = 0.08f;

        for (int i = 0; i < count; i++)
        {

            time += 0.002f;
            Card _card = Cards[Result[i]];
            _card.m_Box.enabled = false;
            //_card.m_Box.enabled = false;
            Vector3 temp = position;
            temp.y = temp.y - (i) * GameData.CARD_RATIO_HEIGHT;
            //Debug.LogError(index + i);
            temp.z = -(index + i);

            if (i == count - 1)
                LeanTween.move(_card.gameObject, temp, time).setOnComplete(() =>
                    {
                        SetAttributeCardResult(collum, index, cardflip, value, ondone);
                        //Compute.m_Instance.CloseCollum(collum);
                        SceneManager.instance.PlayGameController.CreateEffect(_card.Value % 4, _card.gameObject.transform.position);
                    });
            else
            {
                LeanTween.move(_card.gameObject, temp, time).setOnComplete(() =>
                {
                    if (i == count - 1)
                    {
                        SceneManager.instance.PlayGameController.CreateEffect(_card.Value % 4, temp);
                    }
                });
            }
            AudioController.instance.PlaySoundSortCard();
        }
    }

    void SetAttributeCardResult(int collum, int index, int cardflip, int value, UnityAction ondone)
    {
        int count = Result.Count;
        for (int i = 0; i < count; i++)
        {
            Card _card = Cards[Result[i]];
            _card.Collum = collum;
            _card.Index = index + i;
        }
        //if (value >= 0)
        //{
        //    //Debug.Log(ListDrawResult.Count);
        //    //Debug.Log(value);
        //    m_CardDraw.RemoveCard(Cards, value);
        //    //Debug.Log("xxx");
        //}
        //else
        //{
        //    m_CardDraw.SortListCardDraw(Cards);
        //}

        if (cardflip >= 0)
        {
            AnimationCard(cardflip);
            LeanTween.delayedCall(0.1f, () =>
        {
            for (int i = 0; i < count; i++)
            {
                Cards[Result[i]].m_Box.enabled = true;
            }

            ResetResult();
            EnableClick(true);
            isDrag = false;
            ondone();
            Compute.m_Instance.CheckSuitInCollum(collum);
            SaveGame();
        });
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Cards[Result[i]].m_Box.enabled = true;
            }
            ResetResult();
            EnableClick(true);
            isDrag = false;
            ondone();
            Compute.m_Instance.CheckSuitInCollum(collum);
            SaveGame();
        }

    }

    public void BlockAllCard()
    {
        int count = GameData.NUMBER_CARD;
        for (int i = 0; i < count; i++)
        {
            Cards[i].m_Box.enabled = false;
        }
        m_CardDraw.EnableDraw(false);
    }

    public void EnablekAllCard()
    {
        int count = GameData.NUMBER_CARD;
        for (int i = 0; i < count; i++)
        {
            Cards[i].m_Box.enabled = true;
        }
        m_CardDraw.EnableDraw(true);
        //m_CardDraw.SetUpCardDraw(Cards);
    }

    private void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    public void StopAnimation()
    {
        isShowAnimation = false;
        //SceneManager.instance.PlayGameController.NewGame();

    }

    public void ChangeCardBack(int index)
    {
        //Debug.LogError(index);
        int numberCard = GameData.NUMBER_CARD;
        for (int i = 0; i < numberCard; i++)
        {
            Cards[i].SetCardBack(index);
            Cards[i].ChangeBackGround();
        }
        m_CardDraw.SetCardBack(index);
    }

    public List<int> GetListCardValue(List<int> indexarray)
    {
        List<int> _result = new List<int>();
        int count = indexarray.Count;
        for (int i = 0; i < count; i++)
        {
            _result.Add(CardValues[indexarray[i]]);
        }
        return _result;
    }

    public void HandleWinSuit(List<int> cards, int flip)
    {
        BlockClick();
        SceneManager.instance.PlayGameController.ActiveUI(false);
        int _count = cards.Count;
        Vector3 des = PositionCollum[0];
        des.x += GameData.CARD_RATIO_WIDTH * NumberWinSuit;
        des.y = PositionOrigin.y;
        for (int i = 0; i < _count; i++)
        {
            Card _card = Cards[cards[i]];
            // Debug.LogError(_card.Value);
            _card.m_Box.enabled = false;
            des.z = -((NumberWinSuit + 1) * 13 - i);
            if (i == _count - 1)
                LeanTween.move(_card.gameObject, des, GameData.TIME_MOVEDRAW + 0.03f * i).setOnComplete(() =>
                    {
                        SceneManager.instance.PlayGameController.CreateEffect(_card.Value % 4, des);
                        if (flip >= 0)
                        {
                            FlipCard(flip);
                        }

                        NumberWinSuit += 1;
                        EnableClick();
                        SceneManager.instance.PlayGameController.ActiveUI(true);
                        SaveGame();
                        if (NumberWinSuit == 8)
                        {
                            BlockAllCard();
                            SceneManager.instance.PlayGameController.ShowWin();
                        }

                    });
            else
                LeanTween.move(_card.gameObject, des, GameData.TIME_MOVEDRAW + 0.03f * i);
        }
    }

    public void BlockClick(int carddown = -1)
    {
        // Debug.LogError("BlockClick");
        ListCardFlip = Compute.m_Instance.GetListCardFlip();
        int count = ListCardFlip.Count;
        for (int i = 0; i < count; i++)
        {
            Cards[ListCardFlip[i]].m_Box.enabled = false;
        }
        m_CardDraw.m_Box.enabled = false;
    }

    public void EnableClick(bool isMove = false)
    {
        //Debug.LogError("EnableClick");
        int count = ListCardFlip.Count;
        for (int i = 0; i < count; i++)
        {
            Cards[ListCardFlip[i]].m_Box.enabled = true;
        }
        m_CardDraw.m_Box.enabled = true;
        ListCardFlip.Clear();
    }

    private void MoveCardsComplete()
    {
        // Debug.LogError(NumberComplete);
        int range = NumberComplete * 13;
        Vector3 des = PositionCollum[1];

        des.y = PositionOrigin.y;
        for (int i = 0; i < range; i++)
        {
            Card _card = Cards[i];
            _card.isFlip = true;
            _card.m_Box.enabled = false;
            // Debug.LogError(i / 13);
            des.x = PositionCollum[1].x + GameData.CARD_RATIO_WIDTH * (i / 13);
            des.z = -i;
            LeanTween.move(_card.gameObject, des, GameData.TIME_MOVEDRAW);
        }
    }

    public void CancelMoveAll()
    {
        LeanTween.cancelAll();
    }

    public void AnimationWin()
    {
        Animation _ani = new Animation(Cards,Compute.m_Instance.GetListCardWin());

    }

    public void ShowCardEnable(List<int> enable, List<int> disable)
    {
        int enableCount = enable.Count;
        int disableCount = disable.Count;
        for (int i = 0; i < enableCount; i++)
        {
            Cards[enable[i]].ShowEnableClick();
        }

        for (int i = 0; i < disableCount; i++)
        {
            Cards[disable[i]].ShowEnableClick(false);
        }
    }

    public void CloseCollum(List<int> cards, int lengt, int range, int collum, bool isClose = false)
    {
        int count = cards.Count;
        int _range = lengt - range;
        //Debug.Log("CloseCollum" + " " + collum);
        for (int i = 0; i < count; i++)
        {

            if (i < _range)
            {
                // Debug.Log(i);
                if (isClose)
                {
                    Vector3 pos = Compute.m_Instance.GetPositionFlip(collum);
                    pos.y -= i * GameData.CARD_RATIO_HEIGH_UP * 2.5f;
                    pos.z = Cards[cards[i]].transform.position.z;
                    Cards[cards[i]].transform.position = pos;
                    //pos.y = 
                }
                else
                {
                    Vector3 pos = Compute.m_Instance.GetPositionFlip(collum);
                    pos.y -= i * GameData.CARD_RATIO_HEIGHT;
                    pos.z = Cards[cards[i]].transform.position.z;
                    Cards[cards[i]].transform.position = pos;
                }
            }
            else
            {
                if (isClose)
                {
                    Vector3 pos = Compute.m_Instance.PositionClose(collum, range);
                    pos.y -= (i - _range) * (GameData.CARD_RATIO_HEIGHT) + ((lengt - range ) * GameData.CARD_RATIO_HEIGH_UP * 1.5f);
                    pos.z = Cards[cards[i]].transform.position.z;
                    Cards[cards[i]].transform.position = pos;
                }
                else
                {
                    //Debug.LogError(i);
                    Vector3 pos = Compute.m_Instance.PositionClose(collum, range);
                    //if (_range > 0)
                    pos.y -= ((i - _range)) * (GameData.CARD_RATIO_HEIGHT);
                    pos.z = Cards[cards[i]].transform.position.z;
                    Cards[cards[i]].transform.position = pos;
                }
            }
        }
    }

    public void SaveGame()
    {
        string m_Data = "";
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Arcade)
            m_Data += SceneManager.instance.CurrentLevel + "*";
        else
            m_Data += "-1*";
        m_Data += CountDrawCards + "*";
        for (int i = 0; i < GameData.NUMBER_CARD; i++)
        {
            int isFlip;
            int isActive;
            if (Cards[i].isFlip)
                isFlip = 1;
            else
                isFlip = 0;
            if (Cards[i].m_Box)
                isActive = 1;
            else
                isActive = 0;
            if (i != GameData.NUMBER_CARD - 1)
                m_Data += CardValues[i] + "_" + isFlip + "_" + isActive + "-";
            else
                m_Data += CardValues[i] + "_" + isFlip + "_" + isActive + "*";
        }
        m_Data = string.Format("{0}{1}", m_Data, Compute.m_Instance.SaveGame());
        PlayerPrefs.SetString(SceneManager.instance.DATA_GAME, m_Data);
        //Debug.LogError(Application.persistentDataPath + "/" + "save.txt");
        //using (StreamWriter outputFile = new StreamWriter(Application.persistentDataPath + @"\save.txt", false))
        //{
        //    outputFile.Write(m_Data);
        //}
        //
    }

    //public void SaveDataGame()
    //{
    //    File.WriteAllText(Application.persistentDataPath + "/" + "save.txt", m_Data);
    //}

    public void LoadDataGame(string[] data)
    {
        string[] _cards = data[2].Split('-');
        for (int i = 0; i < _cards.Length; i++)
        {
            string[] _value = _cards[i].Split('_');
            for (int j = 0; j < _value.Length; j++)
            {
                CardValues[i] = int.Parse(_value[0]);
                //Debug.LogWarning(CardValues[i]);
                Cards[i].Value = CardValues[i];
                int isFlip = int.Parse(_value[1]);
                int isActive = int.Parse(_value[2]);
                if (isFlip == 1)
                {
                    Cards[i].isFlip = true;
                }
                else
                {
                    Cards[i].isFlip = false;
                }

                if (isActive == 1)
                {
                    Cards[i].m_Box.enabled = true;
                }
                else
                {
                    Cards[i].m_Box.enabled = false;
                }
            }
        }
        CountDrawCards = int.Parse(data[1]);
        Compute.m_Instance.LoadDataGame(data);
        Vector3 posDraw = PositionOrigin;
        posDraw.x -= CountDrawCards * GameData.CARD_RATIO_WIDTH;
        posDraw.z = -GameData.DEPT;
        m_CardDraw.transform.position = posDraw;
        SceneManager.instance.PlayGameController.CheckAuto();
    }

    public void LoadCard(List<int>[] data, int[] cardflip)
    {
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            int count = data[i].Count;

            for (int j = 0; j < count; j++)
            {
                Vector3 temp = PositionCollum[i];
                Cards[data[i][j]].Collum = i;
                if (j < cardflip[i])
                {
                    //Debug.LogError(j);
                    temp.y -= j * GameData.CARD_RATIO_HEIGH_UP;
                    Cards[data[i][j]].transform.position = temp;
                }
                else
                    Cards[data[i][j]].transform.position = PositionCollum[i];
                Cards[data[i][j]].Index = j;
            }
        }
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            Compute.m_Instance.ShowCardEnable(i);
        }

        int index = GameData.NUMBER_CARD - (5 - CountDrawCards) * 10;
        Vector3 des = PositionOrigin;
        for (int i = index; i < GameData.NUMBER_CARD; i++)
        {
            // Debug.LogError(((i - index) / 10));
            des.x = PositionOrigin.x - ((i - index) / 10 + CountDrawCards) * GameData.CARD_RATIO_WIDTH;
            des.z = -(GameData.NUMBER_CARD - Cards[i].IndexArray);
            LeanTween.move(Cards[i].gameObject, des, 0);
        }
        SceneManager.instance.PlayGameController.ActiveUI();
        m_CardDraw.EnableDraw();
        ChiaBai = false;
        //SceneManager.instance.PlayGameController.ActiveUI();
    }

    public void WinSuitLoadData(List<int> cards)
    {
        int _count = cards.Count;

        for (int i = 0; i < _count; i++)
        {
            Vector3 des = PositionCollum[0];
            int ratio = i / 13;
            des.x += GameData.CARD_RATIO_WIDTH * ratio;
            des.y = PositionOrigin.y;
            Card _card = Cards[cards[i]];
            // Debug.LogError(_card.Value);
            _card.m_Box.enabled = false;
            des.z = (-i);
            if (i == _count - 1)
                LeanTween.move(_card.gameObject, des, 0).setOnComplete(() =>
                {
                    SceneManager.instance.PlayGameController.ActiveUI(true);
                });
            else
                LeanTween.move(_card.gameObject, des, 0);
        }

    }

    public void SetPositonDraw()
    {
        Vector3 posDraw = Cards[GameData.NUMBER_CARDSDRAW + CountDrawCards * GameData.NUMBER_COLLUM].transform.position;
        posDraw.z = -GameData.DEPT;
        m_CardDraw.transform.position = posDraw;
    }

    public void DisableDragCard(int index, bool isDrag = false)
    {
        Cards[index].isDrag = isDrag;
    }

    public void ChangeCardFace(int index)
    {
        for (int i = 0; i < GameData.NUMBER_CARD; i++)
        {
            Cards[i].m_IndexCardFace = index;
        }
    }


    public bool CheckShowAuto()
    {
        foreach (var item in Cards)
        {
            if (!item.isFlip || !item.IsShow)
                return false;
        }

        return CountDrawCards == 5;
    }

    public void AutoComplete(bool isNormal = true)
    {
        SceneManager.instance.PlayGameController.ActiveUI(false);
        float delayTime = 0f;
        var card = new List<Card>();
        List<Card> CardK = new List<Card>();
        foreach (var item in Cards)
        {
            if (item.Index == 0 && item.Collum < GameData.TOTAL_COLLUM && ((item.Value/4) % 12 != 0 || item.Value ==0))
            {
                card.Add(item);
            }
            else if (item.Index == 0 && item.Collum < GameData.TOTAL_COLLUM && (item.Value / 4) % 12 == 0)
            {
                CardK.Add(item);
            }
        }
        int index = 0;
        List<int> data = Compute.m_Instance.GetDataAutoWin();
        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                //Debug.LogError(card[i].Value + " " + card[i].IndexArray);
                LeanTween.delayedCall(delayTime, () =>
                {
                    //Debug.LogError(card[i])
                    Cards[data[index]].OnThisCardClick();
                    index++;
                });
                delayTime += 0.35f;
            }
        }
        else
        {
            data = Compute.m_Instance.GetDataAutoWin(true);
            index = 0;
            for (int i = 0; i < data.Count; i++)
            {
                LeanTween.delayedCall(delayTime, () =>
                {
                    Cards[data[index]].OnThisCardClick();
                    index++;
                });
                delayTime += 0.35f;
            }
        }

        LeanTween.delayedCall(delayTime + 0.2f, () =>
        {
            if (!Compute.m_Instance.CheckWinCard())
            {
                AutoComplete();
            }
            else
            {
                SceneManager.instance.PlayGameController.ActiveUI();
            }
        });
    }
}

