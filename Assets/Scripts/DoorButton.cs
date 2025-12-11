using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorButton : XRBaseInteractable
{
   
    public float deadTime = 1.0f;
//Bool used to lock down button during its set dead time 
private bool _deadTimeActive = false;
//public Unity Events we can use in the editor and tie other functions to. 
public UnityEvent onPressed, onReleased;



// Called when the button is "grabbed" or pressed
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        onPressed?.Invoke();
        transform.localPosition += new Vector3(0, -0.02f, 0);
    }

    // Called when the button is released
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        onReleased?.Invoke();
        transform.localPosition += new Vector3(0, 0.02f, 0);
    }

}
