using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        public GameObject hostPrefab;
        public GameObject wrongPrefab;
    }

    public GameObject idleHost;

    [Header("Directional Input Data")]
    public InputButtonData[] inputButtons;
    public GameObject[] randoInput;

    [Header("Spawn Location")]
    public Transform previewSpawn;
    private GameObject currentSpawn;
    public Transform[] confettiSpawns;
    public Transform ExplosionSpawn;
    public Transform hostSpawn;
    private GameObject theHostSpawn;

    [Header("Feedback")]
    public GameObject confettiOne;
    public GameObject confettiTwo;
    public GameObject explosion;    

    [Header("UI")]
    public TMP_Text timeUpText;
    public TMP_Text wrongText;
    public TMP_Text introText;
    public Animator openingText;

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

    [Header("Curtains")]
    public Animator Curtain;
    public Animator introCurtain;
 
    [Header("Sound Effects")]
    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip crowdClap;

    private InputButtonData currentCorrectButton;

    void Start()
    {
        introText.gameObject.SetActive(false);
        timeUpText.gameObject.SetActive(false);
        wrongText.gameObject.SetActive(false);

        StartCoroutine(GameStartDelay());
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

                if (currentSpawn != null)

                    Destroy(currentSpawn);
                    Instantiate(currentCorrectButton.wrongPrefab, previewSpawn.position, currentCorrectButton.wrongPrefab.transform.rotation);

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

        if (theHostSpawn != null)

            Destroy(theHostSpawn);

        GameObject button = randoInput[Random.Range(0, randoInput.Length)];

        foreach (InputButtonData data in inputButtons)
        {
            if (data != null && data.arrow.name == button.name.Replace("(Clone)", ""))
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

        theHostSpawn = Instantiate(idleHost, hostSpawn.position, idleHost.transform.rotation);

        Curtain.ResetTrigger("Open");
        Curtain.SetTrigger("Close");        
        
        yield return new WaitForSeconds(previewDuration);

        Curtain.ResetTrigger("Close");
        Curtain.SetTrigger("Open");

        if (theHostSpawn != null)

            Destroy (theHostSpawn);

        theHostSpawn = Instantiate(currentCorrectButton.hostPrefab, hostSpawn.position, currentCorrectButton.hostPrefab.transform.rotation);

        yield return new WaitForSeconds(0.5f);
        showingPreview = false;

        currentSpawn = Instantiate(currentCorrectButton.timerPrefab, previewSpawn.position, currentCorrectButton.timerPrefab.transform.rotation);        

        currentSpawn.SetActive(true);
        theHostSpawn.SetActive(true);


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

            if (currentRound < totalRounds)
                SFXManager.instance.CrowdClapClip(crowdClap, transform, 1f);

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
            if (currentSpawn != null)

                Destroy(currentSpawn);

                Instantiate(currentCorrectButton.wrongPrefab, previewSpawn.position, currentCorrectButton.wrongPrefab.transform.rotation);

            if (explosion != null)
                Instantiate(explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);
            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);
            StartCoroutine(EndGame());
        }
    }

    IEnumerator GameStartDelay()
    {
        introText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        openingText.SetTrigger("Out");

        yield return new WaitForSeconds(1f);

        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        introText.gameObject.SetActive(false);

        SpawnRandomDirection();
    }

    IEnumerator NextRoundDelay()
    {
        yield return new WaitForSeconds(2f);

        Curtain.ResetTrigger("Open");
        Curtain.SetTrigger("Close");

        yield return new WaitForSeconds(delayBetweenRounds);

        if (currentSpawn != null)

            Destroy(currentSpawn);

        SpawnRandomDirection();
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
