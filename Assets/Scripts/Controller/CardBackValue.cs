using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardBackValue : MonoBehaviour
{
    public int IndexCard = 0;
    public GameObject BlockCard;
    public Image CardBack;

    public void IsCheckedCard(bool isChecked)
    {
        if(isChecked)
        {
           // AudioController.instance.PlayButton();
            BlockCard.SetActive(true);
        }
        else
        {
            BlockCard.SetActive(false);
        }
    }
}
