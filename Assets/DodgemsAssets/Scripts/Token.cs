using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Token : MonoBehaviour
{
    public Vector2 target;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float minDistance = 2f;
    public Manager manager;

    private void Start()
    {
        MoveNewTarget();
    }
    void MoveNewTarget()
    {
        Vector2 newTarget;

        do 
        {
            newTarget = new Vector2(
                Random.Range(minBounds.x, maxBounds.x),
                Random.Range(minBounds.y, maxBounds.y)
            );
        }
        while (Vector2.Distance(transform.position, newTarget) < minDistance);

        target = newTarget;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.AddScore(1);
            MoveNewTarget();
            transform.position = target;
        }        
    }
}
