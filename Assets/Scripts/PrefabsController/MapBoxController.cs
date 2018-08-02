using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapBoxController : MonoBehaviour {
    public GameObject Template;
    public List<LevelController> CurrentLevelList;
    public Text Level;
    public int CurrentLevel = 0;

    void Start()
    {
        CurrentLevelList.Add(Template.GetComponent<LevelController>());
        for (int i = 0; i < SceneManager.instance.LevelPerMap - 1;i++)
        {
            var obj = Instantiate(Template) as GameObject;
            obj.transform.SetParent(this.transform);
            obj.transform.localScale = Vector3.one;

            CurrentLevelList.Add(obj.GetComponent<LevelController>());
        }
    }

    public void InitData(int level, List<MapData> mapData)
    {
        
        Level.text = level.ToString();
        for (int i = 0; i < CurrentLevelList.Count;i++)
        {
            if(i < mapData.Count)
            {
                int x = (level-1) * SceneManager.instance.LevelPerMap + i;
                CurrentLevel = x;
                CurrentLevelList[i].InitData(mapData[i],x);
            }
            else
            {
                CurrentLevelList[i].gameObject.SetActive(false);
            }
        }
    }

   public void NextLevel()
    {
        int _level = SceneManager.instance.CurrentLevel %20;
        CurrentLevelList[_level].InitOnNextLevel();
    }

}
