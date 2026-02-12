using UnityEngine;
using UnityEngine.UI; // REGULAR UI, not TMPro!

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText; // REGULAR Text, not TMP_Text
    private int score = 0;
    
    void Start()
    {
        // Make it obvious
        if (scoreText != null)
        {
            scoreText.text = "SCORE: 000000";
            scoreText.color = Color.yellow; // Bright color
            scoreText.fontSize = 24;
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }
    
    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score.ToString("D6");
        }
    }
}