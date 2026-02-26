using Unity.VisualScripting;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Music music;
    void Start()
    {
        if(music != null)
        {
            Destroy(gameObject);
        }
        else
        {
            music = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
