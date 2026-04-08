using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
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
        public GameObject wrongPrefab;
    }

    public GameObject idleHost;

    [Header("Directional Input Data")]
    public InputButtonData[] inputButtons;
    public GameObject[] randoInput;

    [Header("Spawn Location")]
    public Transform[] previewSpawns;
    private GameObject[] currentSpawn;
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

    private int currentIndex = 0;

    public GameObject hostPrefab;

    public Animator woodenSign;

    [Header("Curtains")]
    public Animator Curtain;
    public Animator introCurtain;
 
    [Header("Sound Effects")]
    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip crowdClap;

    private InputButtonData[] currentCorrectButton;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;

    void Start()
    {
        timeUpText.gameObject.SetActive(false);
        wrongText.gameObject.SetActive(false);

        //Start the first round
        StartCoroutine(GameStartDelay());
    }

    void Update()
    {
        //Check to see if either the game has started or finished
        if (showingPreview || gameDone)
            return;

        //Check if the timer is still running
        if (timerActive)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f);

            //Check if the time has ran out
            if (timer <= 0f)
            {
                timerActive = false;
                inputPhase = false;
                gameDone = true;

                timeUpText.gameObject.SetActive(true);

                if (currentSpawn != null)

                    Destroy(currentSpawn[currentIndex]);
                Instantiate(currentCorrectButton[currentIndex].wrongPrefab, previewSpawns[currentIndex].position, currentCorrectButton[currentIndex].wrongPrefab.transform.rotation);

                if (explosion != null)
                    Instantiate(explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);
                SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

                FailSO.Value++;

                //End the game
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

    //Function to choose the 5 inputs
    void SpawnRandomDirection()
    {

        //Clear previous arrows
        ClearSpawns();

        if (theHostSpawn != null)

            Destroy(theHostSpawn);
       
        int inputCount = 5;
        currentIndex = 0;

        currentSpawn = new GameObject[inputCount];

        currentCorrectButton = new InputButtonData[inputCount];

        //Randomly select 5 inputs
        for (int i = 0; i < inputCount; i++)
        {
            GameObject button = randoInput[Random.Range(0, randoInput.Length)];

            //Assign each input so it matches the correct input
            foreach (InputButtonData data in inputButtons)
            {
                if (data != null && data.arrow.name == button.name.Replace("(Clone)", ""))
                {
                    currentCorrectButton[i] = data;
                    break;
                }
            }
        }

        showingPreview = true;

        StartCoroutine(PreviewRoutine());
    }

    //Function to show the player all the buttons that need pressed
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

        theHostSpawn = Instantiate(hostPrefab, hostSpawn.position,hostPrefab.transform.rotation);

        yield return new WaitForSeconds(0.5f);
        showingPreview = false;

        //Spawn all 5 arrows
        for (int i = 0; i < currentCorrectButton.Length; i++)
        {
            currentSpawn[i] = Instantiate(currentCorrectButton[i].timerPrefab,previewSpawns[i].position,currentCorrectButton[i].timerPrefab.transform.rotation);
        }

        theHostSpawn.SetActive(true);


        timer = timeLimit;
        timerActive = true;
        inputPhase = true;
    }

    //Function to check the users input
    void CheckInput(InputButtonData pressed)
    {
        //Check to see if the correct button was pressed
        bool correct = pressed == currentCorrectButton[currentIndex];

        //if the players input was correct
        if (correct)
        {
            
            if (currentSpawn != null)
                Destroy(currentSpawn[currentIndex]);

            //Spawn green/correct arrow
            currentSpawn[currentIndex] = Instantiate(currentCorrectButton[currentIndex].successPrefab,previewSpawns[currentIndex].position,currentCorrectButton[currentIndex].successPrefab.transform.rotation);

            currentIndex++;

            //check to see if 5 inputs have been pressed and all correct
            if (currentIndex >= currentCorrectButton.Length)
            {
                timerActive = false;
                inputPhase = false;
                currentRound++;

                previewDuration -= 1.5f;

                //Check to see the has not yet reached the final round
                if (currentRound < totalRounds)
                    SFXManager.instance.CrowdClapClip(crowdClap, transform, 1f);

                //Check to see if the player has beaten all 3 rounds
                if (currentRound >= totalRounds)
                {
                    Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);
                    Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                    SFXManager.instance.PopClip(pop, transform, 0.5f);
                    SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

                    scoreSO.Value++;

                    StartCoroutine(EndGame());
                }
                //Start delay for next round
                else
                {
                    StartCoroutine(NextRoundDelay());
                }
            }
            else
            {

            }
        }
        //If the players input was incorrect
        else
        {

            inputPhase = false;
            timerActive = false;

            wrongText.gameObject.SetActive(true);

            if (currentSpawn != null)
                Destroy(currentSpawn[currentIndex]);

            Instantiate(currentCorrectButton[currentIndex].wrongPrefab,previewSpawns[currentIndex].position,currentCorrectButton[currentIndex].wrongPrefab.transform.rotation);

            if (explosion != null)
                Instantiate(explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);

            SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.5f);

            FailSO.Value++;
            //End the game
            StartCoroutine(EndGame());
        }
    }


    //Function to clear all the arrows between rounds, as well as delete the timer ones to be replaced with the correct ones
    void ClearSpawns()
    {
        if (currentSpawn == null) return;

        for (int i = 0; i < currentSpawn.Length; i++)
        {
            if (currentSpawn[i] != null)
            {
                Destroy(currentSpawn[i]);
                currentSpawn[i] = null;
            }
        }
    }

    //Function for a delay at the start of the game
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

        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        yield return new WaitForSeconds(2.8f);


        SpawnRandomDirection();
    }

    //Function to create a delay between each round and clear the previous arrows
    IEnumerator NextRoundDelay()
    {
        yield return new WaitForSeconds(2f);

        Curtain.ResetTrigger("Open");
        Curtain.SetTrigger("Close");

        yield return new WaitForSeconds(delayBetweenRounds);

        ClearSpawns();

        yield return null;

        SpawnRandomDirection();
    }
     
    //Function to end the game
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.8f);

        introCurtain.SetTrigger("Ending");
        introCurtain.ResetTrigger("Starting");

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(2);
    }

}
