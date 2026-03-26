using UnityEngine;

public class PlayerDodgem : MonoBehaviour
{
    public float speed = 8f;
    public float turnSpeed = 200f;
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
        rb.MoveRotation(rb.rotation - turnInput * turnSpeed * Time.fixedDeltaTime);

        Vector2 movement = transform.up * moveInput * speed;
        rb.AddForce(movement);
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