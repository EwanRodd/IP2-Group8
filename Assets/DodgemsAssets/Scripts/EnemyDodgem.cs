using UnityEngine;

public class EnemyDodgem : MonoBehaviour
{
    public float speed = 8f;
    public float turnSpeed = 200f;

    public Vector2 minBounds = new Vector2(-8, -4);
    public Vector2 maxBounds = new Vector2(8, 4);

    public float targetReachDistance = 0.5f;

    private Rigidbody2D rb;
    private Vector2 target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickNewTarget();
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    void PickNewTarget()
    {
        target = new Vector2(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y)
        );
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.2f);
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * turnSpeed;
        rb.AddForce(transform.up * speed);

        float distance = Vector2.Distance(transform.position, target);

        if (distance < targetReachDistance)
        {
            PickNewTarget();
        }
    }
}