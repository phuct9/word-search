using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public delegate void EnableSquareSelection();
    public static EnableSquareSelection OnEnableSquareSelection;

    public static void EnableSquareSelectionMethod()
    {
        if (OnEnableSquareSelection != null)
            OnEnableSquareSelection();
    }

    public delegate void DisableSquareSelection();
    public static DisableSquareSelection OnDisableSquareSelection;

    public static void DisableSquareSelectionMethod()
    {
        if (OnDisableSquareSelection != null)
            OnDisableSquareSelection();
    }

    public delegate void SelectSquare(Vector3 position);
    public static SelectSquare OnSelectSquare;

    public static void SelectSquareMethod(Vector3 position)
    {
        if (OnSelectSquare != null)
            OnSelectSquare(position);
    }

    public delegate void CheckSquare(string letter,Vector3 squarePosition,int squareIndex);
    public static CheckSquare OnCheckSquare;

    public static void CheckSquareMethod(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (OnCheckSquare != null)
            OnCheckSquare( letter,  squarePosition,  squareIndex);
    }

    public delegate void ClearSelection();
    public static ClearSelection OnClearSelection;

    public static void ClearSelectionMethod()
    {
        if (OnClearSelection != null)
            OnClearSelection();
    }

    public delegate void CorrectWord(string word, List<int> squareIndexs);
    public static event CorrectWord OnCorrectWord;

    public static void CorrectWordMethod(string word,List<int> squareIndexs)
    {
        if (OnCorrectWord != null)
        {
            OnCorrectWord(word, squareIndexs);
        }
    }

    public delegate void BoardCompleted();
    public static BoardCompleted OnBoardCompleted;

    public static void BoardCompletedMethod()
    {
        if (OnBoardCompleted != null)
            OnBoardCompleted();
    }

    public delegate void UnlockNextCategory();
    public static UnlockNextCategory OnUnlockNextCategory;

    public static void UnlockNextCategorydMethod()
    {
        if (OnUnlockNextCategory != null)
            OnUnlockNextCategory();
    }


    public delegate void LoadNextLevel();
    public static LoadNextLevel OnLoadNextLevel;

    public static void LoadNextLevelMethod()
    {
        if (OnLoadNextLevel != null)
            OnLoadNextLevel();
    }

    public delegate void GameOver();
    public static GameOver OnGameOver;

    public static void OnGameOverMethod()
    {
        if (OnGameOver != null)
            OnGameOver();
    }

    public delegate void ToggleSoundFX();
    public static ToggleSoundFX OnToggleSoundFX;

    public static void ToggleSoundFXMethod()
    {
        if (OnToggleSoundFX != null)
            OnToggleSoundFX();
    }

}
