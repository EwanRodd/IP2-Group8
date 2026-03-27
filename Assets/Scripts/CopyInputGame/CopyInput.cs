using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.PointerEventData;
using TMPro;

public class CopyInput : MonoBehaviour
{

    public enum InputType
    {
        Button, Direction
    }

    [System.Serializable]
    public class InputButtonData
    {
        public string inputName;
        public InputType inputType;
        public int axisDirection;
        public GameObject prefab;
    }

    [Header("Buttons")]
    public GameObject Left;
    public GameObject Right;
    public GameObject Up;
    public GameObject Down;
    public GameObject AButton;
    public GameObject BButton;
    public GameObject XButton;
    public GameObject YButton;
    public TMP_Text wrongButton;

    float lastHorizontal = 0f;
    float lastVertical = 0f;

    [Header("Responses, Confetti & Explosion")]
    public GameObject checkmark;
    public GameObject cross;
    public GameObject confettiOne;
    public GameObject confettiTwo;
    public GameObject Explosion;

    public InputButtonData[] inputButtons;
    private InputButtonData[] correctOrder;

    [Header("Spawns")]
    public GameObject[] randoInput;
    public Transform[] randoSpawns;
    public GameObject currentSpawn;
    public Transform[] AnswerSpawns;
    public Transform[] confettiSpawns;
    public Transform ExplosionSpawn;

    private GameObject[] currentSpawns;
    private int currentIndex = 0;

    [Header("Timer")]
    public float timer;
    public float timelimit = 5f;
    private bool timerActive = false;
    public TMP_Text timerText;
    public TMP_Text TimeUp;

    private bool gameDone = false;


    void Start()
    {
        Left.SetActive(false);
        Right.SetActive(false);
        Up.SetActive(false);
        Down.SetActive(false);
        AButton.SetActive(false);
        BButton.SetActive(false);
        XButton.SetActive(false);
        YButton.SetActive(false);
        TimeUp.gameObject.SetActive(false);
        wrongButton.gameObject.SetActive(false);

        currentSpawns = new GameObject[randoSpawns.Length];
        SpawnRandomInput();
    }

    // Update is called once per frame
    void Update()
    {
        //DPad();

        //Check to see if the timer is still going, if not, stop it at 0
        if (timerActive)
        {
            timer -= Time.deltaTime;

            timer = Mathf.Max(timer, 0f);

            timerText.text = timer.ToString("F1");

            //Check to see if the timer has reached zero, if so, stop the game
            if (timer <= 0f)
            {
                timerActive = false;
                gameDone = true;
                Debug.Log("Time's up!");
                TimeUp.gameObject.SetActive(true);

                if (Explosion != null)
                    Instantiate(Explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);
            }
        }

        //Check to see if the game is done
        if (gameDone)
            return;

        //Assign each button (A, B, X, Y) so it can be properly checked later in the code, and so it can be read properly
        foreach (InputButtonData button in inputButtons)
        {
            if (button.inputType == InputType.Button)
            {
                if (Input.GetButtonDown(button.inputName))
                {
                    CheckInput(button);
                    break;
                }
            }

            //Assign each direction on the DPad properly and so it can be read by the code correctly
            else if (button.inputType == InputType.Direction)
            {
                float axisValue = Input.GetAxisRaw(button.inputName);

                float lastValue = button.inputName == "Horizontal" ? lastHorizontal : lastVertical;

                if (axisValue == button.axisDirection && lastValue == 0)
                {
                    CheckInput(button);
                    break;
                }
            }
        }

        lastHorizontal = Input.GetAxisRaw("Horizontal");
        lastVertical = Input.GetAxisRaw("Vertical");
    }

    //For Testing DPad
    void DPad()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        if (x == -1f)
        {
            Left.SetActive(true);
            Right.SetActive(false);
            Up.SetActive(false);
            Down.SetActive(false);
        }
        else if (x == 1f)
        {
            Left.SetActive(false);
            Right.SetActive(true);
            Up.SetActive(false);
            Down.SetActive(false);
        }
        else if (y == -1f)
        {
            Left.SetActive(false);
            Right.SetActive(false);
            Down.SetActive(true);
            Up.SetActive(false);
        }
        else if (y == 1f)
        {
            Left.SetActive(false);
            Right.SetActive(false);
            Down.SetActive(false);
            Up.SetActive(true);
        }

    }

    //Function to Randomly choose 5 inputs
    void SpawnRandomInput()
    {
        timerActive = true;
        timer = timelimit;
        correctOrder = new InputButtonData[randoSpawns.Length];
        currentIndex = 0;

        //Randomly choose 5 buttons to display on screen
        for (int i = 0; i < randoSpawns.Length; i++)
        {
            if (currentSpawns[i] != null)
            {
                Destroy(currentSpawns[i]);
            }

            GameObject button = randoInput[Random.Range(0, randoInput.Length)];

            //Display randomly chosen buttons on screen
            currentSpawns[i] = Instantiate(button, randoSpawns[i].position, randoSpawns[i].rotation);
            
            currentSpawns[i].SetActive(true);

            //Create an array full of the correct buttons
            foreach (InputButtonData data in inputButtons)
            {
                if (data != null && data.prefab.name == button.name.Replace("(Clone)", ""))
                {
                    correctOrder[i] = data;
                    break;
                }
            }
        }
    }

    //Function to check if the player has picked the right button
    void CheckInput(InputButtonData pressed)
    {
        //Assign a bool to either be true or false depending on if the player is correct or not
        bool correct = pressed == correctOrder[currentIndex];

        Transform answerPos = AnswerSpawns[currentIndex];

        //Create a new prefab that while be assinged to a symbol depending on if the player is correct or not
        GameObject answerPrefab = correct ? checkmark : cross;

        //Place this new symbol in the position of the shown button
        Instantiate(answerPrefab, answerPos.position, answerPos.rotation);

        //Check to see if the player is correct
        if (correct)
        {
            //Check to see if the player has pressed the correct button
            if (pressed == correctOrder[currentIndex])
            {
                currentIndex++;

                //Check to see if the player has won or not
                if (currentIndex >= correctOrder.Length)
                {
                    //If the player has won, stop the game and play confetti
                    if (confettiOne != null)
                        Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                    if (confettiTwo != null)
                        Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                    gameDone = true;
                    timerActive = false;
                }
            }

        }   

        //Check to see if the player was incorrect
        if (!correct)
        {
            //If the player is incorrect, stop the game
            Debug.Log("Wrong input!");
            wrongButton.gameObject.SetActive(true);

            if (Explosion != null)
                Instantiate(Explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);

            gameDone = true;
            timerActive = false;
        }
    }
}
