using UnityEngine;



public class Gun : MonoBehaviour
{
    public float range = 100f;


    void Update()
    {
        // For testing change to controller 
        PcMode();
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

    void PcMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            Debug.Log("Mouse Button pressed");
        }
    }
}
