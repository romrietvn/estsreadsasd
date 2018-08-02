using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public CanvasGroup Loading;
    public Image LoadingBar;
    //public Text LoadingText;
    public static string ID1 = "3242605394392359/7838681025";
    public static string ID2 = "3242605394392359/9315414221";
    public static string ID3 = "3242605394392359/7239750221";
    public static string ID4 = "3242605394392359/8716483422";
    bool IsLoadData = false;
    public void ShowLoading(bool isLoadData = false)
    {
        Loading.alpha = 1;
        IsLoadData = isLoadData;
        Loading.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
        LeanTween.value(0, 1, 1f).setOnUpdate(UpdateLoading).setOnComplete(HideLoading);
    }

    void UpdateLoading(float val)
    {
        LoadingBar.fillAmount = val;
        //LoadingText.text = ((int)(val * 100)).ToString(); ;
    }

    public void HideLoading()
    {
        Loading.alpha = 0;
        Loading.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
        if (!IsLoadData)
            SceneManager.instance.HomeController.ShowHome();
        else
            SceneManager.instance.PlayGameController.ShowPlayGame();
        //AudioController.instance.PlayBackGroundSound();
        SceneManager.instance.ShowBanner(true);
        SceneManager.instance.LoadNewIntertitialBanner();

    }
}
