using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DoorMenu : MonoBehaviour
{
    static List<int> Games = new List<int> {5,6,7};

    public void Start()
    {
        int nextLevel = NextGame();

        StartCoroutine(LoadGame(nextLevel));
    }
    public int NextGame()
    {
        //choose the index of a level:
        int nextLevelIndex = Random.Range(0, Games.Count);
        //get the actual sceneIndex by the index of our list:
        int nextLevel = Games[nextLevelIndex];
        //remove the sceneIndex from the list to make it not appear again:
        Games.Remove(nextLevel);
        // load the level:
        return nextLevel;
    }

    IEnumerator LoadGame(int nextLevel)
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(nextLevel);
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
