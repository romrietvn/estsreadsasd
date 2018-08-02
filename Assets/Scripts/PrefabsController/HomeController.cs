using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{

    public static HomeController instance;
    public Button m_Arcade;
    public Button m_Classis;
    public Button m_More;
    public CanvasGroup Alpha;
    public bool isPlayGame = false;

    void Awake()
    {
        instance = this;
        if (!Alpha)
            Alpha = GetComponent<CanvasGroup>();
		#if UNITY_IOS
			m_More.gameObject.SetActive(false);
		#endif
    }

    public void OnButtonArcadeClick()
    {
        AudioController.instance.PlayButton();
        SceneManager.instance.m_GameMode = SceneManager.GameMode.Arcade;
        HideHome();
        SceneManager.instance.MapController.ShowMap();
    }

    public void OnButtonClassisClick()
    {
        //Debug.Log("OnButtonClassisClick");
        AudioController.instance.PlayButton();
        SceneManager.instance.CurrentLevel = -1;
        if (SceneManager.instance.m_GameMode == SceneManager.GameMode.Classis)
        {
            HideHome();
            SceneManager.instance.PlayGameController.ShowPlayGame();
        }
        else
        {
            SceneManager.instance.m_GameMode = SceneManager.GameMode.Classis;
            HideHome();
            SceneManager.instance.PlayGameController.ShowPlayGame();
            SceneManager.instance.PlayGameController.NewGame();
        }

        //if (isPlayGame)
        
        //else
        //    SceneManager.instance.PlayGameController.StartGame();
        //PopupController.instance.ShowPopup(100);

    }

    IEnumerator WaitNewGame()
    {
        yield return new WaitForSeconds(1);

    }

    public void OnButtonMoreClick()
    {
        AudioController.instance.PlayButton();
#if UNITY_ANDROID
        Application.OpenURL("market://search?id=" + Application.companyName + "");
#endif
    }

    public void ShowHome(bool isplay = false)
    {
        isPlayGame = isplay;
        // Debug.Log(isPlayGame);
        Alpha.alpha = 1;
        Alpha.blocksRaycasts = true;
        gameObject.transform.localPosition = Vector2.zero;
    }

    public void HideHome()
    {
        Alpha.alpha = 0;
        Alpha.blocksRaycasts = false;
        gameObject.transform.localPosition = new Vector2(10000, 10000);
    }

    public void ButtonSettingClick()
    {
        AudioController.instance.PlayButton();
        HideHome();
        SceneManager.instance.MainSettingController.ShowMainSetting();
    }

    public void ButtonFbClick()
    {
        AudioController.instance.PlayButton();
        Application.OpenURL("https://m.facebook.com/tatawindstudio");
    }

    public void ButtonBackClick()
    {
        AudioController.instance.PlayButton();
        Application.Quit();
    }

    public void ButtonShareClick()
    {
        AudioController.instance.PlayButton();
    }

    public void ButtonRateClick()
    {
        AudioController.instance.PlayButton();
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier + "");
#endif
    }
}
