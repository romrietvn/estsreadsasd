using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animation
{

    float size = 6;
    Vector3 originScale = Vector3.zero;

    void CreateRect(Card[] cards, List<int> indexs)
    {
        SceneManager.instance.ChangeRender(false);
        originScale = cards[0].transform.localScale;
        foreach (var item in cards)
        {
            item.transform.localScale = originScale * 1.2f;
        }
        Vector3 Center = Vector3.zero;
        Vector3 Top = new Vector3(0, size * GameData.CARD_HEIGHT / 2, 0);
        Vector3 Bot = new Vector3(0, -size * GameData.CARD_HEIGHT / 2, 0);
        Vector3 Left = new Vector3(-size * GameData.CARD_WIDTH / 1.5f, 0);
        Vector3 Right = new Vector3(size * GameData.CARD_WIDTH / 1.5f, 0);
        int lenght = GameData.NUMBER_CARD;
        for (int i = 0; i < lenght; i++)
        {

            if (i / 13 < 2)
            {
                Vector3 pos = Left;
                pos.y += ((i % 13) - size) * (GameData.CARD_HEIGHT / 2);
                pos.z = -i;
                LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW +(i % 13) * (0.03f));
                AudioController.instance.PlaySoundSortCard();
            }
            else if (i / 13 < 4)
            {
                Vector3 pos = Top;
                pos.x += ((i % 13) - size) * (GameData.CARD_WIDTH / 1.5f);
                pos.z = -i;
                LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW +(i % 13) * (0.03f));
                AudioController.instance.PlaySoundSortCard();
            }
            else if (i / 13 < 6)
            {
                Vector3 pos = Bot;
                pos.x += ((i % 13) - size) * (GameData.CARD_WIDTH / 1.5f);
                pos.z = -i;
                LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW +(i % 13) * (0.03f));
                AudioController.instance.PlaySoundSortCard();
            }
            else
            {
                Vector3 pos = Right;
                pos.y += ((i % 13) - size) * (GameData.CARD_HEIGHT / 2);
                pos.z = -i;
                if (i != lenght - 1)
                {
                    LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.03f));
                    AudioController.instance.PlaySoundSortCard();
                }
                else
                {
                    LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.03f)).setOnComplete(() =>
                          {
                              Resever(cards, indexs);
                          });
                    AudioController.instance.PlaySoundSortCard();
                }
            }
        }
    }

    public Animation(Card[] cards, List<int> indexs)
    {
        CreateRect(cards, indexs);
    }

    void Resever(Card[] cards, List<int> indexs)
    {
        Vector3 Center = Vector3.zero;
        Vector3 Top = new Vector3(0, size * GameData.CARD_HEIGHT / 2, 0);
        Vector3 Bot = new Vector3(0, -size * GameData.CARD_HEIGHT / 2, 0);
        Vector3 Left = new Vector3(-size * GameData.CARD_WIDTH / 1.5f, 0);
        Vector3 Right = new Vector3(size * GameData.CARD_WIDTH / 1.5f, 0);
        int lenght = GameData.NUMBER_CARD;
        for (int i = 0; i < lenght; i++)
        {

            if (i / 13 < 2)
            {
                if (i / 13 == 1)
                {
                    Vector3 pos = Right;
                    pos.y += ((i % 13) - size) * (GameData.CARD_HEIGHT / 2);
                    //pos.z = -i;
                    LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.08f)).setEaseInOutQuad();
                    AudioController.instance.PlaySoundSortCard();
                }
            }
            else if (i / 13 < 4)
            {
                if (i / 13 == 3)
                {
                    Vector3 pos = Bot;
                    pos.x += ((i % 13) - size) * (GameData.CARD_WIDTH / 1.5f);
                    //pos.z = -i;
                    LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.08f)).setEaseInOutQuad();
                    AudioController.instance.PlaySoundSortCard();
                }
            }
            else if (i / 13 < 6)
            {
                if (i / 13 == 5)
                {
                    Vector3 pos = Top;
                    pos.x += ((i % 13) - size) * (GameData.CARD_WIDTH / 1.5f);
                    //pos.z = -i;
                    LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i%13) * (0.08f)).setEaseInOutQuad();
                    AudioController.instance.PlaySoundSortCard();
                }
            }
            else
            {
                if (i / 13 == 7)
                {
                    Vector3 pos = Left;
                    pos.y += ((i % 13) - size) * (GameData.CARD_HEIGHT / 2);
                    //pos.z = -i;
                    if (i != lenght - 1)
                    {
                        LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.08f)).setEaseInOutQuad();
                        AudioController.instance.PlaySoundSortCard();
                    }
                    else
                        LeanTween.move(cards[indexs[i]].gameObject, pos, GameData.TIME_MOVEDRAW + (i % 13) * (0.08f)).setEaseInOutQuad().setOnComplete(() =>
                    {
                        for (int j = 0; j < lenght; j++)
                        {
                            if (j != lenght - 1)
                            {
                                LeanTween.move(cards[indexs[j]].gameObject, Center, 2 * GameData.TIME_MOVEDRAW).setEaseInOutQuad();
                                AudioController.instance.PlaySoundSortCard();
                            }
                            else
                            {
                                LeanTween.move(cards[indexs[j]].gameObject, Center, 2 * GameData.TIME_MOVEDRAW).setEaseInOutQuad().setOnComplete(() =>
                                       {
                                           foreach (var item in cards)
                                           {
                                               item.transform.localScale = originScale;
                                           }
                                           SceneManager.instance.ChangeRender(true);
                                           SceneManager.instance.PlayGameController.NewGame();
                                       });
                                AudioController.instance.PlaySoundSortCard();
                            }
                        }
                    });
                }
            }
        }
    }
}
