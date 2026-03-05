using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject LevelLoader;
     
    public void PlayGame()
    {        
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        LevelLoader.GetComponent<LevelLoader>().LoadNextLevel(); 
    }

    public void QuitGame()
    {
        Application.Quit();
    } 

}
