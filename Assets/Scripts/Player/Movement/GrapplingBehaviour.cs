using UnityEngine;
using UnityEngine.InputSystem;

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
    [Tooltip("An aesthetic projectile on the end of the barrel when the vehicle isn't grappling")]
    [SerializeField] private GameObject grappleHook;
    #endregion
    [Header("Animation")]
    [Tooltip("The speed at which the grapple gun returns to facing forward")]
    [SerializeField] private float idleRotationSpeed = 8f;
    [Header("Debug")]
    [SerializeField] private Vector3 grapplePoint = Vector3.zero;
    private float grappleDistance = 0f;
    private bool isInGrappleRange = false;
    private bool isGrappling = false;

    // Audio refs
    [SerializeField] private PlayerAudio playerAudio;

    public void GrappleInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isInGrappleRange) return;
            isGrappling = true;

            // Start grapple audio
            playerAudio.GrappleStart();

            if (grappleHook) grappleHook.SetActive(false);
        }
        else if (context.canceled)
        {
            isGrappling = false;

            // Change audio behaviour
            playerAudio.GrappleEnd();

            if (grappleHook) grappleHook.SetActive(true);
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
        }

        if (isGrappling || isInGrappleRange)
        {
            // I know it's ugly but it works
            grappleAzimuthObject.LookAt(grapplePoint, grappleAzimuthObject.parent.up);
            grappleAzimuthObject.localEulerAngles = new Vector3(0, grappleAzimuthObject.localEulerAngles.y, 0);
            grappleElevationObject.LookAt(grapplePoint, grappleAzimuthObject.up);
        }

        if (isInGrappleRange)
        {

            Vector3 diff = grapplePoint - playerCamera.transform.position;

            grappleUIIndicator.gameObject.SetActive(Vector3.Dot(playerCamera.transform.forward, diff.normalized) > 0f);

            Vector2 viewPortpoint = playerCamera.WorldToViewportPoint(grapplePoint);
            viewPortpoint.x = Mathf.Clamp(viewPortpoint.x, 0, 1);
            viewPortpoint.y = Mathf.Clamp(viewPortpoint.y, 0, 1);

            if (playerCamera.rect.width == 1 && playerCamera.rect.height == 0.5)
            {
                viewPortpoint.x = (viewPortpoint.x * 2) - 0.5f;
                grappleUIIndicator.anchoredPosition = viewPortpoint * new Vector2(
                    playerCamera.scaledPixelWidth / playerCamera.rect.width,
                    playerCamera.scaledPixelHeight / playerCamera.rect.height);
            }
            else
            {
                grappleUIIndicator.anchoredPosition = viewPortpoint * new Vector2(
                    playerCamera.scaledPixelWidth / playerCamera.rect.width,
                    playerCamera.scaledPixelHeight / playerCamera.rect.height);
            }
        }
        else
        {
            if (!isGrappling)
            {
                if (grappleAzimuthObject.localEulerAngles.y != 0)
                {
                    // I know it's ugly but it works
                    grappleAzimuthObject.localEulerAngles = new Vector3(
                        0, Mathf.LerpAngle(
                            grappleAzimuthObject.localEulerAngles.y,
                            0, idleRotationSpeed * Time.deltaTime), 0);
                }
                if (grappleElevationObject.localEulerAngles.x != 0)
                {
                    grappleElevationObject.localEulerAngles = new Vector3(
                        Mathf.LerpAngle(
                            grappleElevationObject.localEulerAngles.x,
                            0, idleRotationSpeed * Time.deltaTime), 0, 0);
                }
            }
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