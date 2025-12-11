using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class PushButton : XRBaseInteractable

{
    public float deadTime = 1.0f;
private bool _deadTimeActive = false;
public UnityEvent onPressed, onReleased;

// Called when the button is "pressed" (controller/hand selects it)
    // Called when the button is "grabbed" or pressed
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        onPressed?.Invoke();
        // Optional: animate button moving down
        transform.localPosition += new Vector3(0, -0.02f, 0);
    }

    // Called when the button is released
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        onReleased?.Invoke();
        // Optional: animate button moving back up
        transform.localPosition += new Vector3(0, 0.02f, 0);
    }


}
