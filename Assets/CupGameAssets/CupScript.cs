using System.Collections;
using UnityEngine;

public class Cup : MonoBehaviour
{

    //these should really be prefabs but ehh sunk cost fallacy

    public float moveDistance = 2f;
    private float moveSpeed = 3f;
    private bool interactable = false;

    public SpriteRenderer spriteRenderer;
    public Sprite cupNormal;
    public Sprite cupHand;

    [SerializeField] private CupGameManager gameManager;

    private void Start()
    {
        MoveVertical(-moveDistance);
    }

    public void PlaceBall(Transform ball)
    {
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

    private void OnMouseDown()
    {
        if (interactable)
        {
            MoveVertical(moveDistance);
            gameManager.OnCupClicked(this);
        }
    }


    public void BecomeHand()
    {
        spriteRenderer.sprite = cupHand;
    }

    public void BecomeCup()
    {
        spriteRenderer.sprite = cupNormal;
    }

    public void MoveVertical(float distance)
    {
        Vector3 targetPosition = transform.position + Vector3.up * distance;
        StartCoroutine(MoveToPosition(targetPosition));
    }

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
