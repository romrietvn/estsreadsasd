﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundValue : MonoBehaviour {

    public int IndexCard = 0;
    public GameObject BlockCard;
    public Image BG;

    public void IsCheckedCard(bool isChecked)
    {
        if (isChecked)
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
