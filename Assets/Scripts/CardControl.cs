using UnityEngine;
using System.Collections;

public class CardControl
{

    public CardControl(int[] cards, GameData.eCardControl control)
    {
        if (control == GameData.eCardControl.Init)
            InitCardValue(cards);
        else
            ShufferCards(cards);
    }

    //public CardControl(Card card, Card[] cards)
    //{
    //  //CloneCard(card, cards);
    //}
    public void InitCardValue(int[] cards)
    {
        int numbercard = cards.Length;
        for (int i = 0; i < numbercard; i++)
        {
            cards[i] = i;
        }

    }

  


    public void ShufferCards(int[] cards)
    {
        int numbercard = cards.Length;
        for (int i = 0; i < numbercard - 1; i++)
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


}
