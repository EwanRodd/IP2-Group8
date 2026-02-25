using System.Collections;
using UnityEngine;
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

    [SerializeField] private float swapHeight = 0.5f;

    private int swapCount = 5;
    private float swapDuration = 0.5f;



    private Cup cupWithBall;
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    private void Start()
    {
        ApplyDifficulty();
        PlaceBallUnderRandomCup();
        StartCoroutine(ShuffleRoutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void ApplyDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                swapCount = easySwaps;
                swapDuration = easySwapSpeed;
                break;

            case Difficulty.Medium:
                swapCount = mediumSwaps;
                swapDuration = mediumSwapSpeed;
                break;

            case Difficulty.Hard:
                swapCount = hardSwaps;
                swapDuration = hardSwapSpeed;
                break;
        }
    }
    public void OnCupClicked(Cup clickedCup)
    {
        // Lock all cups immediately
        SetCupsInteractable(false);

        // Optional: check result
        bool correct = clickedCup == cupWithBall;
        
        if (correct)
        {
            winText.SetActive(true);
        }
        else
        {
            failText.SetActive(true);
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
        cupWithBall = cups[Random.Range(0, cups.Length)];

        Vector3 ballPosition = cupWithBall.transform.position;
        ballPosition.y -= 2f;
        ballPosition.z += 1;
        ball.position = ballPosition;
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSeconds(1f);

        cupWithBall.PlaceBall(ball);

        for (int i = 0; i < swapCount; i++)
        {
            (Cup a, Cup b) = GetTwoDifferentCups();
            yield return SwapCups(a, b);
        }

        ball.SetParent(null);

        SetCupsInteractable(true);
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
        int first = Random.Range(0, cups.Length);
        int second;

        do
        {
            second = Random.Range(0, cups.Length);
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

            cupA.transform.position = Vector3.Lerp(startA, startB, t) + Vector3.up * height;
            cupB.transform.position = Vector3.Lerp(startB, startA, t) + Vector3.up * height;

            yield return null;
        }

        cupA.transform.position = startB;
        cupB.transform.position = startA;
    }
}
