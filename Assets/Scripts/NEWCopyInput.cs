using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NEWCopyInput : MonoBehaviour
{
    public enum InputType
    {
        Direction
    }

    [System.Serializable]
    public class InputButtonData
    {
        public string inputName;    
        public InputType inputType;
        public int axisDirection;
        public GameObject arrow;
        public GameObject successPrefab;
        public GameObject timerPrefab;
    }

    [Header("Directional Input Data")]
    public InputButtonData[] inputButtons;
    public GameObject[] randoInput;

    [Header("Spawn Location")]
    public Transform previewSpawn;
    private GameObject currentSpawn;
    public Transform[] confettiSpawns;
    public Transform ExplosionSpawn;

    [Header("Feedback")]
    public GameObject confettiOne;
    public GameObject confettiTwo;
    public GameObject explosion;    

    [Header("UI")]
    public TMP_Text timeUpText;
    public TMP_Text wrongText;

    [Header("Timer Settings")]
    public float previewDuration = 3f;
    public float timeLimit = 5f;

    private float timer;
    private bool timerActive = false;
    private bool showingPreview = false;
    private bool inputPhase = false;
    private bool gameDone = false;

    private float lastHorizontal;
    private float lastVertical;

    public float delayBetweenRounds = 3f;
    public int totalRounds = 3;
    private int currentRound = 0;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;

    private InputButtonData currentCorrectButton;

    void Start()
    {
        timeUpText.gameObject.SetActive(false);
        wrongText.gameObject.SetActive(false);

        SpawnRandomDirection();
    }

    void Update()
    {
        if (showingPreview || gameDone)
            return;

        
        if (timerActive)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f);

            if (timer <= 0f)
            {
                timerActive = false;
                inputPhase = false;
                gameDone = true;

                timeUpText.gameObject.SetActive(true);

                if (explosion != null)
                    Instantiate(explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);
                SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

                StartCoroutine(EndGame());

            }
        }

        if (inputPhase)
        {
            foreach (InputButtonData button in inputButtons)
            {
                float axisValue = Input.GetAxisRaw(button.inputName);

                float lastValue = button.inputName == "Horizontal" ? lastHorizontal : lastVertical;

                if (Mathf.Sign(axisValue) == button.axisDirection && Mathf.Abs(axisValue) > 0.5f && lastValue == 0)
                {
                    CheckInput(button);
                    break;
                }
            }
        }

        lastHorizontal = Input.GetAxisRaw("Horizontal");
        lastVertical = Input.GetAxisRaw("Vertical");
    }

    void SpawnRandomDirection()
    {
        if (currentSpawn != null)
        {
            Destroy(currentSpawn);
        }

        GameObject button = randoInput[Random.Range(0, randoInput.Length)];

        currentSpawn = Instantiate(button, previewSpawn.position, button.transform.rotation);

        currentSpawn.SetActive(true);

        foreach (InputButtonData data in inputButtons)
        {
            if (data != null &&
                data.arrow.name == button.name.Replace("(Clone)", ""))
            {
                currentCorrectButton = data;
                break;
            }
        }

        showingPreview = true;

        StartCoroutine(PreviewRoutine());
    }

    IEnumerator PreviewRoutine()
    {
        yield return new WaitForSeconds(previewDuration);

        if (currentSpawn != null)

            Destroy(currentSpawn);

        showingPreview = false;

        currentSpawn = Instantiate(currentCorrectButton.timerPrefab, previewSpawn.position, currentCorrectButton.timerPrefab.transform.rotation);

        timer = timeLimit;
        timerActive = true;
        inputPhase = true;
    }

    void CheckInput(InputButtonData pressed)
    {
        bool correct = pressed == currentCorrectButton;

        if (correct)
        {
            timerActive = false;
            inputPhase = false;
            currentRound++;

            previewDuration = previewDuration - 1.5f;

            if (currentSpawn != null)

                Destroy(currentSpawn);

            currentSpawn = Instantiate(currentCorrectButton.successPrefab, previewSpawn.position, currentCorrectButton.successPrefab.transform.rotation);

            if (currentRound >= totalRounds)
            {

                if (confettiOne != null)
                    Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                if (confettiTwo != null)
                    Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                SFXManager.instance.PopClip(pop, transform, 0.5f);
                SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);
                StartCoroutine(EndGame());
            }
            else
            {
                StartCoroutine(NextRoundDelay());
            }
        }
        else
        {
            inputPhase = false;
            timerActive = false;

            wrongText.gameObject.SetActive(true);

            if (explosion != null)
                Instantiate(explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);
            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);
            StartCoroutine(EndGame());
        }
    }

    IEnumerator NextRoundDelay()
    {

        yield return new WaitForSeconds(delayBetweenRounds);

        if (currentSpawn != null)

            Destroy(currentSpawn);

        timer = timeLimit;
        timerActive = true;
        inputPhase = true;

        SpawnRandomDirection();
    }
        
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(delayBetweenRounds);

        SceneManager.LoadScene(2);
    }

}
