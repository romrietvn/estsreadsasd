using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SimpleJson;

public class Compute : MonoBehaviour
{

    #region SingleTon
    public static Compute m_Instance;
    public static Compute GetInstance()
    {
        return m_Instance;
    }
    #endregion
    public RectTransform Button_Back;
    Vector2[] PosCollums = new Vector2[GameData.TOTAL_COLLUM];
    Vector2 PositionOrigin;
    Vector2 PositionDraw;

    private List<int>[] Collums = new List<int>[GameData.TOTAL_COLLUM];
    private List<int>[] IndexDeck = new List<int>[GameData.TOTAL_COLLUM];
    private int[] LastCardCollums = new int[GameData.TOTAL_COLLUM];
    private Rule m_Rule = new Rule();
    private List<int> m_Result = new List<int>();
    private List<int> CurrentCardsIndexDrag = new List<int>();
    private List<int> CurrentCardsValueDrag = new List<int>();
    public int[] CardFlip = new int[GameData.NUMBER_COLLUM];
    public static float Dept;
    int CurrentCollum;
    private bool isDrag = false;
    List<Hint> m_Hint = new List<Hint>();
    Vector2 CardOriginPosition;
    List<Undo> m_Undo = new List<Undo>();
    bool isFirstTouch = true;
    int CountHint = 0;
    private List<int> CardsWin = new List<int>();
    private bool[] CloseCollums = new bool[GameData.NUMBER_COLLUM];

    void Awake()
    {
        //Debug.Log("Compute" + Time.realtimeSinceStartup);

        m_Instance = this;
        for (int i = 0; i < GameData.TOTAL_COLLUM; i++)
        {
            Collums[i] = new List<int>();
            IndexDeck[i] = new List<int>();
            //Debug.Log(Collums[i].Count);
        }
        InitPosition(GameData.eScreen.Portrait);
    }

    public void Reset()
    {
        for (int i = 0; i < GameData.TOTAL_COLLUM; i++)
        {
            Collums[i].Clear();
            //RemoveList(IndexDeck[i]);
            IndexDeck[i].Clear();
            CloseCollums[i] = false;
        }
        m_Result.Clear();
        CurrentCardsIndexDrag.Clear();
        CurrentCardsValueDrag.Clear();
        m_Hint.Clear();
        m_Undo.Clear();
        CardsWin.Clear();
        CurrentCollum = 0;
        isFirstTouch = true;

    }

    void InitPosition(GameData.eScreen screen)
    {
        if (screen == GameData.eScreen.Portrait)
        {
            ModePortrait();
        }
        else
        {
            ModeLancape();
        }
    }

    #region SetPositionFolowMode
    void ModePortrait()
    {
        GameData.CARD_PADDING_WIDTH = (GameData.SCREEN_WIDTH / 100 - (GameData.CARD_WIDTH * GameData.NUMBER_COLLUM)) / (GameData.NUMBER_COLLUM + 1);
        //Debug.Log("Padding  " + GameData.CARD_PADDING_WIDTH);
        GameData.CARD_PADDING_HEIGHT = GameData.CARD_PADDING_WIDTH;
        short numberCollum = GameData.NUMBER_COLLUM;
        for (short i = 0; i < numberCollum; i++)
        {
            PosCollums[i].x = (i + 1) * GameData.CARD_PADDING_WIDTH + i * GameData.CARD_WIDTH - GameData.SCREEN_WIDTH / 200 + GameData.CARD_WIDTH / 2;
            PosCollums[i].y = GameData.SCREEN_HEIGHT / 200 - 2.4f * GameData.CARD_HEIGHT;
            //Debug.Log("Collum" + i + " " + PosCollums[i]);
        }
        //short totaldes = GameData.DESTINATION;
        //for (short i = 0; i < totaldes; i++)
        //{
        //    PosCollums[numberCollum + i].x = (i + 1) * GameData.CARD_PADDING_WIDTH + i * GameData.CARD_WIDTH - GameData.SCREEN_WIDTH / 200 + GameData.CARD_WIDTH / 2;
        //    PosCollums[numberCollum + i].y = GameData.SCREEN_HEIGHT / 200 - GameData.CARD_PADDING_HEIGHT;
        //    //Debug.Log("Collum" + (numberCollum + i) + " " + PosCollums[numberCollum + i]);
        //}
        PositionOrigin.x = PosCollums[GameData.NUMBER_COLLUM - 1].x - GameData.CARD_WIDTH / 2;
        //PositionOrigin.y = Button_Back.transform.position.y/100 - Button_Back.sizeDelta.y / 200 - GameData.CARD_RATIO_HEIGHT / 2;
        PositionOrigin.y = (GameData.SCREEN_HEIGHT / 200) - GameData.CARD_HEIGHT / 2 - 15 * GameData.CARD_PADDING_HEIGHT;
        PositionDraw.x = PositionOrigin.x - GameData.CARD_PADDING_WIDTH - GameData.CARD_WIDTH;
        PositionDraw.y = PositionOrigin.y;
        //Debug.Log(PositionOrigin);
        // Bound = GameData.SCREEN_HEIGHT / 200 - GameData.CARD_PADDING_HEIGHT - GameData.CARD_HEIGHT;
    }

    void ModeLancape()
    {
        GameData.CARD_PADDING_WIDTH = ((GameData.SCREEN_WIDTH - (GameData.CARD_WIDTH * GameData.NUMBER_COLLUM)) / (GameData.NUMBER_COLLUM + 1));
    }
    #endregion

    public Vector2 GetFirstPositionCollum()
    {
        return PosCollums[0];
    }

