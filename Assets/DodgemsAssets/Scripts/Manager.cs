using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public int score;
    public int maxScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public float maxTimer;

    public float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        //int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime);


        maxTimer -= Time.deltaTime;
        maxTimer = Mathf.Max(maxTimer, 0);

        timerText.text = maxTimer.ToString("F1");
    }

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
