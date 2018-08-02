using UnityEngine;
using System.Collections;

public class RateGameController : MonoBehaviour {

	public void OnRateNow()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier + "");
#endif
        PlayerPrefs.SetInt(SceneManager.RATE_DATA, 1);
        gameObject.SetActive(false);
    }

    public void OnRemindMeLater()
    {
        PlayerPrefs.SetInt(SceneManager.RATE_DATA, 0);
        PlayerPrefs.SetInt(SceneManager.LEVEL_RATING, 0);
        gameObject.SetActive(false);
    }

    public void DontAskAgain()
    {
        PlayerPrefs.SetInt(SceneManager.RATE_DATA, 1);
        gameObject.SetActive(false);
    }
}
