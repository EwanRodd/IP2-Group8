using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject PauseScreen;
    public static PauseMenu menu;

    private void Start()
    {
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

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
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
