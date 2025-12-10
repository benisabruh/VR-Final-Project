// using UnityEngine;

public class MovingToStart : MonoBehaviour
{
    
    Vector3 direction = new Vector3(-0.327f, 4f, -1.53f);
 
    
//     public float speed = 1f;


    private Transform target;
    public GameObject ring;
    public GameObject key;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
//     void Start()
//     {
//         Debug.Log("HI");
//         Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
//         if (GetComponent<Collider>() != null){
//             Debug.Log("This exists");
//         }
        
//         //target = ring.transform;
//         //target.transform.position = new Vector3(-0.327f, -0.106f, -1.53f);
//         //Quaternion targetRotation = Quaternion.Euler(90,0,0);
//     }

    void onTriggerEnter(Collider col){
        if(col.tag == "wire"){
            MovingToStart.moveToStart(ring);
        }
        else if (col.tag == "Winner"){
            Winning.win(key);
        }
        
    }
    static void moveToStart(GameObject obj){
        Vector3 targetPosition = new Vector3(-0.327f, -0.106f, -1.53f);
        obj.GetComponent<Collider>().enabled = false;
        obj.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        while (obj.transform.position != targetPosition /*|| this.transform.rotation != targetRotation*/){
            float step = 2 * Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            
        }
        Debug.Log("Movement finished");
        obj.GetComponent<Collider>().enabled = true;
       // GetComponent<Rigidbody>().enabled = true;
        
//     }

//     // Update is called once per frame

        
        
    
}
