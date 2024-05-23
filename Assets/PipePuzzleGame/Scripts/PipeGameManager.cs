using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    [SerializeField] private GameObject pipeHolder;
    [SerializeField] private GameObject[] pipes;
    [SerializeField] private int correctPlacedPipesCount = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text remainingPipeText;
    [SerializeField] private TMP_Text remainingAttemptsText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private int totalPipes = 0;
    private int totalAttempts = 8;

    StringBuilder placedPipesSB = new StringBuilder();
    StringBuilder attemptsRemainingSB = new StringBuilder();

    private void Start()
    {
        totalPipes = pipeHolder.transform.childCount;
        pipes = new GameObject[totalPipes];
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i] = pipeHolder.transform.GetChild(i).gameObject;
        }
    }
    private void Update()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        placedPipesSB.Length = 0;
        attemptsRemainingSB.Length = 0;
        placedPipesSB.Append("Placed: " + correctPlacedPipesCount.ToString() + "/" + totalPipes.ToString());
        attemptsRemainingSB.Append("Attempts left: " + totalAttempts.ToString());
        remainingPipeText.text = placedPipesSB.ToString();
        remainingAttemptsText.text = attemptsRemainingSB.ToString();
    }
    public void CorrectPlacedPipe()
    {
        correctPlacedPipesCount++;
        totalAttempts = (totalAttempts <= 0) ? 0 : totalAttempts - 1;
        WinOrLoss();
    }
    public void WrongPlacedPipe()
    {
        correctPlacedPipesCount--;
        totalAttempts = (totalAttempts <= 0) ? 0 : totalAttempts - 1;
        WinOrLoss();
    }
    private void WinOrLoss()
    {
        if(correctPlacedPipesCount==totalPipes && totalAttempts == 0)
        {
            winPanel.SetActive(true);
            DisablePipes();
        }
        else if(correctPlacedPipesCount != totalPipes && totalAttempts == 0)
        {
            losePanel.SetActive(true);
            DisablePipes();
        }
    }
    private void DisablePipes()
    {
        for(int i=0; i< pipes.Length; i++)
        {
            pipes[i].SetActive(false);
        }
    }
}
