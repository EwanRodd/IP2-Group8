using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FollowGameTarget : MonoBehaviour
{
    public float moveRadius = 5f;
    public float moveSpeed = 4f;
    public float circleRadius = 1.5f;

    public FollowGameMovement player;

    public bool gameStarted;

    Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Call function to start game delay
        StartCoroutine(GameStartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            PlayGame();
        }
    }

    void PlayGame()
    {
        //Check to see if the game is still going
        if (player.gameDone)
            return;

        //Change the targets position to the new location
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //Ensure the change of location is big enough
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            PickNewTarget();
        }
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(7.8f);

        gameStarted = true;
    }

    //Function for the target to choose its next position
    void PickNewTarget()
    {
        Vector2 random = Random.insideUnitCircle * moveRadius;
        targetPosition = new Vector3(random.x, random.y, 0f);
    }
}
