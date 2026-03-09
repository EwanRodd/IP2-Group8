using UnityEngine;

public float range = 100f

public class Gun : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        // For testing change to controller 
        PcMode();
    }

    void Fire()
    {
        Raycast2D hit = Physics2D.Raycast(transform.position, transform.right, range);
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
