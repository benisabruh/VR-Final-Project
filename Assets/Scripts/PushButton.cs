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
    Vector3 _initialLocalPosition;

// Called when the button is "pressed" (controller/hand selects it)
    // Called when the button is "grabbed" or pressed
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        HandlePress();
    }

    // Called when the button is released
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        HandleRelease();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        HandlePress();
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        HandleRelease();
    }

    protected override void Awake()
    {
        base.Awake();
        _initialLocalPosition = transform.localPosition;
    }

    void HandlePress()
    {
        if (_deadTimeActive) return;
        onPressed?.Invoke();
        transform.localPosition = _initialLocalPosition + new Vector3(0, -0.02f, 0);
    }

    void HandleRelease()
    {
        if (_deadTimeActive) return;
        onReleased?.Invoke();
        transform.localPosition = _initialLocalPosition;
        StartCoroutine(WaitForDeadTime());
    }

    IEnumerator WaitForDeadTime()
    {
        _deadTimeActive = true;
        yield return new WaitForSeconds(deadTime);
        _deadTimeActive = false;
    }


}
