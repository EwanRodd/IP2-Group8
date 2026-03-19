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
        /*if (rb.linearVelocity.magnitude < 0.1f)
        {
            PickNewTarget();
            rb.AddForce(Random.insideUnitCircle * 3f, ForceMode2D.Impulse);
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            // Pick a new direction so we don't keep pushing
            PickNewTarget();

            // Push away from collision
            Vector2 away = (transform.position - collision.transform.position).normalized;
            rb.AddForce(away * 5f, ForceMode2D.Impulse);
        }
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