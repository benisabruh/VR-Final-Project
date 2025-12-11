using UnityEngine;

public class DoorColorChanger : MonoBehaviour
{
   public Renderer targetRenderer;

    public void ChangeColor(Color newColor)
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = newColor;
        }
    }

}
