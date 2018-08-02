using UnityEngine;
using System.Collections;
using UnityEditor;

public class Tool : MonoBehaviour
{
    [MenuItem("ToolClearData/ClearData")]
    private static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("ToolClearData/WinGame")]
    private static void WinGame()
    {
        SceneManager.instance.PlayGameController.ShowWin();
    }

    [MenuItem("ToolClearData/LoseGame")]
    private static void LoseGame()
    {
        SceneManager.instance.PlayGameController.ShowLose();
    }

    [MenuItem("ToolClearData/UnLockAllCardFace")]
    private static void UnLockAllCardFace()
    {
        PlayerPrefs.SetInt("CF_REWARD", 99);
    }

}
