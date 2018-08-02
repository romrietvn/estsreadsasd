using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public CanvasGroup HomeMenu;

    public void ShowHome()
    {
        HomeMenu.alpha = 1;
        HomeMenu.blocksRaycasts = true;
        this.gameObject.transform.localPosition = Vector2.zero;
    }

    public void HideHome()
    {
        HomeMenu.alpha = 0;
        HomeMenu.blocksRaycasts = false;
        this.gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public void OnStartGameClick()
    {
        HideHome();
        SceneManager.instance.HomeController.ShowHome();
        //AudioController.instance.OffBackGroundSound();
    }

    public void OnSettingClick()
    {
        HideHome();
        SceneManager.instance.MainSettingController.ShowMainSetting();
    }
}
