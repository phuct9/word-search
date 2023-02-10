using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{

    public GameData currentGameData;
    public GameLevelData levelData;


    void Awake()
    {
        selectSequentalBoardData();   
    }

    /** truoc method nay currentGameData.selectedBoardData = default
     */
    private void selectSequentalBoardData()
    {
        foreach(var data in levelData.datas)
        {
            if (data.categoryName == currentGameData.selectedCategoryName)
            {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);
                //boardIndex = 2;//test - load board index = 0
                if (boardIndex < data.boardDatas.Count)
                {
                    currentGameData.selectedBoardData = data.boardDatas[boardIndex];
                }
                else
                {
                    var randomIndex = Random.Range(0, data.boardDatas.Count);
                    currentGameData.selectedBoardData = data.boardDatas[randomIndex];
                }
            }
        }
    }
}
