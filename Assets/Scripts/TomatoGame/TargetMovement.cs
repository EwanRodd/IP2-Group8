using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed =2f;
    public float moveDistance = 3f;

    private Vector3 startPos;
    private int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;

        // Chooses random direction so prefabs aint in sync
        direction = Random.value < 0.5f ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2. right * speed * direction * Time.deltaTime);
        
        if (Mathf.Abs(transform.position.x - startPos.x) > moveDistance){
            
            // Switch direction 
            direction *= -1;
        }
    }

    
}
