using UnityEngine;

public class MazeStarFinish : MonoBehaviour
{
    [Tooltip("Reference to the labyrinth object whose material will turn blue")]
    public GameObject labyrinth;

    [Tooltip("Reference to the closed door object")]
    public GameObject closedDoor;

    [Tooltip("Reference to the open door object")]
    public GameObject openDoor;

    [Tooltip("Color to change the labyrinth material to")]
    public Color targetColor = Color.blue;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if collision is made with the Drop, which is tagged as "Finish"
        // You can change the tag and manually change this accordingly.
        if (other.tag == "marble" && !hasTriggered)
        {
            hasTriggered = true;
            // Make the star disappear
            gameObject.SetActive(false);

            // Turn the labyrinth material blue
            if (labyrinth != null)
            {
                Renderer renderer = labyrinth.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.color = targetColor;
                }
            }

            // Trigger glass_open_door animation
            if (closedDoor != null)
            {
                closedDoor.SetActive(false);
            }
            if (openDoor != null)
            {
                openDoor.SetActive(true);
            }

            // Deactivate marble (optional, you had this originally)
            other.gameObject.SetActive(false);
        }
    }
}