using UnityEngine;
using System.Threading;

public class DodgemPlayer : MonoBehaviour
{
    public float moveSpeed = 5;

    public GameManager manager;
    
    
    void Start()
    {
        
    }


    void Update()
    {
        if (manager.gameHasEnded == false)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveX, moveY, 0f);
            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
    }
}
