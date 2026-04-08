using UnityEngine;

public class EnemyDodgemTop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5f;
    private Rigidbody2D rb;
    public GameManager manager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Find the GameManager by tag
        GameObject gmObj = GameObject.FindWithTag("GameManager");
        if (gmObj != null)
        {
            manager = gmObj.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure it has the 'GameManager' tag.");
        }
    }

    void FixedUpdate()
    {
        // Always move left
        rb.linearVelocity = Vector2.left * speed;
        rb.rotation = 180f - 90f; // = 90 degrees

        if (manager.gameState == "over")
        {
            speed = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with enemy car");
        manager.lives--;
        manager.UpdateLives();
        Debug.Log(manager.lives);
        Destroy(gameObject);
    }
}
