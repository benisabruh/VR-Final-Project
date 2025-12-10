using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandRotate : MonoBehaviour
{
    public XRBaseInteractor leftHand;
    public XRBaseInteractor rightHand;

    private bool leftHeld = false;
    private bool rightHeld = false;

    private Quaternion initialRotation;
    private Vector3 initialDirection;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (interactor.name.Contains("Left"))
            leftHeld = true;

        if (interactor.name.Contains("Right"))
            rightHeld = true;

        if (leftHeld && rightHeld)
            CacheInitial();
    }

    public void OnSelectExited(XRBaseInteractor interactor)
    {
        if (interactor.name.Contains("Left"))
            leftHeld = false;

        if (interactor.name.Contains("Right"))
            rightHeld = false;
    }

    private void Update()
    {
        if (leftHeld && rightHeld)
            ApplyRotation();
    }

    private void CacheInitial()
    {
        initialRotation = transform.rotation;
        initialDirection = rightHand.transform.position - leftHand.transform.position;
    }

    private void ApplyRotation()
    {
        Vector3 currentDirection = rightHand.transform.position - leftHand.transform.position;
        Quaternion rotationDelta = Quaternion.FromToRotation(initialDirection, currentDirection);
        transform.rotation = rotationDelta * initialRotation;
    }
}
