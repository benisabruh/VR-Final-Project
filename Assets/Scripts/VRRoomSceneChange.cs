using UnityEngine;
using UnityEngine.SceneManagement;

public class VRRoomSceneChange : MonoBehaviour
{
    public GameObject door_1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VRroomDoor")
        {
            SceneManager.LoadScene("maze Room");
        }
    }

    // Call this from a button's OnPressed UnityEvent to hide the door
    public void HideDoor()
    {
        if (door_1 != null)
        {
            door_1.SetActive(false);
        }
    }
}
