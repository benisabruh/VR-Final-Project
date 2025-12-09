using UnityEngine;

public class MazeStarFinish : MonoBehaviour
{
private void OnTriggerEnter(Collider other)
    {
        // Check if collision is made with the Drop, which is tagged as "Finish"
        // You can change the tag and manually change this accordingly.
        if (other.tag == "marble")
        {
            other.gameObject.SetActive(false);
        }
        }
        }