using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class PushButton : XRBaseInteractable

{
    public Transform button;   // The mesh that moves
    public float pressDepth = 0.02f; // How far it moves down
    public float returnSpeed = 5f;   // How fast it returns

    private Vector3 initialPos;
    private bool isPressed = false;

    protected override void Awake()
    {
        base.Awake();
        initialPos = button.localPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isPressed = true;
        button.localPosition = initialPos - new Vector3(0, pressDepth, 0);

        Debug.Log("Button pressed!");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isPressed = false;
    }


    void Update()
    {
        if (!isPressed)
        {
            button.localPosition = Vector3.Lerp(
                button.localPosition,
                initialPos,
                Time.deltaTime * returnSpeed
            );
        }
    }


}
