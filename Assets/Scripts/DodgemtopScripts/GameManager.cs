using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    //Ewan change end delay to match curtains
    public float endDelay = 1f;

    public GameObject WinLevelUI;
    public GameObject LoseLevelUI;

    //public void CompleteLevel()
    // {
    //    completeLevelUI.SetActive(true);
    //     EndGame();
    //  }

    public void WinGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("SceneChange", endDelay);
           
            SceneManager.LoadScene(2);
            
        } 
    }

    public void LoseGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("SceneChange", endDelay);

            SceneManager.LoadScene(2);

        }
    }


}
