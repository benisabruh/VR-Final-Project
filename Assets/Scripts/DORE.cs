using UnityEngine;
using UnityEngine.SceneManagement;

public class DORE : MonoBehaviour
{
    public Lazers lazers;

    void Start()
    {
        lazers.solved.AddListener(OnSolved);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (lazers != null && lazers.done)
                SceneManager.LoadScene("Ring MAze");
        }
    }

    void OnSolved()
    {
        GetComponent<BoxCollider>().isTrigger = lazers.done;
    }
}
