using UnityEngine;
using System.Collections;

public class MovingToStart : MonoBehaviour
{

    Vector3 direction = new Vector3(-0.327f, 4f, -1.53f);


    //     public float speed = 1f;


    private Transform target;
    public GameObject ring;
    public GameObject key;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        key.gameObject.SetActive(false);
        Debug.Log("HI");
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        if (GetComponent<Collider>() != null)
        {
            Debug.Log("This exists");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "wire")
        {
            StartCoroutine(MoveToStartCoroutine(ring));
        }
        else if (col.tag == "Winner")
        {
            Winning.win(key);
        }

    }
    IEnumerator MoveToStartCoroutine(GameObject obj)
    {
        Vector3 targetPosition = new Vector3(-0.327f, -0.106f, -1.53f);
        obj.GetComponent<Collider>().enabled = false;
        obj.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        while (obj.transform.position != targetPosition /*|| this.transform.rotation != targetRotation*/)
        {
            float step = 2 * Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, step);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
            yield return null;
        }
        Debug.Log("Movement finished");
        obj.GetComponent<Collider>().enabled = true;
        // GetComponent<Rigidbody>().enabled = true;

    }
}
