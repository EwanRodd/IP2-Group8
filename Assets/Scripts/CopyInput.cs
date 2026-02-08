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
        if (timerActive)
        {
            timer -= Time.deltaTime;

            timer = Mathf.Max(timer, 0f);

            timerText.text = timer.ToString("F1");

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

        if (gameDone)
            return;

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

    void SpawnRandomInput()
    {
        timerActive = true;
        timer = timelimit;
        correctOrder = new InputButtonData[randoSpawns.Length];
        currentIndex = 0;

        for (int i = 0; i < randoSpawns.Length; i++)
        {
            if (currentSpawns[i] != null)
            {
                Destroy(currentSpawns[i]);
            }

            GameObject button = randoInput[Random.Range(0, randoInput.Length)];

            currentSpawns[i] = Instantiate(button, randoSpawns[i].position, randoSpawns[i].rotation);
            
            currentSpawns[i].SetActive(true);

            foreach (InputButtonData data in inputButtons)
            {
                if (data != null && data.prefab == button)
                {
                    correctOrder[i] = data;
                    break;
                }
            }
        }
    }

    void CheckInput(InputButtonData pressed)
    {

        bool correct = pressed == correctOrder[currentIndex];


        Transform answerPos = AnswerSpawns[currentIndex];
        GameObject answerPrefab = correct ? checkmark : cross;

        Instantiate(answerPrefab, answerPos.position, answerPos.rotation);

        if (correct)
        {
            if (pressed == correctOrder[currentIndex])
            {
                currentIndex++;

                if (currentIndex >= correctOrder.Length)
                {

                    if (confettiOne != null)
                        Instantiate(confettiOne, confettiSpawns[0].position, confettiSpawns[0].rotation);

                    if (confettiTwo != null)
                        Instantiate(confettiTwo, confettiSpawns[1].position, confettiSpawns[1].rotation);

                    gameDone = true;
                    timerActive = false;
                }
            }

        }

        if (!correct)
        {
            Debug.Log("Wrong input!");
            wrongButton.gameObject.SetActive(true);

            if (Explosion != null)
                Instantiate(Explosion, ExplosionSpawn.position, ExplosionSpawn.rotation);

            gameDone = true;
            timerActive = false;
        }
    }
}
