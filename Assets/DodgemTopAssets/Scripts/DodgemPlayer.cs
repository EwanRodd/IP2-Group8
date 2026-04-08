using UnityEngine;
using System.Threading;
using System.Collections;
//using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DodgemPlayer : MonoBehaviour
{
    public float moveSpeed = 5;

    public int lives;
    public Image[] livesUI;
    private int delay = 3;

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
        if (lives <= 0)
        {
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);

        if (collision.GetComponent<CapsuleCollider2D>() && collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            lives -= 1;
            for (int i = 0; i < livesUI.Length; i++)
            {
                if (i < lives)
                {
                    livesUI[i].enabled = true;
                }
                else
                {
                    livesUI[i].enabled = false;
                }
            }

        }

    }
}
