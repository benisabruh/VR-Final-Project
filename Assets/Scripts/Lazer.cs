using UnityEngine;

public class Lazer : MonoBehaviour
{
    public GameObject destination;

    public void Init(Vector3 position, GameObject destination)
    {
        this.destination = destination;
        GetComponent<LineRenderer>().SetPosition(0, position);
    }

    public void SetSource(Vector3 position)
    {
        GetComponent<LineRenderer>().SetPosition(0, position);
    }

    void Update()
    {
        GetComponent<LineRenderer>().SetPosition(1, destination.transform.position);
    }
}
