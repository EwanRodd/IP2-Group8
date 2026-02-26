using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StackerGameMovement : MonoBehaviour
{
    [SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float gridSize = 1f;

    private bool isMoving = false;
    private bool hasStopped = false;

    private GameObject previousStack;

    [SerializeField] private float leftLimit = -5f;
    [SerializeField] private float rightLimit = 5f;

    private int direction = 1;
    [SerializeField] public float moveSpeed = 1f;

    private Vector2 bounceDirection = Vector2.right;

    [SerializeField] private AudioClip crowdCheer;
    [SerializeField] private AudioClip crowdBoo;
    [SerializeField] private AudioClip pop;

    private StackerGameManager spawner;

    [SerializeField] private float requiredFirstBlockX = 0f;

    private int delay = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = FindFirstObjectByType<StackerGameManager>();

        StartCoroutine(Bounce());
    }
    void Update()
    {

        Debug.Log(moveSpeed);
        if (!isMoving)
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            if (transform.position.x >= rightLimit)
                direction = -1;
            else if (transform.position.x <= leftLimit)
                direction = 1;
        }

        if (!hasStopped && Input.GetButtonDown("Fire2"))
        {
            hasStopped = true;
            StopAllCoroutines();

            SpawnStackEffect();

            if (!IsAligned())
            {
                Debug.Log("Game Over!");
                SFXManager.instance.CrowdBooClip(crowdBoo, transform, 0.1f);
                StartCoroutine(EndGame());
                return;
            }

            if (spawner.currentStacks >= spawner.maxStacks)
            {
                if (spawner.confettiOne != null)
                    Instantiate(spawner.confettiOne, spawner.confettiSpawns[0].position, spawner.confettiSpawns[0].rotation);

                if (spawner.confettiTwo != null)
                    Instantiate(spawner.confettiTwo, spawner.confettiSpawns[1].position, spawner.confettiSpawns[1].rotation);

                enabled = true;

                SFXManager.instance.PopClip(pop, transform, 0.1f);
                SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.2f);
                StartCoroutine(EndGame());
                return;
            }

            spawner.SpawnStack();
        }
    }

    private IEnumerator Move(Vector2 direction)
    {
        isMoving = true;

        transform.position += (Vector3)(direction * gridSize);

        yield return new WaitForSeconds(moveDuration);

        isMoving = false;

    }

    private IEnumerator Bounce()
    {
        while (!hasStopped)
        {
            yield return StartCoroutine(Move(bounceDirection));

            if (transform.position.x >= rightLimit)
                bounceDirection = Vector2.left;
            else if (transform.position.x <= leftLimit)
                bounceDirection = Vector2.right;
        }
    }

    public void SetPreviousStack(GameObject stack)
    {
        previousStack = stack;
    }

    private bool IsAligned()
    {
        if (previousStack == null)
        {
            return transform.position.x == requiredFirstBlockX;
        }


        float distance = Mathf.Abs(transform.position.x - previousStack.transform.position.x);

        float allowedOverlap = gridSize;

        return distance < allowedOverlap;
    }

    private void SpawnStackEffect()
    {
        if (spawner.stackEffectPrefab == null)
            return;

        Vector3 spawnPosition = transform.position;

        spawnPosition.y -= transform.localScale.y / 2f;

        GameObject effect = Instantiate(spawner.stackEffectPrefab, spawnPosition, Quaternion.identity);

    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(1);
    }

}
