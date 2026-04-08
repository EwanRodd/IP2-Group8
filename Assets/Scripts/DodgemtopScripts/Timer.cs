using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float time; 
    public TextMeshProUGUI timerText;

    public GameManager manager;
      

    // Update is called once per frame
    void Update()
    {
        if (manager.gameHasEnded == false)
        {
            time -= Time.deltaTime;
            timerText.text = Mathf.Floor(time).ToString();

            if (time < 1)
            {

            }
        }
       
    }

   

}
