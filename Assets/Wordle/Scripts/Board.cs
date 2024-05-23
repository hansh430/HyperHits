using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private Row[] rows;
    private int rowIndex;
    private int columnIndex;

    private string[] solutions;
    private string[] validWords;
    private string word;

    [Header("States")]
    public State emptyState;
    public State occupiedState;
    public State correctState;
    public State wrongSpotState;
    public State incorrectState;

    [Header("UI")]
    [SerializeField]private TMP_Text wrongWordText;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button newWordButton;
    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }
    private void OnEnable()
    {
        tryAgainButton.gameObject.SetActive(false);
        newWordButton.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        tryAgainButton.gameObject.SetActive(true);
        newWordButton.gameObject.SetActive(true);
    }
    private void Start()
    {
        LoadWordData();
        NewGame();
    }
    public void NewGame()
    {
        ClearBoard();
        SetRandomWord();
        enabled = true;
    }
    public void TryAgain()
    {
        ClearBoard();
        enabled = true;
    }
    private void LoadWordData()
    {
        TextAsset textFile = Resources.Load("official_wordle_all") as TextAsset;
        validWords = textFile.text.Split('\n');
        textFile = Resources.Load("official_wordle_common") as TextAsset;
        solutions = textFile.text.Split('\n');
    }
    private void SetRandomWord()
    {
        word = solutions[UnityEngine.Random.Range(0, solutions.Length)];
        word = word.ToLower().Trim();
    }
    private void Update()
    {

        Row currentRow = rows[rowIndex];
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter('\0');
            currentRow.tiles[columnIndex].SetState(emptyState);
            wrongWordText.gameObject.SetActive(false);

        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < KeyHelper.SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(KeyHelper.SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)KeyHelper.SUPPORTED_KEYS[i]);
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    columnIndex++;
                    break;
                }
            }
        }

    }

    private void SubmitRow(Row row)
    {
        if (!IsValidWord(row.Word))
        {
            wrongWordText.gameObject.SetActive(true);
            return;
        }
        string remaining = word;

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            if (tile.letter == word[i])
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!word.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
        }
        for(int i=0; i< row.tiles.Length; i++)
        {
            Tile tile= row.tiles[i];
            if(tile.state!=correctState && tile.state!=incorrectState)
            {
                if (remaining.Contains(tile.letter))
                {
                    tile.SetState(wrongSpotState);

                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(incorrectState);
                }
            }
        }
        if (HasWon(row))
        {
            Won();
        }

        rowIndex++;
        columnIndex = 0;

        if (rowIndex >= rows.Length)
        {
            Loss();
        }
    }
    private void ClearBoard()
    {
      //  Debug.Log("1");
        for(int row=0; row< rows.Length; row++)
        {
            for(int col=0; col < rows[row].tiles.Length; col++)
            {
                rows[row].tiles[col].SetLetter('\0');
                rows[row].tiles[col].SetState(emptyState);
            }
        }
        rowIndex = 0;
        columnIndex = 0;
    }
    private bool IsValidWord(string word)
    {
        for(int i=0; i<validWords.Length; i++)
        {
            if (validWords[i] == word)
            {
                return true;
            }
        }
        return false;
    }
    private bool HasWon(Row row)
    {
        for(int i=0; i<row.tiles.Length; i++)
        {
            if (row.tiles[i].state!=correctState) { return false; }
        }
        return true;
    }
    private void Won()
    {
        WordleScoreManager.Instance.UpdateScore(++WordleScoreManager.Score);
        tryAgainButton.interactable = false;
        enabled = false;
    }
    private void Loss()
    {
        tryAgainButton.interactable=true;
        enabled = false;
    }
}
