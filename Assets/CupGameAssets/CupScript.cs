using System.Collections;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public float moveDistance = 2f;
    private float moveSpeed = 3f;
    private bool interactable = false;

    public SpriteRenderer spriteRenderer;
    public Sprite cupNormal;
    public Sprite cupHand;
    public Sprite cupTilt;

    public Vector3 normalScale;
    public Vector3 tiltScale;

    [SerializeField] private CupGameManager gameManager;

    private void Start()
    {
        StartCoroutine(GoDown());
    }
    private IEnumerator GoDown()
    {
        yield return new WaitForSeconds(1f);
        MoveVertical(-moveDistance);
    }

    public void PlaceBall(Transform ball)
    {
        //making this cup the parent of the ball
        //so the ball travels with the cup
        ball.SetParent(transform);
        ball.localPosition = Vector3.forward;
    }

    public bool HasBall()
    {
        return transform.childCount > 0;
    }
    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    //you dont use mouse anymore but this is the function called on when selecting with controller
    //which is spaghetti but who cares
    public void OnMouseDown()
    {
        Debug.Log("Moused");
        if (interactable)
        {
            BecomeHand();
            MoveVertical(moveDistance);
            gameManager.OnCupClicked(this);
        }
    }


    //BecomeHand(), BecomeCup(), and BecomeTilt() are all of a trio
    //Their purpose is to just be a function to change the cups sprites and be accessible from the manager
    //But also, each sprite is slightly different and offset due to exporting fuckery
    //So each function will manually adjust their positions and scaling just slightly to fix this
    //you may say "these should really use less magic numbers that seems untenable"
    //to which I say shut up it works

    public void BecomeHand()
    {
        if (spriteRenderer.sprite == cupTilt)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.247f, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x - 0.05f , transform.position.y + 0.66f, transform.position.z);
        spriteRenderer.sprite = cupHand;
        transform.localScale = normalScale;
    }

    public void BecomeCup()
    {
        if (spriteRenderer.sprite == cupHand)
        {
            transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y - 0.66f, transform.position.z);
        }
        else if (spriteRenderer.sprite == cupTilt)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.247f, transform.position.z);
        }
        spriteRenderer.sprite = cupNormal;
        transform.localScale = normalScale;
    }

    public void BecomeTilt()
    {
        spriteRenderer.sprite = cupTilt;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.247f, transform.position.z);
        transform.localScale = tiltScale;
    }


    //go up or go down using the smooth animation thing
    public void MoveVertical(float distance)
    {
        Vector3 targetPosition = transform.position + Vector3.up * distance;
        StartCoroutine(MoveToPosition(targetPosition));
    }

    //the smooth animation thing
    private IEnumerator MoveToPosition(Vector3 target)
    {
        Debug.Log("moving");
        //spriteRenderer.sprite = cupHand;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;
        //spriteRenderer.sprite = cupNormal;
    }
}
