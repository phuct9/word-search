using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    // Start is called before the first frame update
    private BoardData gameDataInstance => target as BoardData;
    private ReorderableList _dataList;

    private void OnEnable()
    {
        initializeReorableList(ref _dataList, "searchWords", "Searching Words");
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        serializedObject.Update();

        gameDataInstance.timeInSeconds = EditorGUILayout.FloatField("Max Game Time (In Seconds)",gameDataInstance.timeInSeconds);

        drawColumnsRowInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton(); 
       
        if (gameDataInstance.board != null && gameDataInstance.columns > 0 && gameDataInstance.rows > 0)
            drawBoardTable();

        GUILayout.BeginHorizontal();
        clearBoardButton();
        fillUpWithRandomLettersButton();
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        _dataList.DoLayoutList();
        
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gameDataInstance);
        }
    }

    private void drawColumnsRowInputFields()
    {
        var columnsTmp = gameDataInstance.columns;
        var rowsTmp = gameDataInstance.rows;
        var colNew = EditorGUILayout.IntField("Columns", gameDataInstance.columns);
        var rowNew = EditorGUILayout.IntField("Rows", gameDataInstance.rows);
        gameDataInstance.columns = colNew;
        gameDataInstance.rows = rowNew;
        //Debug.Log(columnsTmp+","+rowsTmp+":"+colNew+":"+rowNew);
        if ((columnsTmp!=colNew || rowsTmp != rowNew) && colNew>0 && rowNew > 0)
        {
            
            gameDataInstance.createNewBoard();
        }
    }

    private void drawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;
        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;
        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;
        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;
        var textFieldStyle = new GUIStyle();
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.BeginHorizontal(tableStyle);
        for(var x = 0; x < gameDataInstance.board.Length; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for(var y = 0; y < gameDataInstance.board[0].row.Length; y++)
            {
                if (x>=0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(gameDataInstance.board[x].row[y],textFieldStyle);
                    if (gameDataInstance.board[x].row[y].Length > 1)
                    {
                        character = gameDataInstance.board[x].row[y].Substring(0,1);
                    }
                    gameDataInstance.board[x].row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void initializeReorableList(ref ReorderableList list,string propertyName,string listLabel)
    {
        SerializedProperty tmp = serializedObject.FindProperty(propertyName);
        //Debug.Log("NAME = " + propertyName);
        //Debug.Log("TMP  = " + tmp.stringValue);
        list = new ReorderableList(serializedObject, tmp, true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };
        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            SerializedProperty tmp = element.FindPropertyRelative("Word");
            
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), tmp, GUIContent.none);
        };
    }

    private void ConvertToUpperButton()
    {
        if (GUILayout.Button("To Upper"))
        {
            for(var i = 0; i < gameDataInstance.columns; i++)
            {
                for(var j = 0; j < gameDataInstance.rows; j++)
                {
                    var erroCounter = Regex.Matches(gameDataInstance.board[i].row[j], @"a-z").Count;
                    if (erroCounter == 0)
                        gameDataInstance.board[i].row[j] = gameDataInstance.board[i].row[j].ToUpper();
                }
            }
            foreach(var searchWord in gameDataInstance.searchWords)
            {
                var erroCounter = Regex.Matches(searchWord.Word, @"a-z").Count;
                if (erroCounter == 0)
                {
                    searchWord.Word = searchWord.Word.ToUpper();
                }
                   
            }

        }
    }

    private void clearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            for(int i = 0; i < gameDataInstance.columns; i++)
            {
                for(int j = 0; j < gameDataInstance.rows; j++)
                {
                    gameDataInstance.board[i].row[j] = "";
                }
            }
        }
    }

    private void fillUpWithRandomLettersButton()
    {
        if (GUILayout.Button("Fill Up With Random")) {
            for(int i = 0; i < gameDataInstance.columns; i++)
            {
                for(int j = 0; j < gameDataInstance.rows; j++)
                {
                    int errorCounter = Regex.Matches(gameDataInstance.board[i].row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = UnityEngine.Random.Range(0,letters.Length);
                    if (errorCounter == 0)
                    {
                        gameDataInstance.board[i].row[j] = letters[index].ToString();
                    }
                    
                }
            }
        }
    }

}
