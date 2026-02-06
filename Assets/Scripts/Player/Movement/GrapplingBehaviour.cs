using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingBehaviour : MonoBehaviour
{
    #region Component refs
    [SerializeField] private RectTransform grappleUIIndicator;
    [SerializeField] private Camera playerCamera;
    private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody vehicleRigidbody;
    [Tooltip("The object that will follow the grapple hook's azimuth/heading/yaw rotation (grapple turret)")]
    [SerializeField] private Transform grappleAzimuthObject;
    [Tooltip("The object that will follow the grapple hook's elevation/pitch rotation (grapple barrel)")]
    [SerializeField] private Transform grappleElevationObject;
    [Tooltip("Location from which the grapple hook is fired")]
    [SerializeField] private Vector3 grappleMuzzleOffset = Vector3.zero;
    #endregion

    [SerializeField] private Vector3 grapplePoint = Vector3.zero;
    private float grappleDistance = 0f;
    private bool isInGrappleRange = false;
    private bool isGrappling = false;
    public void Toggle()
    {
        if (isGrappling) isGrappling = false;
        else
        {
            if (!isInGrappleRange) return;
            isGrappling = true;
        }

        lineRenderer.enabled = isGrappling;
        if (isGrappling)
            grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, grapplePoint);
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, grapplePoint);
    }

    void Update()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, grappleElevationObject.TransformPoint(grappleMuzzleOffset));
            lineRenderer.SetPosition(1, grapplePoint);
            
            grappleElevationObject.LookAt(grapplePoint);
        }


        if (isInGrappleRange)
        {
            Vector3 diff = grapplePoint - playerCamera.transform.position;
            grappleUIIndicator.gameObject.SetActive(Vector3.Dot(playerCamera.transform.forward, diff.normalized) > 0f);
            Vector2 pointOnScreen = playerCamera.WorldToScreenPoint(grapplePoint);
            grappleUIIndicator.anchoredPosition = pointOnScreen - new Vector2(Screen.width, Screen.height) / 2f;
        }
    }
    public void EnteredGrappleRange(Grappleable grappleable)
    {
        grapplePoint = grappleable.GrapplePoint;
        isInGrappleRange = true;
    }
    public void ExitedGrappleRange(Grappleable grappleable)
    {
        grappleUIIndicator.gameObject.SetActive(false);
        grapplePoint = grappleable.GrapplePoint;
        isInGrappleRange = false;
    }
    void FixedUpdate()
    {
        if (!isGrappling) return;
        Vector3 grappleDir = (grapplePoint - vehicleRigidbody.transform.position).normalized;
        float relativeVelocity = Vector3.Dot(vehicleRigidbody.linearVelocity, grappleDir);

        if (relativeVelocity < 0f)
        {
            vehicleRigidbody.linearVelocity -= grappleDir * relativeVelocity;
        }

        float dist = Vector3.Distance(vehicleRigidbody.transform.position, grapplePoint);
        if (dist > grappleDistance)
        {
            Vector3 desiredPosition = grapplePoint - grappleDir * grappleDistance;
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