using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorMenu : MonoBehaviour
{
    static List<int> Games = new List<int> {5,7,8,10,11};
    static List<int> originalGames = new List<int> { 5, 7, 8, 10, 11};

    public Animator wheel;
    public Animator introCurtain;

    [SerializeField] private AudioClip drum;

    [SerializeField]
    private FloatSO scoreSO;

    public void Start()
    {       
        wheel.ResetTrigger("CupGame");
        wheel.ResetTrigger("CopyGame");
        wheel.ResetTrigger("FollowGame");
        wheel.ResetTrigger("TokenGame");
        wheel.ResetTrigger("DodgemGame");

        DontDestroyOnLoad(gameObject);

        StartCoroutine(CurtainDelay());

    }
    public void NextGame()
    {

        int nextLevelIndex = Random.Range(0, Games.Count);

        int nextLevel = Games[nextLevelIndex];

        SFXManager.instance.DrumClip(drum, transform, 0.5f);

        Games.Remove(nextLevel);
        // load the level:
        if (nextLevel == 5)
        {
            StartCoroutine(CopyGame());
        }
        else if (nextLevel == 8)
        {
            StartCoroutine(FollowGame());
        }
        else if (nextLevel == 7)
        {
            StartCoroutine(CupGame());
        }
        else if (nextLevel == 10)
        {
            StartCoroutine(TokenGame());
        }
        else if (nextLevel == 11)
        {
            StartCoroutine(DodgemGame());
        }
    }

    IEnumerator CurtainDelay()
    {
        if (Games.Count == 0)
        {
            EndGame();
        }

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

    IEnumerator DodgemGame()
    {

        wheel.SetTrigger("DodgemGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(11);
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

    IEnumerator FollowGame()
    {
        wheel.SetTrigger("FollowGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(8);
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

    IEnumerator TokenGame()
    {
        wheel.SetTrigger("TokenGame");
        yield return new WaitForSeconds(5f);

        introCurtain.ResetTrigger("Starting");
        introCurtain.SetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(10);
    }

    void EndGame()
    {
        SceneManager.LoadScene(9);
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
