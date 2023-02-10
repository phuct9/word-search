using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject searchingWordPrefab;
    private float offest = 0.0f;
    private int maxColumns = 3;
    private int maxRows = 4;

    private int _columns = 2;
    private int _rows;
    private int _wordsCount;
    private List<GameObject> _words = new List<GameObject>();

   private void Start()
    {
        _wordsCount = currentGameData.selectedBoardData.searchWords.Count;
        if (_wordsCount < _columns)
        {
            _rows = 1;
        }
        else
        {
            calculateColumnAndRowsNumber();
        }
        creatWordObjects();
        setWordsPosition();
    }

    private void setWordsPosition()
    {
        var trans = _words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = trans.rect.width * trans.transform.localScale.x + offest,
            y = trans.rect.height * trans.transform.localScale.y + offest
        };

        int col = 0;
        int row = 0;
        var startPosition = getFirstSquarePosition();
        foreach(var word in _words)
        {
            var positionX = startPosition.x + wordOffset.x * col;
            var postionY = startPosition.y - wordOffset.y * row;
            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, postionY);
            col++;
            if (col >= _columns)
            {
                col = 0;
                row++;
            }
        }
    }

    private Vector2 getFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, 0f);
        var squareRect = _words[0].GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(0f,0f);

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + offest;
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + offest;

        var shiftBy = (parentRect.rect.width - (squareSize.x * _columns)) / 2;
        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = (parentRect.rect.height - squareSize.y) / 2;
        return startPosition;
    }

    private void calculateColumnAndRowsNumber()
    {
        //do
        //{
        //    _columns++;
        //    _rows = _wordsCount / _columns;
        //} while (_rows >= maxRows);
        //if (_columns > maxColumns)
        //{
            _columns = maxColumns;
            _rows = _wordsCount / _columns;
        //}
        Debug.Log("MAX COLUMN = "+maxColumns);
    }

    private bool tryIncreaseColumnNumber()
    {
        _columns++;
        _rows = _wordsCount / _columns;

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsCount / _columns;
            return false;
        }
        if (_wordsCount % _columns > 0)
        {
            _rows++;
        }
        return true;
    }

    private void creatWordObjects()
    {
        var squareScale = getSquareScale(new Vector3(1, 1, 0));
        for(int index = 0; index < _wordsCount; index++)
        {
            _words.Add(Instantiate(searchingWordPrefab) as GameObject);
            _words[index].transform.SetParent(this.transform);
            _words[index].GetComponent<RectTransform>().localScale = squareScale;
            _words[index].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            _words[index].GetComponent<SearchingWord>().setWord(currentGameData.selectedBoardData.searchWords[index].Word);
        }
    }

    private Vector3 getSquareScale(Vector3 defaulScale)
    {
        var finalScale = defaulScale;
        var adjustment = 0.01f;
        while (shouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;
            if (finalScale.x<=0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool shouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();

        var squareSize = new Vector2(0,0);

        squareSize.x = squareRect.rect.width * targetScale.x + offest;
        squareSize.y = squareRect.rect.height * targetScale.y + offest;

        var totalSquaresHeight = squareSize.y * _rows;

        if (totalSquaresHeight > parentRect.rect.height)
        {
            while(totalSquaresHeight > parentRect.rect.height)
            {
                if (tryIncreaseColumnNumber())
                {
                    totalSquaresHeight = squareSize.y * _rows;
                }
                else
                {
                    return true;
                }
            }
        }
        var totalSquareWidth = squareSize.x * _columns;
        if (totalSquareWidth > parentRect.rect.width)
            return false;
        return false;
    }
    
}
