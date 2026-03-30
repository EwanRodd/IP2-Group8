using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject LevelLoader;

    [SerializeField]
    private FloatSO scoreSO;
    void Start()
    {
        scoreSO.Value = 0;
    }

    public void PlayGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 

}
