using UnityEngine;

/// <summary>
/// Simple helper to reduce physics tunneling for fast-moving objects.
/// Attach to projectiles or the "marble" GameObject.
/// It sets `collisionDetectionMode` to `ContinuousDynamic` and enables interpolation.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PreventTunneling : MonoBehaviour
{
    [Tooltip("Collision detection mode to set on the Rigidbody (default: ContinuousDynamic)")]
    public CollisionDetectionMode collisionMode = CollisionDetectionMode.ContinuousDynamic;

    [Tooltip("Rigidbody interpolation mode to use (default: Interpolate)")]
    public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;

    private void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        if (rb == null)
            return;

        // Only change if it's not already set to the requested mode.
        if (rb.collisionDetectionMode != collisionMode)
            rb.collisionDetectionMode = collisionMode;

        if (rb.interpolation != interpolation)
            rb.interpolation = interpolation;
    }
}
