using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenu : MonoBehaviour
{
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
