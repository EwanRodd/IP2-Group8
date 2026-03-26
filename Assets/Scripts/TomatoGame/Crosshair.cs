using UnityEngine;

public class Crosshair : MonoBehaviour
{

    void Awake()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Confined; 
    }


    // Update is called once per frame
    void Update()
    {
        OnMouseMove();
    }

    void OnMouseMove()
    {
         // gets mouse position from input
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z); 

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.z = -1f;

        transform.position = mousePosition;       
    }
}
