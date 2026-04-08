using UnityEngine;

public class PlayerDodgemTop : MonoBehaviour
{
    public int lives = 3;

    float moveInput;
    float turnInput;
    public float turnSpeed = 150f;

    public float moveSpeed = 5f;
    public float deadzone = 0.2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.right; // default facing

    public GameManager manager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //if (manager.gameState == "active")
        //{
        //}
        if (manager.gameState == "over")
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = 5;
        }


        if (manager.gameState == "active")
        {
            moveInput = Input.GetAxisRaw("Vertical");
            turnInput = Input.GetAxisRaw("Horizontal");
        }


    }

    void FixedUpdate()
    {
        float newRotation = rb.rotation - turnInput * turnSpeed;
        rb.rotation = newRotation; // direct assignment = instant response

        Vector2 targetVelocity = transform.up * moveInput * moveSpeed;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 10f * Time.fixedDeltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player hit");
            lives--;
        }
    }
}
