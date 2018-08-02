using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rectange
{

    public Vector2 Center = Vector2.zero;
    public float Radius = 10;
    List<Vector2> ListPos = new List<Vector2>();
    int index = 0;
    List<int>[] ListResult = new List<int>[4];
    bool isDraw = true;
    public float TimeMove = 0.1f;
    void InitPosition()
    {
        ListResult[0] = new List<int>();
        ListResult[1] = new List<int>();
        ListResult[2] = new List<int>();
        ListResult[3] = new List<int>();
        Vector2 pos1;
        pos1.x = Center.x;
        pos1.y = Center.y + Radius;
        ListPos.Add(pos1);
        Vector2 pos2;
        pos2.x = Center.x + Radius;
        pos2.y = Center.y;
        ListPos.Add(pos2);
        Vector2 pos3;
        pos3.x = Center.x;
        pos3.y = Center.y - Radius;
        ListPos.Add(pos3);
        Vector2 pos4;
        pos4.x = Center.x - Radius;
        pos4.y = Center.y;
        ListPos.Add(pos4);


    }

    public void DrawRectange(Card[] cards, int[] cardvalue)
    {
        //Debug.Log(isDraw);
        int length = cards.Length;
        if (!isDraw)
            return;
        if (index >= length)
        {
            MoveRect(cards);
            index = 0;
            return;
        }
        if (cards[index].Value % 4 == 0)
        {
            // Debug.Log("xxx");
            ListResult[0].Add(index);
            Vector3 temp = ListPos[0];
            temp.z = -index;
            int value = cardvalue[index];
            LeanTween.move(cards[value].gameObject, temp, TimeMove).setOnComplete(() =>
                {
                    index += 1;
                    DrawRectange(cards, cardvalue);
                });
            AudioController.instance.PlaySoundSortCard();
        }
        else if (cards[index].Value % 4 == 1)
        {
            ListResult[1].Add(index);
            Vector3 temp = ListPos[1];
            temp.z = -index;
            int value = cardvalue[index];
            LeanTween.move(cards[value].gameObject, temp, TimeMove).setOnComplete(() =>
                {
                    index += 1;
                    DrawRectange(cards, cardvalue);
                });
            AudioController.instance.PlaySoundSortCard();
        }
        else if (cards[index].Value % 4 == 2)
        {
            ListResult[2].Add(index);
            Vector3 temp = ListPos[2];
            temp.z = -index;
            int value = cardvalue[index];
            LeanTween.move(cards[value].gameObject, temp, TimeMove).setOnComplete(() =>
                {
                    index += 1;
                    DrawRectange(cards, cardvalue);
                });
            AudioController.instance.PlaySoundSortCard();
        }
        else
        {
            ListResult[3].Add(index);
            Vector3 temp = ListPos[3];
            temp.z = -index;
            int value = cardvalue[index];
            LeanTween.move(cards[value].gameObject, temp, TimeMove).setOnComplete(() =>
                {
                    index += 1;
                    DrawRectange(cards, cardvalue);
                });
            AudioController.instance.PlaySoundSortCard();
        }

    }

    void MoveRect(Card[] cards)
    {
        //Debug.Log(isDraw);
        int count = ListResult[0].Count;
        float time = 1;
        for (int i = 0; i < count; i++)
        {
            if (!isDraw)
                return;
            int index = ListResult[0][i];
            Vector3 temp = ListPos[0];
            time += i * 0.01f;
            temp.z = -i;
            LeanTween.move(cards[index].gameObject, temp, time).setEaseInOutQuad();
            AudioController.instance.PlaySoundSortCard();
        }
        time = 1;
        for (int i = 0; i < count; i++)
        {
            if (!isDraw)
                return;
            int index = ListResult[1][i];
            Vector3 temp = ListPos[3];
            time += i * 0.01f;
            temp.z = -i;
            LeanTween.move(cards[index].gameObject, temp, time).setEaseInOutQuad();
            AudioController.instance.PlaySoundSortCard();
        }
        time = 1;
        for (int i = 0; i < count; i++)
        {
            if (!isDraw)
                return;
            int index = ListResult[2][i];
            Vector3 temp = ListPos[2];
            time += i * 0.01f;
            temp.z = -i;
            LeanTween.move(cards[index].gameObject, temp, time).setEaseInOutQuad();
            AudioController.instance.PlaySoundSortCard();
        }
        time = 1;
        for (int i = 0; i < count; i++)
        {
            if (!isDraw)
                return;
            int index = ListResult[3][i];
            Vector3 temp = ListPos[1];
            time += i * 0.01f;
            temp.z = -i;
            if (i == count - 1)
            {
                LeanTween.move(cards[index].gameObject, temp, time).setEaseInOutQuad().setOnComplete(() =>
                       {
                           SceneManager.instance.PlayGameController.NewGame();
                           //Debug.Log("ANimationDOne");
                           ListResult[0].Clear();
                           ListResult[1].Clear();
                           ListResult[2].Clear();
                           ListResult[3].Clear();
                       });
                AudioController.instance.PlaySoundSortCard();
            }
            else
            {
                LeanTween.move(cards[index].gameObject, temp, time).setEaseInOutQuad();
                AudioController.instance.PlaySoundSortCard();
            }
        }

    }

    public void StopAnimation()
    {
        isDraw = false;
        //Debug.Log(isDraw);
    }

    public void Reset()
    {
        ListResult[0].Clear();
        ListResult[1].Clear();
        ListResult[2].Clear();
        ListResult[3].Clear();
        isDraw = true;
    }

    public Rectange()
    {
        InitPosition();
    }
}
