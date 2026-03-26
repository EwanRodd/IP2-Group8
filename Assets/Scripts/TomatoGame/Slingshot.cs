using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class Slingshot : MonoBehaviour
{
    private PlayerInput playerInput;

    public float range = 100f;

    void Awake()
    {
        
        playerInput = new PlayerInput();

        // --New Updated input system to link with controller--
        playerInput.Player.Fire.performed += OnFire;
        
    }


    // !! Only needed for testing with mouse !!
    void Update()
    {
        // ------For testing --------
        //Pc_LeftMouseButtonDown();

    }

    void Fire()
    {
        // debugging ray
        Debug.DrawRay(transform.position, transform.right * range, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, range);
        
        if(hit.collider != null){
            Destroy(hit.collider.gameObject);
            Debug.Log("HITTT");
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Fire();
        Debug.Log("Fired using Input Actions");
    }
    void Controller_OnFireButtonDown()
    {
        Fire();
        Debug.Log("right trigger pressed");
        
    }

    void Pc_LeftMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            Debug.Log("Mouse Button pressed");
        }
    }
}
