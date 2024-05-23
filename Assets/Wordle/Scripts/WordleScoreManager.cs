using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordleScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    public static WordleScoreManager Instance;
    public static int Score = 0;
    private int highScore;

    
    private void Awake()
    {
        Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
    }

    public void UpdateScore(int score)
    {
        Score = score;
        scoreText.text = "SCORE: " + score.ToString();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }
}
