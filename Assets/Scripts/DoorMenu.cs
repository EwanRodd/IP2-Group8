using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DoorMenu : MonoBehaviour
{
    static List<int> Games = new List<int> {5,6,};
    static List<int> originalGames = new List<int> { 5, 6,};

    public Animator wheel;
    public Animator introCurtain;

    public void Start()
    {
        wheel.ResetTrigger("CupGame");
        wheel.ResetTrigger("CopyGame");
        wheel.ResetTrigger("StackerGame");

        DontDestroyOnLoad(gameObject);

        StartCoroutine(CurtainDelay());
    }
    public void NextGame()
    {

        int nextLevelIndex = Random.Range(0, Games.Count);

        int nextLevel = Games[nextLevelIndex];
 
        Games.Remove(nextLevel);
        // load the level:
        if (nextLevel == 5)
        {
            StartCoroutine(CopyGame());
        }
        else if (nextLevel == 6)
        {
            StartCoroutine(StackerGame());
        }
        else if (nextLevel == 7)
        {
            StartCoroutine(CupGame());
        }
    }

    IEnumerator CurtainDelay()
    {
        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        NextGame();
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
            ResetGames();
        }
    }

    void ResetGames()
    {
        Games = new List<int>(originalGames);
    }
    IEnumerator CopyGame()
    {
        wheel.SetTrigger("CopyGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(5);
    }

    IEnumerator StackerGame()
    {
        wheel.SetTrigger("StackerGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(6);
    }

    IEnumerator CupGame()
    {
        wheel.SetTrigger("CupGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(7);
    }

    public void Door1()
    {
        SceneManager.LoadScene(5);
    }

    public void Door2()
    {
        SceneManager.LoadScene(6);
    }

    public void Door3()
    {
        SceneManager.LoadScene(7);
    }

}
