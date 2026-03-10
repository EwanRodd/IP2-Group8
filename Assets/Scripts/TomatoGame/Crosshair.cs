using UnityEngine;

public class Crosshair : MonoBehaviour
{

    void Awake()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Confined; 
    }
    void Start()
    {
        // removes origional cursor from screen
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // gets mouse position from input
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); 

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.z = -1f;

        transform.position = mousePosition;
    }
}
