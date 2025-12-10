using UnityEngine;
using UnityEngine.SceneManagement;

public class mazescenechange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bendoor")
        {
            SceneManager.LoadScene("lazer puzzle");
        }
    }
}