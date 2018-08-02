using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

    public GameObject Hint;

    public void ShowHint()
    {
        Hint.SetActive(true);
    }

    public void HideHint()
    {
        Hint.SetActive(false);
    }

}
