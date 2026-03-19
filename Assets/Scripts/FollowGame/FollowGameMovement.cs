using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class FollowGameMovement : MonoBehaviour
{

    public float speed = 5f;

    public FollowGameTarget target;
    public float graceTime = 0.8f;

    float timer;
    public float winTimer = 10f;
    public bool winTimerActive = false;
    public TMP_Text warning;

    public GameObject confettiOne;
    public GameObject confettiTwo;
    public Transform[] confettiSpawns;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;

    public TMP_Text introText;
    public Animator openingText;
    public Animator introCurtain;

    public bool gameStarted = false;


    public bool gameDone = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winTimerActive = true;
        winTimer = 10f;
        warning.gameObject.SetActive(false);
        introText.gameObject.SetActive(false);

        StartCoroutine(GameStartDelay());
    }

    // Update is called once per frame

    void Update()
    {
        if (gameStarted)
        {
            PlayGame();
        }
    }

    void PlayGame()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0f);

        //Check to see if the game is done or not
        if (gameDone)
            return;

        //Check to see if the timer is still running
        if (winTimerActive)
        {
            winTimer -= Time.deltaTime;

            winTimer = Mathf.Max(winTimer, 0f);

            //Check to see if the player has lasted long enough, if so, stop the game and display confetti
            if (winTimer <= 0f)
            {
                if (confettiOne != null)
                    Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                if (confettiTwo != null)
                    Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                SFXManager.instance.PopClip(pop, transform, 0.5f);
                SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

                winTimerActive = false;
                gameDone = true;
                StartCoroutine(EndGame());
            }
        }
        transform.Translate(movement * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, target.transform.position);

        //Check if the player is inside the target
        if (distance <= target.circleRadius)
        {
            timer = 0f;
            warning.gameObject.SetActive(false);
        }
        //Check if the player is outside the circle, if so, start the grace time and display a warning
        else
        {
            timer += Time.deltaTime;
            warning.gameObject.SetActive(true);

            //Check to see if the player has exceded their grace time and lost
            if (timer >= graceTime)
            {
                PlayerFailed();
            }
        }
    }

    IEnumerator GameStartDelay()
    {
        introText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        openingText.SetTrigger("Out");
        yield return new WaitForSeconds(1f);

        introCurtain.SetTrigger("Starting");

        yield return new WaitForSeconds(1f);
        introText.gameObject.SetActive(false);

        gameStarted = true;
    }

    //Function to stop the game
    void PlayerFailed()
    {
        SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);
        gameDone = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.8f);

        SceneManager.LoadScene(2);
    }

}
