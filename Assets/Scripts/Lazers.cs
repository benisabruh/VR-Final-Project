using UnityEngine;

public class Lazers : MonoBehaviour
{
    public GameObject lazer;
    public GameObject destination;
    public GameObject sourcesParent;

    void Start()
    {
        foreach (Transform child in sourcesParent.transform)
        {
            Instantiate(this.lazer, Vector3.zero, Quaternion.identity).GetComponent<Lazer>().Init(child.position, destination);
        }
    }
}
