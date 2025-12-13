using UnityEngine;
using UnityEngine.XR;

// Attach this to the Canvas or Text GameObject you want to follow the player's right hand.
// Behavior: uses an explicitly assigned `handTransform` if present, then searches common scene object names,
// and finally falls back to XR tracking via `XRNode.RightHand`.
public class FollowRightHand : MonoBehaviour
{
    [Tooltip("Direct reference to the right-hand Transform. If left empty the script will attempt to find one by name or use XR tracking.")]
    public Transform handTransform;

    [Tooltip("Position offset in hand local space (applied after finding hand transform).")]
    public Vector3 positionOffset = Vector3.zero;

    [Tooltip("Rotation offset (Euler degrees) applied on top of the hand rotation.")]
    public Vector3 eulerOffset = Vector3.zero;

    [Tooltip("Smooth movement toward the target pose.")]
    public bool smooth = false;
    public float smoothSpeed = 10f;

    public XRNode handNode = XRNode.RightHand;

    void Start()
    {
        if (handTransform == null)
            TryFindCommonRightHand();
    }

    void TryFindCommonRightHand()
    {
        foreach (var t in FindObjectsOfType<Transform>())
        {
            var n = t.name.ToLower();
            if (n.Contains("right") && (n.Contains("hand") || n.Contains("controller") || n.Contains("wrist") || n.Contains("grip")))
            {
                handTransform = t;
                return;
            }
        }
    }

    void Update()
    {
        if (handTransform != null)
        {
            Vector3 targetPos = handTransform.position + handTransform.TransformVector(positionOffset);
            Quaternion targetRot = handTransform.rotation * Quaternion.Euler(eulerOffset);

            if (smooth)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smoothSpeed);
            }
            else
            {
                transform.position = targetPos;
                transform.rotation = targetRot;
            }

            return;
        }

        // Fallback: use XRNode tracking data
        Vector3 localPos = InputTracking.GetLocalPosition(handNode);
        Quaternion localRot = InputTracking.GetLocalRotation(handNode);

        // Convert tracking-space pose to world-space. Try to locate a likely tracking origin (parent of main camera)
        Transform trackingOrigin = (Camera.main != null) ? Camera.main.transform.parent : null;

        Vector3 worldPos;
        Quaternion worldRot;

        if (trackingOrigin != null)
        {
            worldPos = trackingOrigin.TransformPoint(localPos);
            worldRot = trackingOrigin.rotation * localRot;
        }
        else
        {
            // Last resort: treat local as relative to main camera
            if (Camera.main != null)
            {
                worldPos = Camera.main.transform.TransformPoint(localPos);
                worldRot = Camera.main.transform.rotation * localRot;
            }
            else
            {
                return; // can't resolve pose
            }
        }

        Vector3 targetPosFallback = worldPos + (trackingOrigin != null ? trackingOrigin.TransformVector(positionOffset) : Camera.main.transform.TransformVector(positionOffset));
        Quaternion targetRotFallback = worldRot * Quaternion.Euler(eulerOffset);

        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosFallback, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotFallback, Time.deltaTime * smoothSpeed);
        }
        else
        {
            transform.position = targetPosFallback;
            transform.rotation = targetRotFallback;
        }
    }
}
