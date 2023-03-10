using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
   
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
        {
            value = PlayerPrefs.GetInt(name);
        }
        return value;
    }

    public static void SaveCategoryData(string categoryName,int currentIndex)
    {
        PlayerPrefs.SetInt(categoryName, currentIndex);
        PlayerPrefs.Save();
    }

    public static void ClearGameData(GameLevelData levelData)
    {
        foreach(var data in levelData.datas)
        {
            PlayerPrefs.SetInt(data.categoryName, -1);
        }
        PlayerPrefs.SetInt(levelData.datas[0].categoryName, 0);
        PlayerPrefs.Save();
        
    }



}
