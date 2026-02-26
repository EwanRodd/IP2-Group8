using System.Collections;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public float moveDistance = 2f;
    private float moveSpeed = 3f;
    private bool interactable = false;

    [SerializeField] private CupGameManager gameManager;

    private void Start()
    {
        StartCoroutine(Wait1());
    }
    private IEnumerator Wait1()
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
            MoveVertical(moveDistance);
            gameManager.OnCupClicked(this);
        }
    }

    public void MoveVertical(float distance)
    {
        Vector3 targetPosition = transform.position + Vector3.up * distance;
        StartCoroutine(MoveToPosition(targetPosition));
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
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
    }
}
