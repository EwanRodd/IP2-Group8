using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    public float controllerSpeed = 10f;
    private PlayerInput playerInput;
    private Vector2 lookInput;
    private Camera cam;

    void Awake()
    {
        playerInput = new PlayerInput();

        cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; 
    }


    // Update is called once per frame
    void Update()
    {
        
        lookInput = playerInput.Player.Aim.ReadValue<Vector2>();

        // To allow testing, will detect if mouse present, allowing for seamless testing
        // without having to change too much code
        if(Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            UpdateMouse();

        }
        else
        {
            UpdateController();
        }


    }

    void UpdateMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Mathf.Abs(cam.transform.position.z);

        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.z = -1f;

        transform.position = worldPos;   
    }

    void UpdateController()
    {
        Vector3 movement = new Vector3(lookInput.x, lookInput.y, 0f) * controllerSpeed * Time.deltaTime;
        transform.position += movement;        
    }

    void OnEnable()
    {
        playerInput.Enable();
    }
    void OnDisable()
    {
        playerInput.Disable();
    }
}
