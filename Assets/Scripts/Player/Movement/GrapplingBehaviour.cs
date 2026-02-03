using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingBehaviour : MonoBehaviour
{
    #region Component refs
    private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody rigidbody;
    [Tooltip("The object that will follow the grapple hook's azimuth/heading/yaw rotation (grapple turret)")]
    [SerializeField] private Transform grappleAzimuthObject;
    [Tooltip("The object that will follow the grapple hook's elevation/pitch rotation (grapple barrel)")]
    [SerializeField] private Transform grappleElevationObject;
    [Tooltip("Location from which the grapple hook is fired")]
    [SerializeField] private Vector3 grappleMuzzleOffset = Vector3.zero;
    #endregion

    [SerializeField] private Vector3 lRPoint = Vector3.zero;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.SetPosition(0, grappleElevationObject.TransformPoint(grappleMuzzleOffset));
        lineRenderer.SetPosition(1, lRPoint);

        grappleElevationObject.LookAt(lRPoint);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(grappleElevationObject.position + grappleMuzzleOffset, 0.1f);
    }
}