using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public bool gameHasEnded = true;

    public float endDelay = 1f;
    public PlayerLives playerLives;

    public GameObject WinLevelUI;
    public GameObject LoseLevelUI;

    public Transform[] confettiSpawns;
    public GameObject confettiOne;
    public GameObject confettiTwo;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;

    public Animator introCurtain;
    public Animator openingText;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;

    public void Start()
     {
        StartCoroutine(GameStartDelay());
     }

    public void Update()
    {
        if(playerLives.lives == 0)
        {
            StartCoroutine(LoseGame());
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

        yield return new WaitForSeconds(3f);

        openingText.SetTrigger("Out");

        yield return new WaitForSeconds(1f);

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
            Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

            SFXManager.instance.PopClip(pop, transform, 0.5f);
            SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

            yield return new WaitForSeconds(2.8f);

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
            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

            yield return new WaitForSeconds(2.8f);

            introCurtain.SetTrigger("Ending");
            introCurtain.ResetTrigger("Starting");

            yield return new WaitForSeconds(4f);
            FailSO.Value++;

            SceneManager.LoadScene(2);

        }
    }


}
