using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject LevelLoader;

    public GameObject introCurtains;
    Animator introCurtain;

    [SerializeField]
    private FloatSO scoreSO;
    void Start()
    {
        scoreSO.Value = 0;

        introCurtain = introCurtains.GetComponent<Animator>();

    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameRoutine());
    }

    IEnumerator PlayGameRoutine()
    {
        introCurtains.SetActive(true);

        introCurtain.SetTrigger("Ending");
        introCurtain.ResetTrigger("Starting");

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 

}