    public void InitData(int[] cards, int numbercomplete = 0, int totalcards = 0)
    {

        int _collum = GameData.NUMBER_COLLUM;
        if (numbercomplete == 0)
        {
            for (int i = 0; i < _collum; i++)
            {
                CloseCollums[i] = false;
                Collums[i].Add(cards[i]);
                Collums[i].Add(cards[i + 10]);
                Collums[i].Add(cards[i + 20]);
                Collums[i].Add(cards[i + 30]);
                Collums[i].Add(cards[i + 40]);
                IndexDeck[i].Add(i);
                IndexDeck[i].Add(i + 10);
                IndexDeck[i].Add(i + 20);
                IndexDeck[i].Add(i + 30);
                IndexDeck[i].Add(i + 40);
                if (i < 4)
                {
                    Collums[i].Add(cards[i + 50]);
                    IndexDeck[i].Add(i + 50);
                    CardFlip[i] = 5;
                }
                else
                {
                    CardFlip[i] = 4;
                }
            }
        }
        else
        {
            int numberrow = totalcards / 10;
            int bonus = totalcards % 10;
            int _index = numbercomplete * 13;
            //Debug.LogError(_index + " " + totalcards +" " + cards.Length +" " + numberrow);
            for (int i = 0; i < _index; i++)
            {
                CardsWin.Add(i);
            }

            for (int i = 0; i < GameData.TOTAL_COLLUM; i++)
            {
                for (int j = 0; j < numberrow; j++)
                {
                    //Debug.LogError((_index + i + j * 10) + " " + cards[_index + i + j * 10] + " " + j);
                    // Debug.LogError(i + " " + j);
                    Collums[i].Add(cards[_index + i + j * 10]);
                    IndexDeck[i].Add(_index + i + j * 10);
                    // Debug.LogError((_index + i + j * 10) + " " +cards[_index + i + j * 10] + " " + j);
                }
                if (i < bonus)
                {
                    Collums[i].Add(cards[_index + i + numberrow * 10]);
                    IndexDeck[i].Add(_index + i + numberrow * 10);
                }
            }
            for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                CardFlip[i] = Collums[i].Count - 1;
            }
        }

