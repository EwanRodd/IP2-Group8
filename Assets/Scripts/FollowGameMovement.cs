using System.Threading;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class FollowGameMovement : MonoBehaviour
{

    public float speed = 5f;

    public FollowGameTarget target;
    public float graceTime = 0.3f;

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

        if (gameDone)
            return;

        if (winTimerActive)
        {
            winTimer -= Time.deltaTime;

            winTimer = Mathf.Max(winTimer, 0f);

            timerText.text = winTimer.ToString("F1");

            if (winTimer <= 0f)
            {
                if (confettiOne != null)
                    Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                if (confettiTwo != null)
                    Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                winTimerActive = false;
                gameDone = true;
                Debug.Log("Time's up!");
            }
        }
        transform.Translate(movement * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position,target.transform.position);

        if (distance <= target.circleRadius)
        {
            timer = 0f;
            warning.gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
            warning.gameObject.SetActive(true);

            if (timer >= graceTime)
            {
                PlayerFailed();
            }
        }
    }

    void PlayerFailed()
    {
        gameDone = true;
        Debug.Log("Game Over");
    }
}
