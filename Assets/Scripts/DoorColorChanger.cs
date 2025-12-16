using UnityEngine;

public class DoorColorChanger : MonoBehaviour
{
   public Renderer targetRenderer;
   public Material newMaterial;

    public void ChangeColor()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material = newMaterial;
        }
    }

}
