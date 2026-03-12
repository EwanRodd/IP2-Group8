using UnityEngine;

public class StackerGameManager : MonoBehaviour
{
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] public int maxStacks = 5;
    [SerializeField] private float yOffset = 1.5f;

    public int currentStacks = 0;
    public float currentHeight = -4f;
    private float currentSpeed = 1f;

    private GameObject lastStack;

    [SerializeField] public GameObject stackEffectPrefab;
    public GameObject confettiOne;
    public GameObject confettiTwo;
    public Transform[] confettiSpawns;

    private StackerGameMovement movement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnStack();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnStack()
    {
        if (currentStacks >= maxStacks)
            return;

        Vector3 spawnPosition = new Vector3(0f, currentHeight, 0f);
        GameObject newStack = Instantiate(stackPrefab, spawnPosition, Quaternion.identity);

        StackerGameMovement movement = newStack.GetComponent<StackerGameMovement>();

        newStack.GetComponent<StackerGameMovement>().SetPreviousStack(lastStack);

        movement.moveSpeed = currentSpeed;

        currentSpeed *= 10f;

        lastStack = newStack;

        currentStacks++;
        currentHeight += yOffset;
        
    }
}
