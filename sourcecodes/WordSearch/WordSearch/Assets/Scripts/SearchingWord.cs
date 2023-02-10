using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public Text displayedText;
    public Image crossLine;

    private string _word;



    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.OnCorrectWord += OnCorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= OnCorrectWord;
    }

    public void setWord(string word)
    {
        _word = word;
        displayedText.text = word;
    }

    private void OnCorrectWord(string word,List<int> squareIndexes)
    {
        if (word == _word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }

}
