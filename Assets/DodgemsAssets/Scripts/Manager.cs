using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public int score;
    public int maxScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public float maxTimer;

    public float elapsedTime = 0f;
    public GameObject token;
    public string gameState = "over";

    public Animator woodenSign;

    public Animator introCurtain;
    public Transform[] confettiSpawns;
    public GameObject confettiOne;
    public GameObject confettiTwo;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip curtains;

    public Animator openingText;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;


    void Start()
    {
        StartCoroutine(GameStartDelay());
        UpdateText();
    }
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
            StartCoroutine(LoseGame());
        }
    }

    public IEnumerator LoseGame()
    {
        gameState = "over";
        Debug.Log("loser");
        token.SetActive(false);

        SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

        yield return new WaitForSeconds(2.8f);

        SFXManager.instance.CurtainClip(curtains, transform, 1f);
        introCurtain.SetTrigger("Ending");
        introCurtain.ResetTrigger("Starting");

        yield return new WaitForSeconds(4f);
        FailSO.Value++;

        SceneManager.LoadScene(2);

    }

    public IEnumerator WinGame()
    {
        gameState = "over";
        Debug.Log("winner");
        token.SetActive(false);

        Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);
        Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

        SFXManager.instance.PopClip(pop, transform, 0.5f);
        SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

        yield return new WaitForSeconds(2.8f);

        SFXManager.instance.CurtainClip(curtains, transform, 1f);
        introCurtain.SetTrigger("Ending");
        introCurtain.ResetTrigger("Starting");

        yield return new WaitForSeconds(4f);

        scoreSO.Value++;
        SceneManager.LoadScene(2);

    }

    IEnumerator GameStartDelay()
    {

        yield return new WaitForSeconds(2f);

        woodenSign.SetTrigger("In");

        yield return new WaitForSeconds(1f);

        openingText.SetTrigger("Out");

        yield return new WaitForSeconds(1f);

        woodenSign.SetTrigger("Out");
        woodenSign.ResetTrigger("In");

        yield return new WaitForSeconds(1f);

        SFXManager.instance.CurtainClip(curtains, transform, 1f);
        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        yield return new WaitForSeconds(2.8f);

        gameState = "active";
    }

    public void AddScore(int addedScore)
    {
        score +=1;
        SFXManager.instance.CoinClip(coin, transform, 1f);
        UpdateText();

        if (score >= maxScore)
        {
            StartCoroutine(WinGame());
        }
    }
    public void UpdateText()
    {
        scoreText.text = (score + "/" + maxScore);
    }

}
