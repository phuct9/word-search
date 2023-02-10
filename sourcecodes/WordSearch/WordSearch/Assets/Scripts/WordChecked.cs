using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordChecked : MonoBehaviour
{

    public GameData currentGameData;
    public GameLevelData gameLevelData;
    private string _word;

    private int _assignedPoints = 0;
    private int _completedWords = 0;
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += clearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;

    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= clearSelection;
        GameEvents.OnLoadNextLevel -= LoadNextGameLevel;
    }

    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }


    public void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = squarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;
            _rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y),new Vector2(0,1));
            _rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0, -1));
            _rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 0));
            _rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 0));
            _rayDiagonalLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 1));
            _rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, -1));
            _rayDiagonalRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 1));
            _rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, -1));
        }else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = selectRay(_rayStartPosition,squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            checkedWord();

        }
        else
        {
            if (isPointOnTheRay(_currentRay, squarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                _word += letter;
                checkedWord();
            }
        }
        _assignedPoints++;
        GameEvents.SelectSquareMethod(squarePosition);
        checkedWord();
    }

    private void checkedWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.searchWords)
        {
            if (_word == searchingWord.Word)
            {
                GameEvents.CorrectWordMethod(_word,_correctSquareList);
                _completedWords++;
                _word = string.Empty;
                _correctSquareList.Clear();
                checkBoardCompleted();
                return;
            }
        }
    }

    private bool isPointOnTheRay(Ray currentRay,Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100);
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
            {
                return true;
            }
        }
        return false;
    }

    private Ray selectRay(Vector2 firstPosition,Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }
        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y + 1f) < tolerance)
        {
            return _rayDown;
        }
        if (Mathf.Abs(direction.y) < tolerance && Mathf.Abs(direction.x - 1f) < tolerance)
        {
            return _rayRight;
        }
        if (Mathf.Abs(direction.y) < tolerance && Mathf.Abs(direction.x + 1f) < tolerance)
        {
            return _rayRight;
        }
        if (direction.x<0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }
        if (direction.x<0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }
        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }
        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }
        return _rayDown;
    }
    

    void Start()
    {
        _assignedPoints = 0;
        _completedWords = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (_assignedPoints > 0 && Application.isEditor)
        if (_assignedPoints > 0)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);
            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);
        }   
    }

    private void clearSelection()
    {
        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }

    private void checkBoardCompleted()
    {
        bool loadNextCategory = false;
        if (currentGameData.selectedBoardData.searchWords.Count == _completedWords)
        {
            //save current level progress
            var categoryName = currentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;
            for(int index = 0;index < gameLevelData.datas.Count; index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(gameLevelData.datas[index].categoryName);
                    readNextLevelName = false;
                }
                if (gameLevelData.datas[index].categoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;
                }
            }
            var currentLevelSize = gameLevelData.datas[currentCategoryIndex].boardDatas.Count;
            if (currentBoardIndex < currentLevelSize)
                currentBoardIndex += 1;
            DataSaver.SaveCategoryData(categoryName,currentBoardIndex);

            //Unlock Next Category
            if (currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;
                if (currentCategoryIndex < gameLevelData.datas.Count)
                {
                    categoryName = gameLevelData.datas[currentCategoryIndex].categoryName;
                    currentBoardIndex = 0;
                    loadNextCategory = true;
                    if (nextBoardIndex <= 0)
                    {
                        DataSaver.SaveCategoryData(categoryName,currentBoardIndex);
                    }
                }
                else
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod();
            }
            if (loadNextCategory)
            {
                GameEvents.UnlockNextCategorydMethod();
            }

        }
    }
}
