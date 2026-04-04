using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemeyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float vertPosition;
    

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y - vertPosition, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
          
            moveSpeed *= -1;
        }
    }
}