        UpdateAllCollum();
    }

    private void InitDataCollum(int[] cards, int collum, int index, int range)
    {
        int count = index + range;
        for (int i = index; i < count; i++)
        {
            Collums[collum].Add(cards[i]);
            //Debug.Log("collum" + collum + "index" + index + "value " + cards[i]);
        }
        // Debug.Log("Collum" + collum + Collums[collum].Count);
        UpdateCollum(collum);
        //for(int i = 0; i < Collums[collum].Count; i++)
        //{
        //    Debug.Log("collum " + collum + "index " + i + "value " + Collums[collum][i]);
        //}
    }

    private void UpdateCollum(int collum)
    {
        int count = Collums[collum].Count - 1;
        LastCardCollums[collum] = Collums[collum][count];
        // Debug.Log(LastCardCollums.Length);
    }

    private void UpdateAllCollum()
    {
        int count = GameData.TOTAL_COLLUM;
        for (int i = 0; i < count; i++)
        {
            int index = Collums[i].Count - 1;
            if (index >= 0)
            {
                LastCardCollums[i] = Collums[i][index];
                // Debug.Log(LastCardCollums[i]/4 + " " + LastCardCollums[i]%4);
            }

        }


    }

    public void HandleOnTouch(int collum, int index, int value, int indexarray, Vector2 position)
    {

        if (Time.timeScale == 0)
            return;
        if (isFirstTouch)
        {
            SceneManager.instance.PlayGameController.StartTimeCount();
            isFirstTouch = false;
        }
        if (!isDrag)
        {
            Table.GetInstance().HideHint();
            RemoveListHint();
            CountHint = 0;
            if (m_Rule.CheckInCollum(Collums, index, collum))
            {
                //Debug.Log("xxx");

                Table.GetInstance().BlockClick();
                CardOriginPosition = position;
                Table.m_Instance.DisableDragCard(indexarray, true);
                if (collum < GameData.NUMBER_COLLUM)
                {
                    int range = IndexDeck[collum].Count - index;
                    CurrentCardsIndexDrag = IndexDeck[collum].GetRange(index, range);
                    CurrentCardsValueDrag = Collums[collum].GetRange(index, range);
                    //Debug.LogError(CurrentCardDarg[0]);
                    Table.Result.AddRange(CurrentCardsIndexDrag);
                }
                else
                {

                    CurrentCardsIndexDrag.Add(indexarray);
                    CurrentCardsValueDrag.Add(value);
                    Table.Result.Add(indexarray);
                }
                m_Rule.CheckAllCollum(Collums, LastCardCollums, value, m_Result, collum, CurrentCardsIndexDrag);


            }
            else
            {
                CloseCollums[collum] = !CloseCollums[collum];
                CloseCollum(collum);
                Table.m_Instance.DisableDragCard(indexarray);
            }
        }
        else
        {
            Debug.Log(m_Result.Count);
        }
        // Debug.Log(CloseCollums[collum]);
        // Debug.Log("Currentdrag card" + CurrentCardDarg.Count);
    }

    public void HandleOnDrag(Vector2 position, int collum, int index)
    {

        //if (!m_Rule.CheckInCollum(Collums, index, collum))
        //    return;
        if (!isDrag)
            isDrag = true;
        Table.GetInstance().DragCard(position);
        CurrentCollum = ComputeCurrentIndex(position, collum);
        //.Log(CurremtIndex);
    }

    public void HandleEndDrag(int collum, int index, int value, int indexarray, Vector2 position, bool isNormal = true)
    {
        //if (!m_Rule.CheckInCollum(Collums, index, collum))
        //    return;
        if ((IsCardMove(position) || !isNormal) && !m_Result.Contains(CurrentCollum) && m_Result.Count > 0)
        {

            int _index = UpdateCardIsTouch(collum, index);
            // Debug.LogWarning("ismove");
            Vector3 pos = ComputeDesPosition(true, collum, index, value, true);
            // Debug.LogError(m_Result.Count);
            int _collum = m_Result[0];
            if (!CheckCollum(_collum))
            {
                Undo _undo = new Undo(index, collum, m_Result[0], CurrentCardsIndexDrag, CurrentCardsValueDrag);
                //Debug.LogError(_undo.IsGetCollum);
                CheckFlip(_undo, collum, index);
                m_Undo.Add(_undo);
            }
            else
            {
                int _indexcollum = Collums[_collum].Count - 13;
                Undo _undo = new Undo(index, collum, m_Result[0], IndexDeck[_collum].GetRange(_indexcollum, 13), Collums[_collum].GetRange(_indexcollum, 13), true, false, CurrentCardsIndexDrag.Count);
                CheckFlip(_undo, collum, index);
                CheckFlip(_undo, _collum, _indexcollum, true);
                _undo.IndexDes = _indexcollum;
                m_Undo.Add(_undo);
                // Debug.Log(m_Undo[m_Undo.Count - 1].IndexDes);
                //Debug.Log(m_Undo[m_Undo.Count - 1].IsflipCollumdes);
            }
            UpdateAllCollum();
            ResetResult();
            ResetCurrentCardDrag();

            // Vector3 pos = ComputeDesPosition(true, collum, index, value);
            int cardflip = IsFlipCard(collum);
            //Debug.Log(cardflip);
            //Debug.Log("collum" + collum);
            //if (collum > GameData.TOTAL_COLLUM)
            //    Table.GetInstance().Handle_OnCardEndDrag(pos, _collum, _index, cardflip, value);
            //else
            Table.GetInstance().Handle_OnCardEndDrag(pos, _collum, _index, cardflip, -1, () => { ShowCardEnable(_collum); });
            ShowCardEnable(collum);


            if (isNormal)
            {
                SceneManager.instance.PlayGameController.MoveCount += 1;
                SceneManager.instance.PlayGameController.CheckAuto();
            }
            CheckWin();
        }
        else
        {
            if (m_Result.Contains(CurrentCollum))
            {
                //Debug.LogError("Move");
                int _index = HandleOnDragTrue(collum, index);
                Vector3 pos = ComputeDesPosition(true, collum, index, value);
                if (CheckCollum(CurrentCollum))
                {
                    int _indexcollum = Collums[CurrentCollum].Count - 13;
                    Undo _undo = new Undo(index, collum, CurrentCollum, IndexDeck[CurrentCollum].GetRange(_indexcollum, 13), Collums[CurrentCollum].GetRange(_indexcollum, 13), true, false, CurrentCardsIndexDrag.Count);
                    CheckFlip(_undo, collum, index);
                    CheckFlip(_undo, CurrentCollum, _indexcollum, true);
                    _undo.IndexDes = _indexcollum;
                    m_Undo.Add(_undo);
                    //Debug.Log(m_Undo[m_Undo.Count - 1].IsflipCollumdes);
                }
                else
                {
                    Undo _undo = new Undo(index, collum, CurrentCollum, CurrentCardsIndexDrag, CurrentCardsValueDrag);
                    CheckFlip(_undo, collum, index);
                    m_Undo.Add(_undo);
                    //Debug.Log(m_Undo[m_Undo.Count - 1].IsGetCollum);
                }
                // Debug.Log(m_Undo[m_Undo.Count -1].IsGetCollum);

                UpdateAllCollum();
                ResetResult();
                ResetCurrentCardDrag();


                int cardflip = IsFlipCard(collum);

                //Debug.Log(cardflip);
                //if (collum > GameData.TOTAL_COLLUM)
                //    Table.GetInstance().Handle_OnCardEndDrag(pos, CurrentCollum, _index, cardflip, value);
                //else
                Table.GetInstance().Handle_OnCardEndDrag(pos, CurrentCollum, _index, cardflip, -1, () => { ShowCardEnable(CurrentCollum); });
                ShowCardEnable(collum);
                //ShowCardEnable(CurrentCollum);
                if (isNormal)
                {
                    SceneManager.instance.PlayGameController.MoveCount += 1;
                    SceneManager.instance.PlayGameController.CheckAuto();
                }
                CheckWin();
                RemoveListHint();

                //Debug.Log("UndoCount " + m_Undo.Count + "Index " +
                //   m_Undo[m_Undo.Count - 1].Index + "Collum " +
                //   m_Undo[m_Undo.Count - 1].Collum
                //   + "CollumDes " + m_Undo[m_Undo.Count - 1].CollumDes + "List " + TestListCardUndo());
                SceneManager.instance.PlayGameController.CheckAuto();
            }
            else
            {
                //Debug.Log("HandleEndDrag fail");
                //Debug.LogWarning("fail");
                Vector3 pos = ComputeDesPosition(false, collum, index, value);
                ResetResult();
                UpdateAllCollum();
                ResetCurrentCardDrag();
                Table.GetInstance().Handle_OnCardEndDrag(pos, index, collum);
                if (isNormal)
                {
                    SceneManager.instance.PlayGameController.CheckAuto();
                }
                //DebugCurrentCollum();

            }
        }
        isDrag = false;
        CurrentCollum = 0;
        //DebugListCardFlip();
        //DebugCurrentCollum();
        //Debug.Log("HandleEndDrag" + m_Result.Count + " " + CurrentCardDarg.Count);

    }

    void CheckFlip(Undo undo, int collum, int index, bool isGetCollum = false)
    {
        if ((collum < GameData.TOTAL_COLLUM) && (index > 0))
        {

            //Debug.Log("Collum " + collum + "Index " + (index -1));
            bool check = Table.m_Instance.GetStateCard(IndexDeck[collum][index - 1]);
            //Debug.Log("Collum" + collum + " Count " + (index - 1) + " value" + check);
            if (!isGetCollum)
                undo.Isflip = !check;
            else
                undo.IsflipCollumdes = !check;

            //Debug.Log(undo.IsFlip);
        }
    }

    bool CheckFlipCollumDes(int collum, int index)
    {
        if ((collum < GameData.TOTAL_COLLUM) && (index > 0))
        {
            bool check = Table.m_Instance.GetStateCard(IndexDeck[collum][index - 1]);
            return !check;
        }
        return false;

    }

    void ResetResult()
    {
        int count = m_Result.Count;
        if (count > 0)
            m_Result.RemoveRange(0, count);
    }

    void ResetCurrentCardDrag()
    {
        CurrentCardsIndexDrag.Clear();
        CurrentCardsValueDrag.Clear();
    }

    int ComputeCurrentIndex(Vector2 position, int currentcollum)
    {
        //int count = GameData.TOTAL_COLLUM;
        //for (int i = GameData.NUMBER_COLLUM; i < count; i++)
        //{
        //    if (m_Rule.CheckPosition(position, PosCollums[i], true) && i != currentcollum)
        //    {
        //        //Debug.Log(i);
        //        return i;
        //    }
        //}

        int countCollum = GameData.NUMBER_COLLUM;
        for (int i = 0; i < countCollum; i++)
        {

            if (m_Rule.CheckPosition(position, PosCollums[i], false) && i != currentcollum)
            {
                //Debug.Log(i);
                return i;
            }
        }
        return currentcollum;
    }

    public Vector2[] GetPositionAllCollum()
    {
        return PosCollums;
    }

    public Vector2 GetPositionCollum(int collum)
    {
        if (collum < GameData.TOTAL_COLLUM)
            return PosCollums[collum];
        else
            return PositionOrigin;
    }

    public Vector2 GetPositionOrigin()
    {
        return PositionOrigin;
    }

    int HandleOnDragTrue(int collum, int index)
    {
        int result = Collums[CurrentCollum].Count;
        //Debug.Log(result);
        Collums[CurrentCollum].AddRange(CurrentCardsValueDrag);
        // Collums[m_Result[0]].AddRange(Table.m_Instance.GetListCardValue(CurrentCardDarg));
        //Debug.LogError(Table.m_Instance.GetListCardValue(CurrentCardDarg)[0]);
        IndexDeck[CurrentCollum].AddRange(CurrentCardsIndexDrag);
        // Debug.Log("Collum" + CurremtIndex + " " + Collums[CurremtIndex].Count  );
        //for(int i = 0; i < Collums[CurremtIndex].Count; i++)
        //{
        //    Debug.Log(Collums[CurremtIndex][i]);
        //}
        int range = Collums[collum].Count - index;
        Collums[collum].RemoveRange(index, range);
        IndexDeck[collum].RemoveRange(index, range);
        // Debug.Log("Collum" + collum + Collums[collum].Count);
        //for(int i = 0; i < Collums[collum].Count; i++)
        //{
        //    Debug.Log(Collums[collum][i]);
        //}
        //Debug.Log(result);
        // Debug.Log(result);
        return result;


    }

    Vector3 ComputeDesPosition(bool value, int collum, int index, int _value, bool isMove = false)
    {
        //Debug.LogError("ComputeDesPosition");
        if (!isMove)
        {
            if (value)
            {

                Vector3 pos = PosCollums[CurrentCollum];
                int flip = CardFlip[CurrentCollum];
                int lenght = Collums[CurrentCollum].Count;
                if (!CloseCollums[CurrentCollum])
                {
                    pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT;
                }
                else
                {
                    int range = m_Rule.CheckHintCollum(Collums[CurrentCollum], IndexDeck[CurrentCollum], CurrentCollum);
                    pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP
                        - (range - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT;
                }
                //Debug.LogError(lenght - flip - CurrentCardsIndexDrag.Count + "leght " + " " + lenght + " " + flip);
                //Debug.LogError(CurrentCollum);
                return pos;
            }
            else
            {
                Vector3 pos = PosCollums[collum];
                int flip = CardFlip[collum];
                int lenght = Collums[collum].Count;
                if (!CloseCollums[collum])
                {
                    pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT;
                }
                else
                {
                    //Debug.LogError(collum + " " + CloseCollums[collum]);
                    int range = m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum);
                    //Debug.LogError(range);
                    pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP
                        - (range - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP * 1.5f;
                }
                return pos;
            }
        }
        else
        {
            Vector3 pos = PosCollums[m_Result[0]];
            int flip = CardFlip[m_Result[0]];
            int lenght = Collums[m_Result[0]].Count;

            if (!CloseCollums[m_Result[0]])
                pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT;
            else
            {
                int range = m_Rule.CheckHintCollum(Collums[m_Result[0]], IndexDeck[m_Result[0]], m_Result[0]);
                pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP
                    - (range - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP * 1.5f;
            }
            return pos;
        }
    }

    public Vector3 ComputePosition(int collum, int range)
    {
        Vector3 pos = PosCollums[collum];
        int flip = CardFlip[collum];
        int lenght = Collums[collum].Count;

        if (!CloseCollums[collum])
            pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGHT;
        else
        {
            //Debug.LogError(collum + " " + CloseCollums[collum]);
            pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * (GameData.CARD_RATIO_HEIGH_UP);
        }
        return pos;
    }

    public Vector3 PositionClose(int collum, int range)
    {
        Vector3 pos = PosCollums[collum];
        int flip = CardFlip[collum];
        int lenght = Collums[collum].Count;

        if (!CloseCollums[collum])
            pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGHT;
        else
        {
            //Debug.LogError(collum + " " + CloseCollums[collum]);
            pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * (GameData.CARD_RATIO_HEIGH_UP);
            //if (flip == 0)
            //{
            //    pos.y += GameData.CARD_RATIO_HEIGH_UP * 1.5f;
            //}
        }
        return pos;
    }

    public Vector3 PosDesDraw(int collum)
    {
        Vector3 pos = PosCollums[collum];
        int flip = CardFlip[collum];
        int lenght = Collums[collum].Count;
        if (!CloseCollums[collum])
            pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip) * GameData.CARD_RATIO_HEIGHT;
        else
        {
            // Debug.LogError(collum + " " + CloseCollums[collum]);
            int range = m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum);
            pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP * 2.5f
            - (range) * GameData.CARD_RATIO_HEIGHT;
            if (flip == 0)
            {
                pos.y += GameData.CARD_RATIO_HEIGH_UP * 2;
            }
            else if (flip == 1)
            {
                pos.y += GameData.CARD_RATIO_HEIGH_UP * 0.75f;
            }
        }

        //if (!CloseCollums[collum])
        //{
        //    pos.y = pos.y - flip * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT;
        //}
        //else
        //{
        //    //Debug.LogError(collum + " " + CloseCollums[collum]);
        //    int range = m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum);
        //    //Debug.LogError(range);
        //    pos.y = pos.y - (flip) * (GameData.CARD_RATIO_HEIGH_UP) - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP
        //        - (range - CurrentCardsIndexDrag.Count) * GameData.CARD_RATIO_HEIGHT - (lenght - flip - range) * GameData.CARD_RATIO_HEIGH_UP * 1.5f;
        //}
        return pos;
    }

    public Vector2 GetPositionDraw()
    {
        return PositionDraw;
    }

    int IsFlipCard(int collum)
    {

        //int count = GameData.NUMBER_COLLUM;
        //for (int i = 1; i < count; i++)
        //{
        //    int _count = Collums[i].Count;
        //    Debug.Log(Collums[i].Count);
        //    Debug.Log(CardFlip[i]);
        //    if (_count < CardFlip[i] + 1)
        //    {
        //        if (CardFlip[i] > 0)
        //        {
        //            CardFlip[i] -= 1;
        //        }
        //        if (_count > 0)
        //        {
        //            Debug.LogError(i);
        //            Debug.LogError(IndexDeck[i][_count - 1]);
        //            return IndexDeck[i][_count - 1];
        //        }
        //    }

        int _count = Collums[collum].Count;
        if (_count < CardFlip[collum] + 1)
        {
            if (CardFlip[collum] > 0)
            {
                CardFlip[collum] -= 1;
            }
            if (_count > 0)
            {
                return IndexDeck[collum][_count - 1];
            }
        }
        return -1;
    }

    bool ChecAutokWin()
    {
        //Debug.Log("ChecAutokWin");
        int count = GameData.NUMBER_COLLUM;
        int sum = 0;
        for (int i = 0; i < count; i++)
        {
            sum += CardFlip[i];
        }
        if (sum == 0)
        {
            return true;
        }
        else
        {
            //Debug.Log(sum);
            return false;

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

    void CheckWin()
    {
        //int sum = 0;
        //int count = GameData.DESTINATION;
        //int totalcolum = GameData.NUMBER_COLLUM;
        //for (int i = 0; i < count; i++)
        //{

        //    sum += Collums[totalcolum + i].Count;
        //}
        ////Debug.Log("CheckWin" + "Total Card des" + sum);
        //if (sum == GameData.NUMBER_CARD)
        //{
        //    SceneManager.instance.PlayGameController.ShowWin();
        //    Table.m_Instance.BlockAllCard();
        //}
    }

    public bool CheckWinCard()
    {
        for(int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            if (Collums[i].Count != 0)
                return false;
        }
        return true;
    }

    public void HandleCheckHint()
    {
        Table.GetInstance().HideHint();
        // Debug.Log(CountHint);
        if (CountHint == 0)
        {
            RemoveListHint();
            int totalcollum = GameData.NUMBER_COLLUM;
            for (int i = 0; i < totalcollum; i++)
            {
                int index = CardFlip[i];
                int count = Collums[i].Count;

                for (int j = index; j < count; j++)
                {
                    if (m_Rule.CheckInCollum(Collums, j, i))
                    {
                        //Debug.LogWarning(i + " " + j);
                        if (m_Rule.CheckHintAllCollum(Collums, LastCardCollums, IndexDeck, Collums[i][j], IndexDeck[i][j], i, j))
                        {
                            Hint hintdes = new Hint();
                            m_Rule.ComputeHintDes(Collums, IndexDeck, LastCardCollums, Collums[i][j], IndexDeck[i][j], i, j, PosCollums, hintdes);

                            for (int k = j; k < count; k++)
                            {
                                hintdes.ListCard.Add(IndexDeck[i][k]);
                            }
                            m_Hint.Add(hintdes);


                        }
                    }
                }

            }
        }

        Table.GetInstance().HandleShowHint(m_Hint);




    }

    void RemoveListHint()
    {
        m_Hint.Clear();

        //CountHint = 0;
    }

    bool IsCardMove(Vector2 position)
    {
        float range = Mathf.Abs(position.x - CardOriginPosition.x);
        if ((range <= GameData.CARD_WIDTH / 2) && (m_Result.Count > 0))
            return true;
        else
            return false;
    }

    int UpdateCardIsTouch(int collum, int index)
    {

        int result = Collums[m_Result[0]].Count;
        //Debug.Log(result);
        Collums[m_Result[0]].AddRange(CurrentCardsValueDrag);
        //Debug.LogError(Table.m_Instance.GetListCardValue(CurrentCardDarg)[0]);
        IndexDeck[m_Result[0]].AddRange(CurrentCardsIndexDrag);
        // Debug.Log("Collum" + CurremtIndex + " " + Collums[CurremtIndex].Count  );
        //for(int i = 0; i < Collums[CurremtIndex].Count; i++)
        //{
        //    Debug.Log(Collums[CurremtIndex][i]);
        //}
        int range = Collums[collum].Count - index;
        Collums[collum].RemoveRange(index, range);
        IndexDeck[collum].RemoveRange(index, range);
        // Debug.Log("Collum" + collum + Collums[collum].Count);
        //for(int i = 0; i < Collums[collum].Count; i++)
        //{
        //    Debug.Log(Collums[collum][i]);
        //}
        //Debug.Log(result);
        // Debug.Log(result);
        return result;

    }

    public List<int> GetListCardAutoWin()
    {
        List<int> autowin = new List<int>();
        int count = GameData.NUMBER_COLLUM;
        for (int i = 0; i < count; i++)
        {
            int numbervalue = Collums[i].Count;
            if (numbervalue > 0)
                autowin.AddRange(Collums[i]);
        }

        return autowin;
    }

    public void Undo(bool isNormal = true)
    {
        int count = m_Undo.Count - 1;

        if (count >= 0)
        {
            //Debug.Log(m_Undo.Count);
            //SceneManager.instance.PlayGameController.TimePlay += 2;
            if (isNormal)
            {
                SceneManager.instance.PlayGameController.MoveCount += 1;
            }
            SceneManager.instance.PlayGameController.ActiveUI(false);
            Table.m_Instance.HideHint();
            int collum = m_Undo[count].Collum;
            int collumdes = m_Undo[count].CollumDes;
            bool flip = m_Undo[count].Isflip;
            int cardIndex = m_Undo[count].Index;
            if (collum < GameData.NUMBER_COLLUM)
            {

                if (m_Undo[count].IsGetCollum == false)
                {
                    Collums[m_Undo[count].Collum].AddRange(m_Undo[count].ListValue);
                    int indexDes = Collums[m_Undo[count].CollumDes].Count - m_Undo[count].ListIndex.Count;
                    Collums[m_Undo[count].CollumDes].RemoveRange(indexDes, m_Undo[count].ListValue.Count);
                    IndexDeck[m_Undo[count].Collum].AddRange(m_Undo[count].ListIndex);
                    IndexDeck[m_Undo[count].CollumDes].RemoveRange(indexDes, m_Undo[count].ListIndex.Count);
                    //ResetResult();

                    //CurrentCollum = 0;
                    if ((cardIndex - CardFlip[collum] == 1) && flip)
                    {
                        int cardflip = IndexDeck[collum][CardFlip[collum]];
                        // Debug.Log(cardflip);
                        CardFlip[collum] += 1;
                        Table.m_Instance.Undo(m_Undo[count], cardflip, true);
                        //Debug.Log("Current CardFlip " + CardFlip[collum] + " CardIndex " + cardIndex + " Collum " + collum);
                    }
                    else
                        Table.m_Instance.Undo(m_Undo[count], -1, false);
                }
                else
                {
                    bool FlipCardDes = m_Undo[count].IsflipCollumdes;
                    int range = 13 - m_Undo[count].Lenght;
                    int indexCardWin = CardsWin.Count - 13;
                    CardsWin.RemoveRange(indexCardWin, 13);
                    Collums[m_Undo[count].CollumDes].AddRange(m_Undo[count].ListValue.GetRange(0, range));
                    Collums[m_Undo[count].Collum].AddRange(m_Undo[count].ListValue.GetRange(range, m_Undo[count].Lenght));
                    IndexDeck[m_Undo[count].Collum].AddRange(m_Undo[count].ListIndex.GetRange(range, m_Undo[count].Lenght));
                    IndexDeck[m_Undo[count].CollumDes].AddRange(m_Undo[count].ListIndex.GetRange(0, range));
                    UpdateAllCollum();
                    ResetCurrentCardDrag();
                    if ((cardIndex - CardFlip[collum] == 1) && flip)
                    {
                        int cardflip = IndexDeck[collum][CardFlip[collum]];
                        // Debug.Log(cardflip);
                        CardFlip[collum] += 1;
                        if (m_Undo[count].IndexDes - CardFlip[collumdes] == 1 && (m_Undo[count].IsflipCollumdes))
                        {
                            int cardflipdes = IndexDeck[collumdes][CardFlip[collumdes]];
                            CardFlip[collumdes] += 1;
                            Table.m_Instance.Undo(m_Undo[count], cardflip, true, true, true, cardflipdes);
                        }
                        else
                            Table.m_Instance.Undo(m_Undo[count], cardflip, true, true);
                        //Debug.Log("Current CardFlip " + CardFlip[collum] + " CardIndex " + cardIndex + " Collum " + collum);
                    }
                    else
                    {
                        if (m_Undo[count].IndexDes - CardFlip[collumdes] == 1 && (m_Undo[count].IsflipCollumdes))
                        {
                            int cardflipdes = IndexDeck[collumdes][CardFlip[collumdes]];
                            CardFlip[collumdes] += 1;
                            Table.m_Instance.Undo(m_Undo[count], -1, false, true, true, cardflipdes);
                        }
                        else
                            Table.m_Instance.Undo(m_Undo[count], -1, false, true);

                    }
                }
            }
            else if (collum == GameData.NUMBER_COLLUM)
            {
                if (m_Undo[count].IsGetCollum == false)
                {
                    for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
                    {
                        int _leght = Collums[i].Count - 1;
                        Collums[i].RemoveAt(_leght);
                        IndexDeck[i].RemoveAt(_leght);
                    }
                    Table.m_Instance.Undo(m_Undo[count], -1, false);
                }
                else
                {
                    for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
                    {
                        if (i != collumdes)
                        {
                            int _leght = Collums[i].Count - 1;
                            Collums[i].RemoveAt(_leght);
                            IndexDeck[i].RemoveAt(_leght);
                        }
                    }
                    int indexCardWin = CardsWin.Count - 13;
                    CardsWin.RemoveRange(indexCardWin, 13);
                    Collums[collumdes].AddRange(m_Undo[count].ListValue.GetRange(0, 12));
                    IndexDeck[collumdes].AddRange(m_Undo[count].ListIndex.GetRange(0, 12));
                    // if ((cardIndex - CardFlip[collum] == 1) && flip)
                    //{
                    //    int cardflip = IndexDeck[collum][CardFlip[collum]];
                    //    // Debug.Log(cardflip);
                    //    CardFlip[collum] += 1;
                    //    if (m_Undo[count].IndexDes - CardFlip[collumdes] == 1 && (m_Undo[count].IsflipCollumdes))
                    //    {
                    //        int cardflipdes = IndexDeck[collumdes][CardFlip[collumdes]];
                    //        CardFlip[collumdes] += 1;
                    //        Table.m_Instance.Undo(m_Undo[count], cardflip, true, true, true, cardflipdes);
                    //    }
                    //    else
                    //        Table.m_Instance.Undo(m_Undo[count], cardflip, true, true);
                    //    //Debug.Log("Current CardFlip " + CardFlip[collum] + " CardIndex " + cardIndex + " Collum " + collum);
                    //}
                    //else

                    if (m_Undo[count].IndexDes - CardFlip[collumdes] == 1 && (m_Undo[count].IsflipCollumdes))
                    {
                        int cardflipdes = IndexDeck[collumdes][CardFlip[collumdes]];
                        CardFlip[collumdes] += 1;
                        Table.m_Instance.Undo(m_Undo[count], -1, false, true, true, cardflipdes);
                    }
                    else
                        Table.m_Instance.Undo(m_Undo[count], -1, false, true);

                }
                //Table.m_Instance.Undo(m_Undo[count],)

                //ResetResult();
                //CurrentCollum = 0;
                //if ((cardIndex - CardFlip[collum] == 1) && flip)
                //{
                //    int cardflip = IndexDeck[collum][CardFlip[collum]];
                //    // Debug.Log(cardflip);
                //    Table.m_Instance.Undo(m_Undo[count], cardflip, true);
                //    //Debug.Log("Current CardFlip " + CardFlip[collum] + " CardIndex " + cardIndex + " Collum " + collum);
                //    CardFlip[collum] += 1;

                //}
                //else


            }
            UpdateAllCollum();
            ResetCurrentCardDrag();
            m_Undo.RemoveAt(count);
        }
        LeanTween.delayedCall(0.2f, () =>
        {
            SceneManager.instance.PlayGameController.CheckAuto();
        });

        // DebugCurrentCollum();

    }

    public void AddUndo(int index, int collum, int collumdes, int value)
    {
        //Undo undo = new Undo(index, collum, collum, value);
        //m_Undo.Add(undo);
        //Debug.Log(m_Undo.Count);
        //    Debug.Log("UndoCount " + m_Undo.Count + "Index " +
        //m_Undo[m_Undo.Count - 1].Index + "Collum " +
        //m_Undo[m_Undo.Count - 1].Collum
        //+ "CollumDes " + m_Undo[m_Undo.Count - 1].CollumDes + "List " + TestListCardUndo());
    }

    public void AddUndo(int index, int collum, int collumdes, List<int> indexdeck, List<int> values)
    {
        int count = m_Undo.Count;
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            if (CheckCollum(i))
            {
                List<int> cards = new List<int>();
                List<int> indexs = new List<int>();
                cards.AddRange(Collums[i].GetRange(Collums[i].Count - 13, 12));
                indexs.AddRange(IndexDeck[i].GetRange(Collums[i].Count - 13, 12));
                HandleOnWinSuit(i);
                cards.AddRange(values);
                indexs.AddRange(indexdeck);
                bool flip = false;
                int _flip = IsFlipCard(i);
                if (_flip != -1)
                    flip = true;
                Undo undo = new Undo(index, collum, i, indexs, cards, true, flip, 0, CheckFlipCollumDes(i, Collums[i].Count - 13));
                CheckFlip(undo, i, Collums[i].Count - 13, true);
                //Debug.LogError(undo.IsflipCollumdes);
                m_Undo.Add(undo);
                break;
            }
        }
        if (m_Undo.Count == count)
        {
            Undo undo = new Undo(index, collum, collum, indexdeck, values);
            m_Undo.Add(undo);
        }
        //Undo undo = new Undo(index, collum, collum, indexdeck, values);

        //    Debug.Log("UndoCount " + m_Undo.Count + "Index " +
        //m_Undo[m_Undo.Count - 1].Index + "Collum " +
        //m_Undo[m_Undo.Count - 1].Collum
        //+ "CollumDes " + m_Undo[m_Undo.Count - 1].CollumDes + "List " + TestListCardUndo());
    }

    public List<int> GetAllCollums()
    {
        List<int> alls = new List<int>();
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            alls.AddRange(Collums[i]);
        }
        return alls;
    }

    public int[] GetArrayCardFlip()
    {
        return CardFlip;
    }

    void DebugListCardFlip()
    {
        int lenght = CardFlip.Length;
        string s = "Current CardFlip";
        for (int i = 0; i < lenght; i++)
        {
            s += CardFlip[i] + " ";
        }
        Debug.Log(s);
        Debug.Log("Count Undo" + m_Undo.Count);
    }

    public void ClearNumberCollums()
    {
        int number = GameData.NUMBER_COLLUM;
        for (int i = 0; i < number; i++)
        {
            Collums[i].Clear();
        }
    }

    public void ClearUndo()
    {
        m_Undo.Clear();
    }

    //public bool CheckLose()
    //{
    //    //int count = Table.m_Instance.GetListCarDrawed().Count;
    //    //int cardresult = Table.m_Instance.GetListCardResult().Count;
    //    //for (int i = 0; i < count; i++)
    //    //{
    //    //    if (m_Rule.CheckHintDrawCard(Table.m_Instance.GetListCarDrawed(), Collums, LastCardCollums, i))
    //    //        return false;
    //    //}
    //    //if (cardresult != 0)
    //    //    return false;
    //    //else return true;
    //}

    public List<int> GetListAttributeDesCollum()
    {
        List<int> list = new List<int>();
        int pos1 = Collums[GameData.NUMBER_COLLUM][0] % 4;
        list.Add(pos1);
        int pos2 = Collums[GameData.NUMBER_COLLUM + 1][0] % 4;
        list.Add(pos2);
        int pos3 = Collums[GameData.NUMBER_COLLUM + 2][0] % 4;
        list.Add(pos3);
        int pos4 = Collums[GameData.NUMBER_COLLUM + 3][0] % 4;
        list.Add(pos4);
        return list;
    }

    public void DrawCards(int[] cards, int cardsdrawCount)
    {
        int _currentIndex = GameData.NUMBER_CARDSDRAW + cardsdrawCount * GameData.NUMBER_COLLUM;
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            Collums[i].Add(cards[_currentIndex + i]);
            IndexDeck[i].Add((_currentIndex + i));
        }
        UpdateAllCollum();
    }

    public List<int> GetLenghAllCollums()
    {
        List<int> Lenght = new List<int>();
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            Lenght.Add(Collums[i].Count);
        }
        return Lenght;
    }

    public void CheckSuitInCollum(int collum)
    {
        if (CheckCollum(collum))
        {
            HandleOnWinSuit(collum);
        }
    }

    public bool CheckCollum(int collum)
    {
        if (Collums[collum].Count < 13)
        {
            //Debug.LogError(false);
            return false;
        }
        else
        {
            int _count = Collums[collum].Count;
            int _index = _count - 13;
            if (Collums[collum][_index] / 4 != 12)
            {

                return false;
            }
            else
            {
                if (Table.m_Instance.GetStateCard(IndexDeck[collum][_index]))
                {
                    for (int i = _index; i < _count - 1; i++)
                    {
                        if ((Collums[collum][i] / 4 - Collums[collum][i + 1] / 4 == 1) && (Collums[collum][i] % 4 == Collums[collum][i + 1] % 4))
                        {

                        }
                        else
                        {
                            //Debug.LogError(false);
                            return false;
                        }
                    }
                    //Debug.LogError(true);
                    return true;
                }
                return false;
            }
        }

    }

    public void HandleOnWinSuit(int collum)
    {
        int _index = IndexDeck[collum].Count - 13;
        List<int> _cards = IndexDeck[collum].GetRange(_index, 13);
        IndexDeck[collum].RemoveRange(_index, 13);
        Collums[collum].RemoveRange(_index, 13);
        int _flip = IsFlipCard(collum);
        Table.m_Instance.HandleWinSuit(_cards, _flip);
        ShowCardEnable(collum);
        _cards.Reverse();
        CardsWin.AddRange(_cards);

    }

    public List<int> GetListCardFlip()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            list.AddRange(IndexDeck[i]);
        }
        return list;
    }

    public int GetLenghtCollum(int collum)
    {
        return Collums[collum].Count;
    }

    public List<int> GetListCardWin()
    {
        return CardsWin;
    }

    public void ShowCardEnable()
    {
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            Debug.LogError(m_Rule.CheckInCollum(CardFlip[i], Collums[i]));
        }
    }

    public void ShowCardEnable(int collum)
    {

        int count = Collums[collum].Count;
        if (count == 0)
            return;
        int index = m_Rule.CheckInCollum(CardFlip[collum], Collums[collum]);

        List<int> enable = new List<int>();
        List<int> disable = new List<int>();
        int flip = CardFlip[collum];
        if (index == count)
        {
            if (index == CardFlip[collum])
            {
                enable.Add(IndexDeck[collum][count]);

            }
            else
            {
                enable.Add(IndexDeck[collum][count]);
                disable.AddRange(IndexDeck[collum].GetRange(flip, index - flip));

            }

        }
        else if (index < count)
        {
            enable.AddRange(IndexDeck[collum].GetRange(index, count - index));
            disable.AddRange(IndexDeck[collum].GetRange(flip, index - flip));
        }
        //Debug.LogError(collum);
        // Debug.LogWarning(m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum) + " " + count + "flip" + flip);
        if (count - m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum) == flip)
        {
            CloseCollums[collum] = false;
            //CloseCollum(collum);
        }
        //CloseCollum(collum);
        Table.m_Instance.ShowCardEnable(enable, disable);
        //
        CloseCollum(collum);

    }

    public void CloseCollum(int collum)
    {
        List<int> cards = new List<int>();
        int lenght = IndexDeck[collum].Count - CardFlip[collum];
        cards.AddRange(IndexDeck[collum].GetRange(CardFlip[collum], lenght));
        int range = m_Rule.CheckHintCollum(Collums[collum], IndexDeck[collum], collum);
        int index = Collums[collum].Count - lenght;
        //int range = Collums[collum].Count - indexClose;
        // Debug.LogWarning(lenght + " " + range);
        Table.m_Instance.CloseCollum(cards, lenght, range, collum, CloseCollums[collum]);
    }

    public Vector3 GetPositionFlip(int collum)
    {
        Vector3 pos = PosCollums[collum];
        pos.y -= CardFlip[collum] * GameData.CARD_RATIO_HEIGH_UP;
        return pos;
    }

    public string SaveGame()
    {
        string _collum = "";
        string _indexdeck = "";
        string _cardWin = "";
        string _undo = "";
        string _carflip = "";
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            int count = Collums[i].Count;
            for (int j = 0; j < count; j++)
            {
                if (j != count - 1)
                {
                    _collum += Collums[i][j] + "_";
                    _indexdeck += IndexDeck[i][j] + "_";
                }
                else
                {
                    _collum += Collums[i][j];
                    _indexdeck += IndexDeck[i][j];
                }
            }
            if (i != GameData.NUMBER_COLLUM - 1)
            {
                _collum += "-";
                _indexdeck += "-";
            }
            else
            {
                _collum += "*";
                _indexdeck += "*";
            }


        }
        int cardswin = CardsWin.Count;
        if (cardswin > 0)
            for (int i = 0; i < cardswin; i++)
            {
                if (i != cardswin - 1)
                    _cardWin += CardsWin[i] + "_";
                else
                    _cardWin += CardsWin[i] + "*";
            }
        else
        {
            _cardWin += "*";
        }

        int _count = m_Undo.Count;
        for (int i = 0; i < _count; i++)
        {
            if (i != _count - 1)
                _undo += JsonUtility.ToJson(m_Undo[i]) + "-";
            else
                _undo += JsonUtility.ToJson(m_Undo[i]);
        }
        _undo += "*";
        for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
        {
            if (i != GameData.NUMBER_COLLUM - 1)
                _carflip += CardFlip[i] + "_";
            else
                _carflip += CardFlip[i] + "*";
        }
        string time = SceneManager.instance.PlayGameController.TimePlay.ToString() + "*";
        string Move = SceneManager.instance.PlayGameController.MoveCount.ToString();
        _collum = string.Format("{0}{1}{2}{3}{4}{5}{6}", _collum, _indexdeck, _cardWin, _undo, _carflip, time, Move);

        return _collum;
    }

    public void LoadDataGame(string[] data)
    {
        Reset();
        string[] _collum = data[3].Split('-');
        string[] _indexdeck = data[4].Split('-');
        for (int i = 0; i < _collum.Length; i++)
        {
            // Debug.LogError(_collum.Length);
            if (_collum[i].Length > 0)
            {
                string[] _value = _collum[i].Split('_');
                string[] _index = _indexdeck[i].Split('_');
                for (int j = 0; j < _value.Length; j++)
                {
                    Collums[i].Add(int.Parse(_value[j]));
                    IndexDeck[i].Add(int.Parse(_index[j]));
                }
            }
        }
        string[] _cardwins = data[5].Split('_');
        int _cardswinLenght = _cardwins.Length;
        Table.m_Instance.NumberComplete = _cardswinLenght / 13;
        Table.m_Instance.NumberWinSuit = Table.m_Instance.NumberComplete;
        if (_cardswinLenght > 1)
        {
            for (int i = 0; i < _cardswinLenght; i++)
            {
                CardsWin.Add(int.Parse(_cardwins[i]));
            }
            Table.m_Instance.WinSuitLoadData(CardsWin);
            // Debug.LogError(CardsWin.Count);
        }

        string[] _undo = data[6].Split('-');
        int _countundo = _undo.Length;
        //Debug.LogError(_countundo);
        for (int i = 0; i < _countundo; i++)
        {
            Undo temp = JsonUtility.FromJson<Undo>(_undo[i]);
            //ebug.Log(temp);
            if (temp != null)
                m_Undo.Add(temp);
        }
        string[] _cardFlip = data[7].Split('_');
        for (int i = 0; i < _cardFlip.Length; i++)
        {
            CardFlip[i] = int.Parse(_cardFlip[i]);
        }
        UpdateAllCollum();
        SceneManager.instance.PlayGameController.TimePlay = int.Parse(data[8]);
        SceneManager.instance.PlayGameController.MoveCount = int.Parse(data[9]);
        Table.m_Instance.LoadCard(IndexDeck, CardFlip);
    }

    public List<int> GetDataAutoWin(bool isK = false)
    {
        List<int> data = new List<int>();
        if(isK)
        {
            for(int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                if(Collums[i].Count != 0 && (Collums[i][0]/4)%12 ==0)
                {
                    data.Add(IndexDeck[i][0]);
                }
            }
        }
        else
        {
            for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                if (Collums[i].Count != 0 && ((Collums[i][0] / 4) % 12 != 0 || (Collums[i][0]/4) == 0))
                {
                    data.Add(IndexDeck[i][0]);
                }
            }
            data.Sort();
        }
        return data;
    }

    public List<int> GetDataCardLastCollum(bool isK = false)
    {
        if (!isK)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                if (Collums[i].Count != 0 && ((Collums[i][0] / 4) % 12 != 0 || (Collums[i][0] / 4) == 0))
                {
                    result.AddRange(IndexDeck[i]);
                    
                }
            }
            return result;
        }
        else
        {
            List<int> result = new List<int>();
            for (int i = 0; i < GameData.NUMBER_COLLUM; i++)
            {
                
                if (Collums[i].Count != 0 && (Collums[i][0] / 4) % 12 == 0)
                {
                    result.AddRange(IndexDeck[i]);
                }
                else if (Collums[i].Count != 0 && ((Collums[i][0] / 4) % 12 != 0 || (Collums[i][0] / 4) == 0))
                {
                    result.AddRange(IndexDeck[i]);
                }
            }
            return result;

        }
    }

}



