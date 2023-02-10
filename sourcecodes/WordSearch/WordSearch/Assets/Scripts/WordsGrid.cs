using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    private float squareOffset = 0f;
    private float topPosition = 100;

    private List<GameObject> _squareList = new List<GameObject>();

    void Start()
    {
        spawnGridSquares();
        setSquarePosition();
    }

    /*set các square vào đúng vị trí
     */
    private void setSquarePosition()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTranform = _squareList[0].GetComponent<Transform>();
        var offset = new Vector2
        {
            x = (squareRect.width * squareTranform.localScale.x + squareOffset) / 100f,
            y = (squareRect.height * squareTranform.localScale.y + squareOffset) / 100f
            //x = 1,
            //y = 1
        };

        var starPosition = getFirstSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;
        foreach (var square in _squareList)
        {
            var positionX = starPosition.x + offset.x * columnNumber;
            var positionY = starPosition.y - offset.y * rowNumber;
            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;
            if (rowNumber >= currentGameData.selectedBoardData.rows)
            {
                columnNumber++;
                rowNumber = 0;
            }
        }
    }

    private Vector2 getFirstSquarePosition()
    {
        var starPosition = new Vector2(0f, 0f);
        GameObject obj = _squareList[0];
        var rect = obj.GetComponent<SpriteRenderer>().sprite.rect;
        var tran = obj.GetComponent<Transform>();
        var size = new Vector2 {
            x = rect.width * tran.localScale.x,
            y = rect.height * tran.localScale.y
        };
        int col1 = currentGameData.selectedBoardData.columns - 1;
        int row1 = currentGameData.selectedBoardData.rows - 1;
        var midWidthPosition = (col1 * size.x)/100f / 2f;
        var midWidthHeight = (row1 * size.y) / 100f / 2f;
        starPosition.x = (midWidthPosition > 0) ? -midWidthPosition : midWidthPosition;
        starPosition.y += midWidthHeight;
        return starPosition;
    }

    /*tạo ra 1 loạt các square từ data
     */
    private void spawnGridSquares()
    {
        if (currentGameData != null)
        {
            var squareScale = getSquareScale(new Vector3(10.5f, 10.5f, 0.1f));
            //phuc
            foreach(var squares in currentGameData.selectedBoardData.board)
            {
                foreach(string str in squares.row)
                {
                    var normalLetterData = alphabetData.alphabetNormal.Find(data => data.letter == str);
                    var selectLetterData = alphabetData.alphabetWrong.Find(data => data.letter == str);
                    var correcLetterData = alphabetData.alphabetHighlighted.Find(data => data.letter == str);

                    if (normalLetterData.image == null || selectLetterData.image == null)
                    {
                        Debug.LogError("All fields in your array should have some letters");
#if UNITY_EDITOR
                        //test - tu thoat game neu data chua dung va log error
                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
#endif
                    }
                    else
                    {
                        GameObject obj = Instantiate(gridSquarePrefab);
                        _squareList.Add(obj);
                        obj.GetComponent<GridSquare>().setSprite(normalLetterData, selectLetterData, correcLetterData);
                        obj.transform.SetParent(this.transform);
                        obj.GetComponent<Transform>().transform.localScale = squareScale;
                        obj.GetComponent<GridSquare>().setIndex(_squareList.Count - 1);

                    }
                     
                }
            }
        }
    }
    
    private Vector3 getSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;
        while (shouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;
            if (finalScale.x <= 0 || finalScale.y <= 0)
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
        var rect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var size = new Vector2(0f, 0f);
        var starPosition = new Vector2(0f, 0f);

        size.x = (rect.width * targetScale.x) + squareOffset;
        size.y = (rect.height * targetScale.y) + squareOffset;

        int col = currentGameData.selectedBoardData.columns;
        int row = currentGameData.selectedBoardData.rows;

        var w2 = col * size.x / 100f / 2f;
        var h2 = row * size.y / 100f / 2f;

        starPosition.x = (w2 > 0) ? -w2 : w2;
        starPosition.y = h2;

        return starPosition.x < -getHalfScreenWidth() || starPosition.y > topPosition;
    }

    private float getHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.0f * height) * Screen.width / Screen.height;
        return width / 2f;
    }
    
}
