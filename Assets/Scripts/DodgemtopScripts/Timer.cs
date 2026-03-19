using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float time; 
    public TextMeshProUGUI timerText;
      

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timerText.text = Mathf.Floor(time).ToString();

        if (time < 1)
        {
            Time.timeScale = 0;         
        }
    }

   

}
