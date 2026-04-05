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
    public GameObject token;
    public string gameState = "active";

    void Update()
    {
        elapsedTime += Time.deltaTime;

        //int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime);

        if (gameState == "active")
        {
            maxTimer -= Time.deltaTime;
            maxTimer = Mathf.Max(maxTimer, 0);

            timerText.text = maxTimer.ToString("F1");
        }

        if (maxTimer <= 0)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        gameState = "over";
        Debug.Log("loser");
        token.SetActive(false);

        //EWAN THIS IS FOR YOU
        //this is the lose state put the lose screen and transition here

    }

    public void WinGame()
    {
        gameState = "over";
        Debug.Log("winner");
        token.SetActive(false);

        //this is the win state put the win screen and transition here

    }

    public void AddScore(int addedScore)
    {
        score +=1;
        UpdateText();

        if (score >= maxScore)
        {
            WinGame();
        }
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
