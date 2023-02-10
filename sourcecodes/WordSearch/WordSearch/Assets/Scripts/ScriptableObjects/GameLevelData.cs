using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameLevelData : ScriptableObject
{

    [System.Serializable]
    public struct categoryRecord
    {
        public string categoryName;
        public List<BoardData> boardDatas;

    }

    public List<categoryRecord> datas;
}
