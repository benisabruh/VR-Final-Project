
using System;
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
        public Quaternion offset; // rotation offset: inverse(handRot) * objectRot
    }

    private readonly List<HandInfo> hands = new List<HandInfo>();

    // Two-hand rotation state
    private Quaternion initialHandsRelative = Quaternion.identity;
    private Quaternion initialObjectRotation = Quaternion.identity;
    private bool twoHanding = false;

    // Runtime attach helper to prevent XR from snapping the object
    private Transform runtimeAttach = null;

    [Tooltip("Smoothing factor for two-hand rotation (higher = snappier)")]
    public float twoHandSmoothing = 12f;

    [Tooltip("Smoothing factor for single-hand rotation (higher = snappier)")]
    public float singleHandSmoothing = 20f;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrab);
            grab.selectExited.RemoveListener(OnRelease);
        }
        if (runtimeAttach != null)
        {
            grab.attachTransform = null;
            Destroy(runtimeAttach.gameObject);
            runtimeAttach = null;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        var hand = args.interactorObject.transform;

        // compute rotation offset so we can keep object's current world rotation
        var offset = Quaternion.Inverse(hand.rotation) * transform.rotation;

        hands.Add(new HandInfo { hand = hand, offset = offset });

        // Create a temporary attach Transform under the interactor to preserve world pose
        if (runtimeAttach == null)
        {
            var go = new GameObject("__RuntimeAttach");
            // Keep world position/rotation when parenting
            go.transform.SetParent(hand, true);
            runtimeAttach = go.transform;
            grab.attachTransform = runtimeAttach;
        }

        // Start coroutine that waits until end of frame (after XR attach applied)
        StartCoroutine(ApplyOffsetAtEndOfFrame(hand, offset));
    }

    private IEnumerator ApplyOffsetAtEndOfFrame(Transform hand, Quaternion offset)
    {
        // Wait until the end of frame so the XR system's attach pose has been applied
        yield return new WaitForEndOfFrame();

        // If the hand is still grabbing, apply the rotation to cancel any snap
        bool stillGrabbing = hands.Exists(h => h.hand == hand);
        if (stillGrabbing)
        {
            // Restore rotation so object keeps its world orientation at grab time
            transform.rotation = hand.rotation * offset;
        }

        // If we now have two hands grabbing, capture the initial relative rotation
        if (hands.Count >= 2)
        {
            var h0 = hands[0].hand;
            var h1 = hands[1].hand;
            initialHandsRelative = Quaternion.Inverse(h0.rotation) * h1.rotation;
            initialObjectRotation = transform.rotation;
            twoHanding = true;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        var hand = args.interactorObject.transform;
        hands.RemoveAll(h => h.hand == hand);

        // Stop two-handed mode when a hand is released
        if (hands.Count < 2)
            twoHanding = false;

        // If one hand remains, recalculate its offset so it continues smoothly
        if (hands.Count == 1)
        {
            var remaining = hands[0];
            remaining.offset = Quaternion.Inverse(remaining.hand.rotation) * transform.rotation;
        }

        // If no hands remain, remove runtime attach and restore attachTransform
        if (hands.Count == 0 && runtimeAttach != null)
        {
            grab.attachTransform = null;
            Destroy(runtimeAttach.gameObject);
            runtimeAttach = null;
        }
    }

    private void FixedUpdate()
    {
        // Two-hand rotation
        if (twoHanding && hands.Count >= 2)
        {
            var h0 = hands[0];
            var h1 = hands[1];

            // Compute current relative rotation between the two hands
            Quaternion currentRelative = Quaternion.Inverse(h0.hand.rotation) * h1.hand.rotation;

            // Delta from initial relative rotation
            Quaternion delta = currentRelative * Quaternion.Inverse(initialHandsRelative);

            // Target rotation relative to the initial object rotation captured when the second hand grabbed
            Quaternion target = delta * initialObjectRotation;

            // Smoothly interpolate rotation to reduce jitter
            float t = Mathf.Clamp01(Time.fixedDeltaTime * twoHandSmoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
            return;
        }

        // Single-hand rotation: follow the hand's rotation while preserving initial offset
        if (hands.Count == 1)
        {
            var h = hands[0];
            Quaternion target = h.hand.rotation * h.offset;
            float t = Mathf.Clamp01(Time.fixedDeltaTime * singleHandSmoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
        }
    }
}
