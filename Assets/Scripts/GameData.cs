using UnityEngine;
using System.Collections;
using System;

public static class GameData  {
    public static float SCREEN_WIDTH;
    public static float SCREEN_HEIGHT;
    public static float CARD_RATIO_SCALE;
    public static float CARD_WIDTH;
    public static float CARD_HEIGHT;
    public static float CARD_PADDING_WIDTH;
    public static float CARD_PADDING_HEIGHT = 0.2f;
    public static int NUMBER_CARDDRAW = 24;
    public static float CARDDRAW_RATIO;
    public static float CARDDRAW_PERCENT = 3;
    public static float CARD_RATIO_HEIGHT;
    public static float CARD_RATIO_HEIGH_UP;
    public static int LIMIT_CARDDRAWSHOW = 3;
    public static float DEPT = 60;
    public static float CARD_PERCENT = 2.8f; //30%
    public static float CARD_RATIO_WIDTH;
    public static int TOTAL_COLLUM = 10; // tong collum va des
    public static float CARD_SCALE;
    public static short NUMBER_COLLUM =10;
    public static readonly short DESTINATION = 4;
    public static readonly int NUMBER_CARD = 104;
    public static readonly int MAXDRAWS = 4;
    public static readonly int NUMBER_CARDSDRAW = 54;
    public static readonly int TOTALCARD_INCOLLUM = 28;
    public static float TIME_MOVEDRAW = 0.1f;
    public static int TIMERATE = 90;
    public static eMode MODE = eMode.Klondike;
    public static eModeDraw MODEDRAW = eModeDraw.OneCard;
    public static eScreen SCREEN;
    public static LeanTweenType MoveType;
    public static eCardControl ControlCard;
    public static eCardAction ActionCard;

    public enum eMode
    {
        Klondike,
        Spider
    }

    public enum eScreen
    {
        Portrait,
        Lanscape
    }

    public enum eCardControl
    {
        Init,
        Shuffer
    }
    
    public enum eCardAction
    {
        Move,
        Drag,
        MoveDes
    }

    public enum eModeDraw
    {
        OneCard,
        TwoCard,
        FourCard,
    }
}
