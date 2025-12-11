using UnityEngine;
using UnityEngine.SceneManagement;

public class VRRoomSceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VRroomDoor")
        {
            SceneManager.LoadScene("maze Room");
        }
    }
}
