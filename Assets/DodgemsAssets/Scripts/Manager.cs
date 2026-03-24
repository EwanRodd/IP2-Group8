using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public int score;
    public int maxScore;
    public TextMeshProUGUI scoreText;

    public void AddScore(int addedScore)
    {
        score +=1;
        UpdateText();
    }
    public void UpdateText()
    {
        scoreText.text = (score + "/" + maxScore);
    }

    private void Start()
    {
        UpdateText();
    }
}
