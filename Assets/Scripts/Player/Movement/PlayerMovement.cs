using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Component refs
    private Rigidbody rb;
    [SerializeField] private Transform rotationRoot;
    #endregion

    #region Ground normal vars
    [Header("Ground normal sampling")]
    [SerializeField] private LayerMask groundLayer;
    // [SerializeField] private float rayCastRigHeight = 3f;
    [Tooltip("Position of the raycast rig relative to the vehicle's origin (x: forward/back, y: up/down)")]
    [SerializeField] private Vector2 rayCastRigPosition = new Vector2(0f, 3f);
    [Tooltip("Size of the raycast rig (x: width, y: depth)")]
    [SerializeField] private Vector2 rayCastRigSize = new Vector2(3f, 4f);
    [SerializeField] private bool flipRaycastRigZ = false;
    [Tooltip("How long the raycasts are")]
    [SerializeField] private float rayCastLength = 6f;

    [Header("Gizmo display settings")]
    [SerializeField] private ShowGizmoEnum showRig = ShowGizmoEnum.Selected;
    [SerializeField] private ShowGizmoEnum showGroundSample = ShowGizmoEnum.Always;
    [SerializeField] private ShowGizmoEnum showLocalAxes = ShowGizmoEnum.Never;

    private Vector3[] rigPoints => new Vector3[3]
    {
        // P0
        transform.position +
        // y
        transform.up * rayCastRigPosition.y +
        // z
        transform.forward * (rayCastRigPosition.x + rayCastRigSize.y / (flipRaycastRigZ ? -2f : 2f)),
        
        // P1
        transform.position +
        // y
        transform.up * rayCastRigPosition.y +
        // z
        transform.forward * (rayCastRigPosition.x - rayCastRigSize.y / (flipRaycastRigZ ? -2f : 2f)) +
        // x
        transform.right * (rayCastRigSize.x / (flipRaycastRigZ ? -2f : 2f)),

        // P2
        transform.position +
        // y
        transform.up * rayCastRigPosition.y +
        // z
        transform.forward * (rayCastRigPosition.x - rayCastRigSize.y / (flipRaycastRigZ ? -2f : 2f)) +
        // x
        transform.right * (-rayCastRigSize.x / (flipRaycastRigZ ? -2f : 2f))
    };

    private Vector3[] rayCastHPs = new Vector3[3];

    #endregion

    #region Input vars
    private Vector2 moveInputVector;
    private float accelerationInput;
    private float reversingInput;
    #endregion
    
    #region Movement vars
    [Header("Movement")]
    [Tooltip("Clamp on absolute velocity magnitude. Set to 0 to disable.")]
    [SerializeField] private float topSpeed = 100f;
    [HideInInspector] public float externalTopSpeedModifier = 1f;
    [SerializeField] private float baseAcceleration = 50f;
    [Tooltip("Curve to modify acceleration based on current speed")]
    [SerializeField] private AnimationCurve accelerationOverSpeedModifier = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField][Range(0f, 1f)] private float inAirAccelerationModifier = 0.1f;
    [HideInInspector] public bool externalIgnoreInAirAccelerationModifier = false;
    [HideInInspector] public float externalAccelerationModifier = 1f;
    [SerializeField] private float baseTurningSpeed = 3f;
    [Tooltip("Curve to modify turning speed based on current speed")]
    [SerializeField] private AnimationCurve turningSpeedOverSpeedModifier = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField][Range(0f, 1f)] private float inAirTurningModifier = 0.1f;
    [Tooltip("If true, current speed will be calculated from absolute velocity rather than forward velocity. (Does not affect top speed clamping)")]
    [SerializeField] private bool baseSpeedOnAbsoluteVelocity = false;
    [Tooltip("How fast the vehicle uprights itself when on the ground")]
    [SerializeField] private float onGroundUprightingSpeed = 5f;
    [Tooltip("How fast the vehicle uprights itself when in the air")]
    [SerializeField] private float inAirUprightingSpeed = 1f;
    private Vector3 groundNormal;
    private bool isGrounded;
    #endregion

    #region Public methods
    public float GetTopSpeed() => topSpeed;
    public bool IsGrounded() => isGrounded;
    public Vector3 GetGroundNormal() => groundNormal;
    public float GetCurrentSpeed(bool absolute = false) =>
        absolute ?
            rb.linearVelocity.magnitude :
            Vector3.Dot(rb.linearVelocity, rotationRoot.forward);
    #endregion

    #region Input
    public void TurnInput(InputAction.CallbackContext context)
    {
        moveInputVector.x = context.ReadValue<float>();
    }

    public void GasInput(InputAction.CallbackContext context)
    {
        accelerationInput = context.ReadValue<float>();
    }

    public void ReverseInput(InputAction.CallbackContext context)
    {
        reversingInput = context.ReadValue<float>();
    }
    #endregion

    #region Unity methods
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        moveInputVector.y = Mathf.Clamp(accelerationInput - reversingInput, -1f, 1f);
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
        bool[] didHits = new bool[3];
        RaycastHit[] hitInfos = new RaycastHit[3];

        for (int i = 0; i < 3; i++)
        {
            didHits[i] = Physics.Raycast(
                rigPoints[i],
                Vector3.down,
                out hitInfos[i],
                rayCastLength,
                groundLayer);
        }

        int hitCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if (!didHits[i]) continue;
            
            hitCount++;
            rayCastHPs[i] = hitInfos[i].point;
        }

        isGrounded = hitCount >= 2;

        if (hitCount == 3)
        {
            groundNormal = Vector3.Cross(
                rayCastHPs[0] - rayCastHPs[1],
                rayCastHPs[0] - rayCastHPs[2]
            ).normalized;
        }
        else
        {
            groundNormal = Vector3.up;
        }
    }
    private void ProcessMovement()
    {
        // Align vehicle to ground normal
        rotationRoot.rotation = Quaternion.Slerp(
            rotationRoot.rotation,
            Quaternion.FromToRotation(Vector3.up, groundNormal) * Quaternion.LookRotation(transform.forward),
            Time.fixedDeltaTime * (isGrounded ? onGroundUprightingSpeed : inAirUprightingSpeed)
        );

        // Turning
        rb.angularVelocity = rb.rotation * new Vector3(
            0f,
            // Input
            moveInputVector.x *
            // Base turning speed
            baseTurningSpeed *
            // Speed modifier
            turningSpeedOverSpeedModifier.Evaluate(
                baseSpeedOnAbsoluteVelocity ?
                    rb.linearVelocity.magnitude / topSpeed :
                    Vector3.Dot(rb.linearVelocity, rotationRoot.forward) / topSpeed
            ) *
            // Air modifier
            (isGrounded ? 1f : inAirTurningModifier),
            0f
        );

        // Acceleration
        rb.AddForce(
            // Direction
            rotationRoot.forward *
            // Input
            moveInputVector.y *
            // Base acceleration
            baseAcceleration * 
            // Speed modifier
            accelerationOverSpeedModifier.Evaluate(
                baseSpeedOnAbsoluteVelocity ?
                    rb.linearVelocity.magnitude / topSpeed :
                    Vector3.Dot(rb.linearVelocity, rotationRoot.forward) / topSpeed
            ) *
            // Air modifier
            (isGrounded || externalIgnoreInAirAccelerationModifier ? 1f : inAirAccelerationModifier) *
            // External modifier
            externalAccelerationModifier,
            ForceMode.Acceleration
        );

        // Max speed clamp
        if (rb.linearVelocity.magnitude != 0 && rb.linearVelocity.magnitude > topSpeed * externalTopSpeedModifier)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * topSpeed * externalTopSpeedModifier;
        }
    }
    #endregion

    #region Debug
    public void OnDrawGizmos()
    {
        if (showRig == ShowGizmoEnum.Always) DrawRigGizmos();
        if (showGroundSample == ShowGizmoEnum.Always) DrawGroundSampleGizmos();
        if (showLocalAxes == ShowGizmoEnum.Always) DrawLocalAxesGizmos();
    }
    public void OnDrawGizmosSelected()
    {
        if (showRig == ShowGizmoEnum.Selected) DrawRigGizmos();
        if (showGroundSample == ShowGizmoEnum.Selected) DrawGroundSampleGizmos();
        if (showLocalAxes == ShowGizmoEnum.Selected) DrawLocalAxesGizmos();
    }
    private void DrawRigGizmos()
    {
        // Draw ground sample rig
        Gizmos.color = Color.cyan;
        Gizmos.DrawLineStrip(rigPoints, true);

        // Draw raycasts
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineList(
            new Vector3[]
            {
                rigPoints[0],
                rigPoints[0] + Vector3.down * rayCastLength,

                rigPoints[1],
                rigPoints[1] + Vector3.down * rayCastLength,

                rigPoints[2],
                rigPoints[2] + Vector3.down * rayCastLength
            }
        );
    }
    private void DrawGroundSampleGizmos()
    {
        // Draw ground normal
        Gizmos.color = Color.magenta;

        Gizmos.DrawLineStrip(rayCastHPs, true);

        Gizmos.DrawLine(
            (rayCastHPs[0] + rayCastHPs[1] + rayCastHPs[2]) / 3f,
            (rayCastHPs[0] + rayCastHPs[1] + rayCastHPs[2]) / 3f + groundNormal * 2f
        );
    }
    private void DrawLocalAxesGizmos()
    {
        // Draw rotated up
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            transform.position,
            transform.position + rotationRoot.up * 5f
        );

        // Draw rotated forward
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position,
            transform.position + rotationRoot.forward * 5f
        );

        // Draw rotated right
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + rotationRoot.right * 5f
        );
    }
    #endregion
}
