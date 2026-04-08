using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEngine.Rendering.DebugUI;

public class CupGameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ball;
    public GameObject winText;
    public GameObject failText;
    [SerializeField] private Cup[] cups;

    [Header("Difficulty")]
    [SerializeField] private Difficulty difficulty;

    [Header("Shuffle Settings")]
    [SerializeField] private int easySwaps = 3;
    [SerializeField] private int mediumSwaps = 6;
    [SerializeField] private int hardSwaps = 10;

    [SerializeField] private float easySwapSpeed = 0.7f;
    [SerializeField] private float mediumSwapSpeed = 0.5f;
    [SerializeField] private float hardSwapSpeed = 0.35f;

    [SerializeField] private float easyStopSpeed = 0.1f;
    [SerializeField] private float mediumStopSpeed = 0.05f;
    [SerializeField] private float hardStopSpeed = 0f;

    [SerializeField] private float swapHeight = 0.5f;

    private int swapCount = 5;
    private float swapDuration = 0.5f;
    private int delay = 3;
    private float stopSpeed;

    [Header("Confetti")]
    public Transform[] confettiSpawns;
    public GameObject confettiOne;
    public GameObject confettiTwo;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;

    private int selectedCup = 0;
    public bool selecting = false;
    public Animator introCurtain;

    public bool started = false;
    public Animator openingText;

    private Cup cupWithBall;

    [SerializeField]
    private FloatSO FailSO;

    [SerializeField]
    private FloatSO scoreSO;
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    private void Start()
    {
        StartCoroutine(GameStartDelay());

        if (started == true)
        {
            
        }
        

    }

    private float lastHorizontal = 0f;
    private void Update()
    {
        if (selecting)
        {
            float dir = Input.GetAxisRaw("Horizontal");

            if (dir != 0 && lastHorizontal == 0) // detects "press"
            {
                if (dir > 0 && selectedCup < 3)
                {
                    cups[selectedCup].BecomeCup();
                    selectedCup += 1;
                    cups[selectedCup].BecomeTilt();
                }
                else if (dir < 0 && selectedCup > 0)
                {
                    cups[selectedCup].BecomeCup();
                    selectedCup -= 1;
                    cups[selectedCup].BecomeTilt();
                }

                Debug.Log(selectedCup);
                
            }

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                Debug.Log("South button pressed");
                cups[selectedCup].OnMouseDown();
            }

            lastHorizontal = dir;
        }
    }
    private void ApplyDifficulty()
    {
        //just applying the difficulty variables
        switch (difficulty)
        {
            case Difficulty.Easy:
                swapCount = easySwaps;
                swapDuration = easySwapSpeed;
                stopSpeed = easyStopSpeed;
                break;

            case Difficulty.Medium:
                swapCount = mediumSwaps;
                swapDuration = mediumSwapSpeed;
                stopSpeed = mediumStopSpeed;
                break;

            case Difficulty.Hard:
                swapCount = hardSwaps;
                swapDuration = hardSwapSpeed;
                stopSpeed = hardStopSpeed;
                break;
        }
    }
    public void OnCupClicked(Cup clickedCup)
    {
        SetCupsInteractable(false);

        bool correct = clickedCup == cupWithBall;
        
        if (correct)
        {
            winText.SetActive(true);

            if (confettiOne != null)
                Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

            if (confettiTwo != null)
                Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

            SFXManager.instance.PopClip(pop, transform, 0.5f);
            SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

            scoreSO.Value++;
            StartCoroutine(EndGame());
        }
        else
        {
            failText.SetActive(true);
            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

            FailSO.Value++;
            StartCoroutine(EndGame());
        }

        StartCoroutine(RevealAllAfterDelay(1, clickedCup));
    }
    private IEnumerator RevealAllAfterDelay(float delay, Cup clickedCup)
    {
        yield return new WaitForSeconds(delay);

        foreach (Cup cup in cups)
        {
            if (cup != clickedCup)
            {
            cup.MoveVertical(cup.moveDistance);
            }
        }
    }

    private void PlaceBallUnderRandomCup()
    {
        cupWithBall = cups[UnityEngine.Random.Range(0, cups.Length)];

        Vector3 ballPosition = cupWithBall.transform.position;
        ballPosition.y -= 2f;
        ballPosition.z += 1;
        ball.position = ballPosition;
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSeconds(2f);

        cupWithBall.PlaceBall(ball);

        for (int i = 0; i < swapCount; i++)
        {
            (Cup a, Cup b) = GetTwoDifferentCups();
            a.BecomeHand();
            b.BecomeHand();
            yield return SwapCups(a, b);

            yield return new WaitForSeconds(stopSpeed);
            //stopSpeed is the buffer when a cup gets swapped, and then stays like that for a bit before moving to the next swap
            a.BecomeCup();
            b.BecomeCup();
        }

        ball.SetParent(null);

        SetCupsInteractable(true);

        selecting = true;
        Array.Sort(cups, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        cups[selectedCup].BecomeTilt();

    }
    public void SetCupsInteractable(bool value)
    {
        foreach (Cup cup in cups)
        {
            cup.SetInteractable(value);
        }
    }

    private (Cup, Cup) GetTwoDifferentCups()
    {
        int first = UnityEngine.Random.Range(0, cups.Length);
        int second;

        do
        {
            second = UnityEngine.Random.Range(0, cups.Length);
        }
        while (second == first);

        return (cups[first], cups[second]);
    }

    private IEnumerator SwapCups(Cup cupA, Cup cupB)
    {
        Vector3 startA = cupA.transform.position;
        Vector3 startB = cupB.transform.position;

        float elapsed = 0f;

        while (elapsed < swapDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swapDuration;

            float height = Mathf.Sin(t * Mathf.PI) * swapHeight;

            cupA.transform.position = Vector3.Lerp(startA, startB, t) + ( Vector3.up * height );
            //cupA.transform.position = new Vector3(cupA.transform.position.x, cupA.transform.position.y + 0.66f, cupA.transform.position.z);
            cupB.transform.position = Vector3.Lerp(startB, startA, t) + Vector3.up * height;
            //cupB.transform.position = new Vector3(cupB.transform.position.x, cupB.transform.position.y + 0.66f, cupB.transform.position.z);

            yield return null;
        }

        cupA.transform.position = startB;
        cupB.transform.position = startA;
    }

    IEnumerator GameStartDelay()
    {

        yield return new WaitForSeconds(3f);

        openingText.SetTrigger("Out");

        yield return new WaitForSeconds(1f);

        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        yield return new WaitForSeconds(3f);

        started = true;

        foreach (Cup cup in cups)
        {
            cup.MoveVertical(-cup.moveDistance);
        }

        ApplyDifficulty();
        PlaceBallUnderRandomCup();
        StartCoroutine(ShuffleRoutine());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.8f);

        introCurtain.SetTrigger("Ending");
        introCurtain.ResetTrigger("Starting");

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(2);
    }
}
