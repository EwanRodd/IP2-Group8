using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int lives;
    public Image[] livesUI;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);

        if(collision.GetComponent<CapsuleCollider2D>() && collision.gameObject.tag == "Enemy")
        {
          Destroy(collision.gameObject);
          lives -= 1;      
          for(int i = 0; i < livesUI.Length; i++)
            {
                if(i < lives)
                {
                    livesUI[i].enabled = true;
                }
                else
                {
                    livesUI[i].enabled=false;
                }
           }
          if (lives <= 0)
          {
            Destroy(gameObject);
          }
           
        }
    }
}
