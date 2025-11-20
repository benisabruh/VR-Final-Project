using UnityEngine;

public class Lazer : MonoBehaviour
{
    GameObject destination;

    public void Init(Vector3 position, GameObject destination)
    {
        this.destination = destination;
        GetComponent<LineRenderer>().SetPosition(0, position);
    }

    void Update()
    {
        if (destination == null) {
            Debug.LogError("Lazer not initialized. Make sure to call Init()!");
            return;
        }
        GetComponent<LineRenderer>().SetPosition(1, destination.transform.position);
    }
}
