using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    public GameObject FirstButton;
    public Animator introCurtain;

    public TMP_Text score;
    public TMP_Text fails;
    public TMP_Text total;

    private float totalScore;

    [SerializeField] private AudioClip drum;
    [SerializeField] private AudioClip crowdCheer;

    [SerializeField]
    private FloatSO scoreSO;

    [SerializeField]
    private FloatSO FailSO;

    public void Start()
    {

        totalScore = scoreSO.Value - FailSO.Value;
        StartCoroutine(CurtainDelay());

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(FirstButton);

        score.text = "" + scoreSO.Value;
        fails.text = "" + FailSO.Value;
        total.text = "" + totalScore;
    }

    IEnumerator CurtainDelay()
    {
        yield return new WaitForSeconds(.5f);

        SFXManager.instance.DrumClip(drum, transform, 0.5f);

        introCurtain.SetTrigger("Starting");
        introCurtain.ResetTrigger("Ending");

        yield return new WaitForSeconds(3.5f);

        SFXManager.instance.CrowdCheerClip(crowdCheer, transform, 0.5f);

    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
