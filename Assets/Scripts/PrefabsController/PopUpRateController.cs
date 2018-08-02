using UnityEngine;
using System.Collections;

public class PopUpRateController : MonoBehaviour {

    public CanvasGroup Alpha;
    private string RateURL;

    void Start()
    {
        
    }

    public void ShowPopUpRate()
    {
        Alpha.alpha = 1;
        Alpha.blocksRaycasts = true;
        this.gameObject.transform.localPosition = new Vector2(0, 150);
    }

    public void HidePopUpRate()
    {
        Alpha.alpha = 0;
        Alpha.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public void OnButtonNotNowClick()
    {
        AudioController.instance.PlayButton();
        HidePopUpRate();
        //SceneManager.instance.PlayGameController.ShowPlayGame();
        //SceneManager.instance.PlayGameController.ShowWin();
    }

    public void OnButtonRateClick()
    {
        AudioController.instance.PlayButton();
        HidePopUpRate();
        Application.OpenURL(SceneManager.instance.RateURL);
        //Debug.LogError("OnButtonRateClick");
    }
}
