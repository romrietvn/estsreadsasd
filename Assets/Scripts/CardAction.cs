using UnityEngine;
using System.Collections.Generic;
using System;


public class CardAction
{

    public void Move(Card card, Vector2 position)
    {
        LeanTween.move(card.gameObject, position, 0);
    }

    public void Move(Card[] cards, List<int> value, Vector2 position)
    {
        int count = value.Count;
        for (int i = 0; i < count; i++)
        {
            LeanTween.move(cards[value[i]].gameObject, position, 0);
        }
    }

    public void MoveToDes(Card card, Vector2 posititon, float time, LeanTweenType type, Action ondone)
    {
        LeanTween.move(card.gameObject, posititon, time).setEase(type).setOnComplete(ondone);
    }

    public void MoveToDes(Card[] cards, List<int> value, Vector2 position, float time, LeanTweenType type, Action ondone)
    {
        int count = value.Count;
        for (int i = 0; i < count; i++)
        {
            if (i != count - 1)
                LeanTween.move(cards[value[i]].gameObject, position, time).setEase(type);
            else
                LeanTween.move(cards[value[i]].gameObject, position, time).setEase(type).setOnComplete(ondone);
        }
    }

    public void MoveTo()
    {

    }

    public void Rotate(Card card)
    {

    }


}
