using UnityEngine;

public class Door : MonoBehaviour
{
    public Renderer[] renderers;
    public Color pressedColor = Color.red;

    // Call this from a button UnityEvent
    public void ChangeColor()
    {
        if (renderers == null || renderers.Length == 0)
        {
            var r = GetComponent<Renderer>();
            if (r != null) renderers = new Renderer[] { r };
        }

        foreach (var rend in renderers)
        {
            if (rend == null) continue;
            // Use material to change instance color at runtime
            if (rend.material != null)
                rend.material.color = pressedColor;
        }
    }
}
