using System.Threading;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class FollowGameMovement : MonoBehaviour
{

    public float speed = 5f;

    public FollowGameTarget target;
    public float graceTime = 0.8f;

    float timer;
    public float winTimer = 10f;
    public bool winTimerActive = false;
    public TMP_Text timerText;
    public TMP_Text warning;

    public GameObject confettiOne;
    public GameObject confettiTwo;
    public Transform[] confettiSpawns;


    public bool gameDone = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winTimerActive = true;
        winTimer = 10f;
        warning.gameObject.SetActive(false);
    }

    // Update is called once per frame

    void Update()
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

            timerText.text = winTimer.ToString("F1");

            //Check to see if the player has lasted long enough, if so, stop the game and display confetti
            if (winTimer <= 0f)
            {
                if (confettiOne != null)
                    Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                if (confettiTwo != null)
                    Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                winTimerActive = false;
                gameDone = true;
                Debug.Log("Win!");
            }
        }
        transform.Translate(movement * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position,target.transform.position);

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

    //Function to stop the game
    void PlayerFailed()
    {
        gameDone = true;
        Debug.Log("Game Over");
    }
}
