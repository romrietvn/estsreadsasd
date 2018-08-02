using UnityEngine;
using System.Collections;

public class CardsController
{


    public void ShufferCards(int[] cards)
    {
        int numbercard = cards.Length;
        for (int i = 0; i < numbercard - 1; i++)
        {
            int k = Random.Range(i + 1, numbercard - 1);
            Swap(ref cards[i], ref cards[k]);
        }
    }

    public void ShufferCards(int[] cards, int numbercomplete)
    {
        int index = numbercomplete * 13;
        int numbercard = cards.Length;
        for (int i = index; i < numbercard - 1; i++)
        {
            int k = Random.Range(i + 1, numbercard - 1);
            Swap(ref cards[i], ref cards[k]);
        }
    }

    private void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    public void InitCards(int[] cards, GameData.eModeDraw mode)
    {
        if (mode ==GameData.eModeDraw.OneCard) // random bich
        {
            for (int i = 0; i < 13; i++)
            {
                cards[i] = i*4;
                cards[i + 13] = i*4;
                cards[i + 26] = i*4;
                cards[i + 39] = i*4;
                cards[i + 52] = i*4;
                cards[i + 65] = i*4;
                cards[i + 78] = i*4;
                cards[i + 91] = i*4;
            }
            //string s = "";
            //for(int i = 0; i < cards.Length; i++)
            //{
            //    s += cards[i] + " ";
            //}
            //Debug.LogError(s);
        }
        else if (mode == GameData.eModeDraw.TwoCard) // random bich va co
        {
            for (int i = 0; i < 13; i++)
            {
                cards[i] = i*4;
                cards[i + 13] = i*4;
                cards[i + 26] = i*4;
                cards[i + 39] = i*4;
                cards[i + 52] = i * 4 + 2;
                cards[i + 65] = i * 4 + 2;
                cards[i + 78] = i * 4 + 2;
                cards[i + 91] = i * 4 + 2;
            }
            //string s = "";
            //for (int i = 0; i < cards.Length; i++)
            //{
            //    s += cards[i] + " ";
            //}
            //Debug.LogError(s);
        }
        else if (mode == GameData.eModeDraw.FourCard) // random bich, chuon, ro, co
        {
            for (int i = 0; i < 13; i++)
            {
                cards[i] = i;
                cards[i + 13] = i * 4 + 1;
                cards[i + 26] = i * 4 + 2;
                cards[i + 39] = i * 4 + 3;
                cards[i + 52] = i;
                cards[i + 65] = i * 4 + 1;
                cards[i + 78] = i * 4 + 2;
                cards[i + 91] = i * 4 + 3;
            }
            //string s = "";
            //for (int i = 0; i < cards.Length; i++)
            //{
            //    s += cards[i] + " ";
            //}
            //Debug.LogError(s);
        }
    }

    public CardsController(int[] cards, GameData.eModeDraw mode)
    {
        //Debug.LogError(mode);
        InitCards(cards, mode);
        ShufferCards(cards);
    }

    public CardsController(int[] cards)
    {
        ShufferCards(cards);
        //string s = "";
        //for (int i = 0; i < cards.Length; i++)
        //{
        //    s += cards[i] + " ";
        //}
        //Debug.LogError(s);
    }

    public CardsController(int[] cards, GameData.eModeDraw mode, int numbercomplete)
    {
        InitCards(cards, mode);
        ShufferCards(cards, numbercomplete);
    }
}
