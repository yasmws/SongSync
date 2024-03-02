using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMPro.TextMeshPro comboScoreText;
    public TMPro.TextMeshPro scoreText;
    public int comboScore;
    public int score = 0;
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }
    public void Hit()
    {
        comboScore += 1;
    }
    public void Miss()
    {
        comboScore = 0;
    }
    public void Point(int points = 1)
    {
        score += points;
    }
    private void Update()
    {
        scoreText.text = comboScore.ToString();
        comboScoreText.text = score.ToString();
    }
}