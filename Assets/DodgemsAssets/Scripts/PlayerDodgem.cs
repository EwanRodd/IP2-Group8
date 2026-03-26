using UnityEngine;

public class PlayerDodgem : MonoBehaviour
{
    public float speed = 16f;
    public float turnSpeed = 320f;
    public float pushbackForce;
    public int lives = 3;

    float moveInput;
    float turnInput;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        float newRotation = rb.rotation - turnInput * turnSpeed;
        Debug.Log(turnInput);
        rb.rotation = newRotation; // direct assignment = instant response

        Vector2 targetVelocity = transform.up * moveInput * speed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 10f * Time.fixedDeltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player hit");
            lives--;

            Vector2 away = (transform.position - collision.transform.position).normalized;
            rb.AddForce(away * pushbackForce, ForceMode2D.Impulse); //dodgems bump off each other
        }
    }
}