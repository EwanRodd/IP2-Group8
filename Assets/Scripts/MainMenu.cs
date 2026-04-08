using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject LevelLoader;

    public GameObject FirstButton;
    public GameObject introCurtains;
    Animator introCurtain;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;
    void Start()
    {
        scoreSO.Value = 0;

        FailSO.Value = 0;

        introCurtain = introCurtains.GetComponent<Animator>();

        EventSystem.current.SetSelectedGameObject(FirstButton);

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
