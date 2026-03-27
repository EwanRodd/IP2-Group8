using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Music music;
    void Start()
    {

        //Check if the scene is the main menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
            return;
        }
        if (music != null && music != this)
        {
            Destroy(gameObject);
            return;
        }

        //Don't destroy the music when going to each game
        music = this;
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
        //Check if the scene is the main menu
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
