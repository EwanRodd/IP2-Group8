using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseScreen;
    public static PauseMenu menu;
    public bool isPaused;
    public GameObject pauseFirstButton;
    private void Start()
    {
        isPaused = false;

        //Check to see if the current scene is the main menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
            return;
        }
        if (menu != null && menu != this)
        {
            Destroy(gameObject);
            return;
        }

        //Don't destroy the pause menu
        menu = this;
        DontDestroyOnLoad(gameObject);

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    //Check to see if the current scene is the main menu
    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {

        //Stop and pause the game when the pause button is pressed
        if (Input.GetButtonDown("Pause") && isPaused == false)
        {
            isPaused = true;
            Debug.Log("PAUSED");
            PauseScreen.SetActive(true);
            Time.timeScale = 0;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }

        //Unpause the game if the pause button is pressed while the game is paused
        else if (Input.GetButtonDown("Pause") && isPaused == true)
        {
            isPaused = false;
            Debug.Log("UNPAUSED");
            PauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    //Button for the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    //Button to resume the game
    public void Resume()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    //Button to quit the game
    public void Quit()
    {
        Application.Quit();     
    }
}
