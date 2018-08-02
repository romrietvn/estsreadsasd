using UnityEngine;
using System.Collections;

public class UIManage : MonoBehaviour {

    public static UIManage m_Instance;
    public static UIManage GetInstance()
    {
        return m_Instance;
    }

    void Awake()
    {
        m_Instance = this;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowWin()
    {
        Debug.Log("ShowWin");
    }

    public void ShowAutoWin()
    {
        Debug.Log("ShowAutoWin");
    }

    public void CHIABAI()
    {
        Table.GetInstance().CHIABAI();
    }

    public void Reset()
    {
        Table.GetInstance().ResetTable();
    }

    public void NewGame()
    {

    }

    public void SetModeDraw(GameData.eModeDraw mode)
    {
        GameData.MODEDRAW = mode;
        Table.GetInstance().ResetTable();
    }
}
