using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject PauseScreen;

    public void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("PAUSED");
            PauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();     
    }
}
