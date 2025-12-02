using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeepRotationAfterAttach : MonoBehaviour
{
    private XRGrabInteractable grab;

    private class HandInfo
    {
        public Transform hand;
        public Quaternion offset;
    }

    private readonly List<HandInfo> hands = new List<HandInfo>();

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        var hand = args.interactorObject.transform;
        var offset = Quaternion.Inverse(hand.rotation) * transform.rotation;

        hands.Add(new HandInfo { hand = hand, offset = offset });

        // Start coroutine that waits until end of frame (after XR attach applied)
        StartCoroutine(ApplyOffsetAtEndOfFrame(hand, offset));
    }

    private IEnumerator ApplyOffsetAtEndOfFrame(Transform hand, Quaternion offset)
    {
        // Wait until the end of frame so the XR system's attach pose has been applied
        yield return new WaitForEndOfFrame();

        // If the hand is still grabbing, apply the rotation to cancel the snap
        bool stillGrabbing = hands.Exists(h => h.hand == hand);
        if (stillGrabbing)
            transform.rotation = hand.rotation * offset;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        var hand = args.interactorObject.transform;
        hands.RemoveAll(h => h.hand == hand);
    }
}
