using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using System.Threading;
//using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameHasEnded = true;
    public float lives = 3;

    public float endDelay = 1f;
    public DodgemPlayer DodgemPlayer;

    public GameObject WinLevelUI;
    public GameObject LoseLevelUI;

    public Transform[] confettiSpawns;
    public GameObject confettiOne;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip curtains;

    public Animator introCurtain;
    public Animator openingText;
    public Animator woodenSign;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;

    public float maxTimer;
    public float elapsedTime = 0f;
    public string gameState = "active";

    public Image[] livesUI;

    public void Start()
     {
        StartCoroutine(GameStartDelay());
     }

    public void Update()
    {
        if(lives == 0)
        {
            gameState = "over";
            StartCoroutine(LoseGame());
        }
        elapsedTime += Time.deltaTime;

        //int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime);

        if (gameState == "active")
        {
            maxTimer -= Time.deltaTime;
            maxTimer = Mathf.Max(maxTimer, 0);

        }

        if (maxTimer <= 0)
        {
            gameState = "over";
            StartCoroutine(WinGame());
        }
    }

    public void UpdateLives()
    {
        for (int i = 0; i < livesUI.Length; i++)
        {
            if (i < lives)
            {
                livesUI[i].enabled = true;
            }
            else
            {
                livesUI[i].enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name == "EndTrigger")
        {

            StartCoroutine(WinGame());
        }

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

        gameHasEnded = false;
    }
    public IEnumerator WinGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

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
    }

    public IEnumerator LoseGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            SFXManager.instance.ExplosionClip(explosion, transform, 0.5f);
            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

            yield return new WaitForSeconds(2.8f);

            SFXManager.instance.CurtainClip(curtains, transform, 1f);

            introCurtain.SetTrigger("Ending");
            introCurtain.ResetTrigger("Starting");

            yield return new WaitForSeconds(4f);
            FailSO.Value++;

            SceneManager.LoadScene(2);

        }
    }


}
