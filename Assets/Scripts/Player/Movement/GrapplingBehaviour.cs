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
    private Vector3 cachedGrapplePoint = Vector3.zero;
    [SerializeField] private IGrappleable grappleable = null;
    private float grappleDistance = 0f;
    private bool isInGrappleRange = false;
    private bool isGrappling = false;
    public float TimeSinceGrapple { get; private set;}

    // Audio refs
    [SerializeField] private PlayerAudio playerAudio;

    public void GrappleInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isInGrappleRange) StartGrappple();
        }
        else if (context.canceled)
        {
            EndGrapple(true);
        }

        if (isGrappling)
            grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, grappleable.GetGrapplePoint(this));
    }
    public void StartGrappple(Grappleable grappleable = null)
    {
        if (grappleable != null) this.grappleable = grappleable;

        isGrappling = true;
        TimeSinceGrapple = 0f;

        // Start grapple audio
        playerAudio.GrappleStart();

        lineRenderer.enabled = true;
        if (grappleHook) grappleHook.SetActive(false);
    }
    public void EndGrapple(bool respectLock)
    {
        if (respectLock && grappleable != null && grappleable.IsLocking) return;

        isGrappling = false;

        // Change audio behaviour
        playerAudio.GrappleEnd();

        lineRenderer.enabled = false;
        if (grappleHook) grappleHook.SetActive(true);
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (grappleable != null)
            grappleDistance = Vector3.Distance(vehicleRigidbody.transform.position, grappleable.GetGrapplePoint(this));
    }

    void Update()
    {
        if (grappleable != null) cachedGrapplePoint = grappleable.GetGrapplePoint(this);

        if (isGrappling)
        {
            TimeSinceGrapple += Time.deltaTime;
            lineRenderer.SetPosition(0, grappleElevationObject.TransformPoint(grappleMuzzleOffset));
            lineRenderer.SetPosition(1, cachedGrapplePoint);
        }

        if (isGrappling || isInGrappleRange)
        {
            // I know it's ugly but it works
            grappleAzimuthObject.LookAt(cachedGrapplePoint, grappleAzimuthObject.parent.up);
            grappleAzimuthObject.localEulerAngles = new Vector3(0, grappleAzimuthObject.localEulerAngles.y, 0);
            grappleElevationObject.LookAt(cachedGrapplePoint, grappleAzimuthObject.up);
        }

        if (isInGrappleRange)
        {

            Vector3 diff = cachedGrapplePoint - playerCamera.transform.position;

            grappleUIIndicator.gameObject.SetActive(Vector3.Dot(playerCamera.transform.forward, diff.normalized) > 0f);

            Vector2 viewPortpoint = playerCamera.WorldToViewportPoint(cachedGrapplePoint);
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
    public void EnteredGrappleRange(IGrappleable grappleable)
    {
        this.grappleable = grappleable;
        isInGrappleRange = true;
    }
    public void ExitedGrappleRange(IGrappleable grappleable)
    {
        this.grappleable = null;
        grappleUIIndicator.gameObject.SetActive(false);
        isInGrappleRange = false;
    }
    void FixedUpdate()
    {
        if (!isGrappling || grappleable == null) return;

        cachedGrapplePoint = grappleable.GetGrapplePoint(this);

        Vector3 grappleDir = (cachedGrapplePoint - vehicleRigidbody.transform.position).normalized;
        float relativeVelocity = Vector3.Dot(vehicleRigidbody.linearVelocity, grappleDir);

        if (relativeVelocity < 0f)
        {
            vehicleRigidbody.linearVelocity -= grappleDir * relativeVelocity;
        }

        float dist = Vector3.Distance(vehicleRigidbody.transform.position, cachedGrapplePoint);
        if (dist > grappleDistance)
        {
            Vector3 desiredPosition = cachedGrapplePoint - grappleDir * grappleDistance;
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