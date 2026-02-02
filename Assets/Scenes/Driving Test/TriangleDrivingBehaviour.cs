using UnityEngine;
using UnityEngine.InputSystem;

public class TriangleDrivingBehaviour : MonoBehaviour
{
    #region Component refs
    private Rigidbody rb;
    [SerializeField] private Transform RotationRoot;
    #endregion
    #region Ground normal vars
    [Header("Ground normal sampling")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayCastTriangleHeight = 0.0f;
    [Tooltip("How far from the origin the triangle's tip is")]
    [SerializeField] private float rayCastTriangleTipDistance = 1.0f;
    [Tooltip("How far from the origin the triangle's base corners are")]
    [SerializeField] private float rayCastTriangleBaseDistance = 0.5f;
    [Tooltip("Width of the triangle's base")]
    [SerializeField] private float rayCastTriangleBaseWidth = 0.5f;
    [SerializeField] private bool reverseTriangleOrientation = false;
    [Tooltip("Height above the triangle vertices where the raycasts start")]
    [SerializeField] private float rayCastStartHeight = 3f;
    [Tooltip("Height below the triangle vertices where the raycasts end")]
    [SerializeField] private float rayCastEndHeight = 3f;
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private bool showGizmosSelected = false;

    private Vector3 triPoint0 =>
        transform.position +
        // y
        transform.up * rayCastTriangleHeight +
        // z
        transform.forward * rayCastTriangleTipDistance * (reverseTriangleOrientation ? -1f : 1f);

    private Vector3 triPoint1 =>
        transform.position +
        // y
        transform.up * rayCastTriangleHeight +
        // z
        transform.forward * rayCastTriangleBaseDistance * (reverseTriangleOrientation ? 1f : -1f) +
        // x
        transform.right * (rayCastTriangleBaseWidth / (reverseTriangleOrientation ? -2f : 2f));

    private Vector3 triPoint2 =>
        transform.position +
        // y
        transform.up * rayCastTriangleHeight +
        // z
        transform.forward * rayCastTriangleBaseDistance * (reverseTriangleOrientation ? 1f : -1f) +
        // x
        transform.right * (rayCastTriangleBaseWidth / (reverseTriangleOrientation ? 2f : -2f));

    private Vector3[] rayCastHPs = new Vector3[3];

    #endregion

    #region Input vars
    private Vector2 moveInputVector;

    #endregion
    
    #region Movement vars
    [Header("Movement")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField][Range(0f, 1f)] private float inAirAccelerationModifier = 0.1f;
    [SerializeField] private float turningSpeed = 5f;
    [SerializeField][Range(0f, 1f)] private float inAirTurningModifier = 0.1f;
    [Tooltip("How fast the vehicle uprights itself when in the air")]
    [SerializeField] private float inAirUprightingSpeed = 5f;
    private Vector3 groundNormal;
    private bool isGrounded;
    #endregion

    #region Input
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInputVector = context.ReadValue<Vector2>();
    }
    #endregion

    #region Unity methods
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        ProcessRayCasts();
        ProcessMovement();
    }
    #endregion

    #region Movement
    private void ProcessRayCasts()
    {
        bool hit0 = Physics.Raycast(
            triPoint0 + Vector3.up * rayCastStartHeight,
            Vector3.down,
            out RaycastHit hitInfo0,
            rayCastStartHeight + rayCastEndHeight,
            groundLayer);
        
        bool hit1 = Physics.Raycast(
            triPoint1 + Vector3.up * rayCastStartHeight,
            Vector3.down,
            out RaycastHit hitInfo1,
            rayCastStartHeight + rayCastEndHeight,
            groundLayer);

        bool hit2 = Physics.Raycast(
            triPoint2 + Vector3.up * rayCastStartHeight,
            Vector3.down,
            out RaycastHit hitInfo2,
            rayCastStartHeight + rayCastEndHeight,
            groundLayer);

        rayCastHPs[0] = hitInfo0.point;
        rayCastHPs[1] = hitInfo1.point;
        rayCastHPs[2] = hitInfo2.point;

        int hitCount = (hit0 ? 1 : 0) + (hit1 ? 1 : 0) + (hit2 ? 1 : 0);
        isGrounded = hitCount >= 2;

        if (hitCount == 3)
        {
            groundNormal = Vector3.Cross(
                hitInfo1.point - hitInfo0.point,
                hitInfo2.point - hitInfo0.point
            ).normalized;
        }
        else
        {
            groundNormal = Vector3.Lerp(
                groundNormal,
                Vector3.up,
                Time.fixedDeltaTime * inAirUprightingSpeed
            );
        }
    }
    private void ProcessMovement()
    {
        // Align vehicle to ground normal
        RotationRoot.rotation = Quaternion.FromToRotation(Vector3.up, groundNormal) * Quaternion.LookRotation(transform.forward);

        rb.angularVelocity = rb.rotation * new Vector3(
            0f,
            moveInputVector.x * turningSpeed * (isGrounded ? 1f : inAirTurningModifier),
            0f
        );

        rb.AddForce(
            RotationRoot.forward * moveInputVector.y * acceleration * (isGrounded ? 1f : inAirAccelerationModifier),
            ForceMode.Acceleration
        );
    }
    #endregion

    #region Debug
    public void OnDrawGizmos()
    {
        if (showGizmos) DrawGizmos();
    }
    public void OnDrawGizmosSelected()
    {
        if (showGizmosSelected) DrawGizmos();
    }
    private void DrawGizmos()
    {
        // Draw ground sample triangle
        Gizmos.color = Color.cyan;
        Gizmos.DrawLineStrip(new Vector3[]
        {
            triPoint0,
            triPoint1,
            triPoint2
        }, true);

        // Draw raycasts
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineList(
            new Vector3[]
            {
                triPoint0 + Vector3.up * rayCastStartHeight,
                triPoint0 - Vector3.up * rayCastEndHeight,

                triPoint1 + Vector3.up * rayCastStartHeight,
                triPoint1 - Vector3.up * rayCastEndHeight,

                triPoint2 + Vector3.up * rayCastStartHeight,
                triPoint2 - Vector3.up * rayCastEndHeight,
            }
        );

        // Draw ground normal
        Gizmos.color = Color.magenta;
        Gizmos.DrawLineStrip(rayCastHPs, true);
        Gizmos.DrawLine(
            (rayCastHPs[0] + rayCastHPs[1] + rayCastHPs[2]) / 3f,
            (rayCastHPs[0] + rayCastHPs[1] + rayCastHPs[2]) / 3f + groundNormal * 2f
        );

        // Draw rotated up
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            transform.position,
            transform.position + RotationRoot.up * 5f
        );

        // Draw rotated forward
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position,
            transform.position + RotationRoot.forward * 5f
        );

        // Draw rotated right
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + RotationRoot.right * 5f
        );
    }
    #endregion
}
