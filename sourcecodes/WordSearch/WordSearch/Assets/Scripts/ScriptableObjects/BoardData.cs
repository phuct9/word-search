using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    [System.Serializable]
    public class SearchingWord
    {
        public string Word;
    }

    [System.Serializable]
    public class BoardRow
    {
        public int kichthuoc;
        public string[] row;

        public BoardRow(int size)
        {
            createRow(size);
        }

        public void createRow(int size)
        {
            this.kichthuoc = size;
            this.row = new string[size];
            for (int i = 0; i < size; i++)
                row[i] = "A";
            Debug.Log("BOARD y = " + size);

        }

        public void clearRow()
        {
            for(int i = 0; i < kichthuoc; i++)
            {
                row[i] = " ";
            }
        }

    }

    public void clearWithEmptyString()
    {
        for(int i = 0; i < columns; i++)
        {
            board[i].clearRow();
        }
    } 

    public void createNewBoard()
    {
        Debug.Log("BOARD x = " + columns);
        board = new BoardRow[columns];
        for(int i = 0; i < columns; i++)
        {
            board[i] = new BoardRow(rows);
        }
    }

    public float timeInSeconds;
    public int columns = 0;
    public int rows = 0;
    public BoardRow[] board;
    public List<SearchingWord> searchWords = new List<SearchingWord>();

}
