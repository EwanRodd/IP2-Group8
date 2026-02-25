using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenu : MonoBehaviour
{
    public void Door1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex * 2);
    }

    public void Door2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex * 3);
    }

    public void Door3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex * 4);
    }

}
