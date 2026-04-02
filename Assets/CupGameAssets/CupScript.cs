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
    public Sprite cupTilt;

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
            BecomeHand();
            MoveVertical(moveDistance);
            gameManager.OnCupClicked(this);
        }
    }


    public void BecomeHand()
    {
        spriteRenderer.sprite = cupHand;
        //transform.position = new Vector3(transform.position.x - 0.05f , transform.position.y + 0.66f, transform.position.z);
        // these manual adjustments of the positions are just cause the two sprites are slightly offset and this fixes that
    }

    public void BecomeCup()
    {
        if (spriteRenderer.sprite == cupTilt || true)
        {
            transform.Rotate(0f, 0f, -15f);
        }
        spriteRenderer.sprite = cupNormal;
        //transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y - 0.66f, transform.position.z);
        // these manual adjustments of the positions are just cause the two sprites are slightly offset and this fixes that
    }

    public void BecomeTilt()
    {
        spriteRenderer.sprite = cupTilt;
        transform.Rotate(0f, 0f, 15f);
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
