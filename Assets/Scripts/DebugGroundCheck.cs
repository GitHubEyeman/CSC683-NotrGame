using UnityEngine;

public class DebugGroundCheck : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name + 
                  " | Layer: " + LayerMask.LayerToName(collision.gameObject.layer) +
                  " | Tag: " + collision.gameObject.tag);
        
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("Contact point: " + contact.point + 
                      " | Normal: " + contact.normal);
        }
    }
}