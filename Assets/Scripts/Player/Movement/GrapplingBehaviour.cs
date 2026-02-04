using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingBehaviour : MonoBehaviour
{
    #region Component refs
    private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody vehicleRigidbody;
    [Tooltip("The object that will follow the grapple hook's azimuth/heading/yaw rotation (grapple turret)")]
    [SerializeField] private Transform grappleAzimuthObject;
    [Tooltip("The object that will follow the grapple hook's elevation/pitch rotation (grapple barrel)")]
    [SerializeField] private Transform grappleElevationObject;
    [Tooltip("Location from which the grapple hook is fired")]
    [SerializeField] private Vector3 grappleMuzzleOffset = Vector3.zero;
    #endregion

    [SerializeField] private Vector3 lRPoint = Vector3.zero;
    private float grappleDistance = 0f;
    private bool isGrappling = false;
    public void Toggle()
    {
        Debug.Log("Toggling grapple");
        isGrappling = !isGrappling;
        lineRenderer.enabled = isGrappling;
        if (isGrappling)
            grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, lRPoint);
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, lRPoint);
    }

    void Update()
    {
        if (!isGrappling) return;
        lineRenderer.SetPosition(0, grappleElevationObject.TransformPoint(grappleMuzzleOffset));
        lineRenderer.SetPosition(1, lRPoint);

        grappleElevationObject.LookAt(lRPoint);
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;
        Vector3 grappleDir = (lRPoint - vehicleRigidbody.transform.position).normalized;
        float relativeVelocity = Vector3.Dot(vehicleRigidbody.linearVelocity, grappleDir);

        if (relativeVelocity < 0f)
        {
            vehicleRigidbody.linearVelocity -= grappleDir * relativeVelocity;
        }

        float dist = Vector3.Distance(vehicleRigidbody.transform.position, lRPoint);
        if (dist > grappleDistance)
        {
            Vector3 desiredPosition = lRPoint - grappleDir * grappleDistance;
            Vector3 correctionVelocity = (desiredPosition - vehicleRigidbody.transform.position) / Time.fixedDeltaTime;
            vehicleRigidbody.linearVelocity += correctionVelocity;
        }

        if (dist < grappleDistance)
        {
            grappleDistance = dist;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(grappleElevationObject.position + grappleMuzzleOffset, 0.1f);
    }
}