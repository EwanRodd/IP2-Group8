using UnityEngine;



public class Gun : MonoBehaviour
{
    public float range = 100f;


    void Update()
    {
        // For testing change to controller 
        Pc_LeftMouseButtonDown();
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
