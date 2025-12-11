using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
 if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
        }
    }
}
