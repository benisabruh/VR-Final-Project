using UnityEngine;

public class VRDoorController : MonoBehaviour
{
   public Animator animator;

    public void OpenDoor()
    {
        animator.SetTrigger("Open");
    }

}
