using System.Collections.Generic;
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
    [SerializeField] private ShowGizmoEnum showForces = ShowGizmoEnum.Always;

    private const int RIG_POINT_COUNT = 4;
    private Vector3[] rigPoints => new Vector3[]
    {
        // P0
        transform.position +
        // y
        transform.up * rayCastRigPosition.y +
        // z
        transform.forward * (rayCastRigPosition.x + rayCastRigSize.y / (flipRaycastRigZ ? -2f : 2f)) +
        // x
        transform.right * (rayCastRigSize.x / (flipRaycastRigZ ? -2f : 2f)),
        
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
        transform.right * (-rayCastRigSize.x / (flipRaycastRigZ ? -2f : 2f)),

        // P3
        transform.position +
        // y
        transform.up * rayCastRigPosition.y +
        // z
        transform.forward * (rayCastRigPosition.x + rayCastRigSize.y / (flipRaycastRigZ ? -2f : 2f)) +
        // x
        transform.right * (-rayCastRigSize.x / (flipRaycastRigZ ? -2f : 2f))
    };

    private Vector3[] rayCastHPs = new Vector3[RIG_POINT_COUNT];
    bool[] didHits = new bool[RIG_POINT_COUNT];


    #endregion

    #region Input vars
    private Vector2 moveInputVector;
    private bool accelerationInput;
    private bool reversingInput;
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
    [Header("Friction")]
    [Tooltip("The maximum friction acting sideways on the vehicle")]
    [SerializeField] private float maxSidewaysFriction = 10f;
    // [Tooltip("Curve to modify sideways friction based on sideways velocity")]
    // [SerializeField] private AnimationCurve sidewaysFrictionOverSpeedModifier = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [Tooltip("The required directional velocity required to reach max friction")]
    [SerializeField] private float velocityForMaxSidewaysFriction = 30f;
    [Tooltip("The maximum friction acting forward or backwards on the vehicle when the vehicle is not accelerating against the friction direction")]
    [SerializeField] private float maxForwardFriction = 10f;
    // [Tooltip("Curve to modify forward friction based on forward velocity")]
    // [SerializeField] private AnimationCurve forwardFrictionOverSpeedModifier = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [Tooltip("The required directional velocity required to reach max friction")]
    [SerializeField] private float velocityForMaxForwardFriction = 30f;
    [Header("Uprighting")]
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
        accelerationInput = context.performed;
    }

    public void ReverseInput(InputAction.CallbackContext context)
    {
        reversingInput = context.performed;
    }
    #endregion

    #region Unity methods
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        moveInputVector.y = Mathf.Clamp((accelerationInput ? 1 : 0) - (reversingInput ? 1 : 0), -1f, 1f);
    }

    public void FixedUpdate()
    {
        ProcessRayCasts();
        ProcessMovement();
        ProcessFriction();
    }
    #endregion

    #region Movement
    private void ProcessRayCasts()
    {
        RaycastHit[] hitInfos = new RaycastHit[RIG_POINT_COUNT];

        for (int i = 0; i < RIG_POINT_COUNT; i++)
        {
            didHits[i] = Physics.Raycast(
                rigPoints[i],
                Vector3.down,
                out hitInfos[i],
                rayCastLength,
                groundLayer);
        }

        int hitCount = 0;
        for (int i = 0; i < RIG_POINT_COUNT; i++)
        {
            if (!didHits[i]) continue;

            hitCount++;
            rayCastHPs[i] = hitInfos[i].point;
        }

        isGrounded = hitCount >= 2;

        switch (hitCount)
        {
            case 4:
            {
                Vector3 normalCandidate0 = Vector3.Cross(
                rayCastHPs[0] - rayCastHPs[1],
                rayCastHPs[0] - rayCastHPs[3]
                ).normalized;

                Vector3 normalCandidate1 = Vector3.Cross(
                rayCastHPs[3] - rayCastHPs[0],
                rayCastHPs[3] - rayCastHPs[2]
                ).normalized;

                if (Vector3.Dot(rayCastHPs[0] - rayCastHPs[2], normalCandidate0) >= 0)
                {
                    Vector3 mirrorNormal = Vector3.Cross(
                        rayCastHPs[2] - rayCastHPs[3],
                        rayCastHPs[2] - rayCastHPs[1]
                    ).normalized;
                    groundNormal = (normalCandidate0 + mirrorNormal).normalized;
                }
                else
                {
                    Vector3 mirrorNormal = Vector3.Cross(
                        rayCastHPs[1] - rayCastHPs[2],
                        rayCastHPs[1] - rayCastHPs[0]
                    ).normalized;
                    groundNormal = (normalCandidate1 + mirrorNormal).normalized;
                }
            } break;
            case 3:
            {
                Vector3[] points = new Vector3[3];
                int pointI = 0;

                for (int i = 0; i < RIG_POINT_COUNT; i++)
                {
                    if(!didHits[i]) continue;

                    points[pointI++] = rayCastHPs[i];
                }

                groundNormal = Vector3.Cross(
                    points[0] - points[1],
                    points[0] - points[2]
                ).normalized;
            } break;
            default:
            {
                groundNormal = Vector3.up;
            } break;
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

        // Move vehicle up if too low (avoid clipping)
        int groPoiCount = 0;
        float groundHeight = 0;
        for (int i = 0; i < RIG_POINT_COUNT; i++)
        {
            if (!didHits[i]) continue;

            groPoiCount++;
            groundHeight += rayCastHPs[i].y;
        }
        groundHeight = groundHeight/groPoiCount - transform.position.y;
        if (groundHeight >= 0)
        rotationRoot.localPosition = new Vector3(0,Mathf.Lerp(rotationRoot.localPosition.y,groundHeight,Time.deltaTime*5),0);

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
    private void ProcessFriction()
    {
        if (!isGrounded) return;

        // Sideways friction
        float rightVel = Vector3.Dot(rotationRoot.right, rb.linearVelocity);
        rb.AddForce(
            rotationRoot.right * (-1 * maxSidewaysFriction * Mathf.Clamp(rightVel / velocityForMaxSidewaysFriction, -1, 1)),
            ForceMode.Acceleration
        );

        // Forward friction
        float forwardVel = Vector3.Dot(rotationRoot.forward, rb.linearVelocity);
        // Don't apply if tank is accelerating
        if (Mathf.Sign(forwardVel) != moveInputVector.y)
        {
            rb.AddForce(
                rotationRoot.forward * (-1 * maxForwardFriction * Mathf.Clamp(forwardVel / velocityForMaxForwardFriction, -1, 1)),
                ForceMode.Acceleration
            );
        }
    }
    #endregion

    #region Audio
    public float GetNormalizedSpeed()
    {
        float forwardSpeed = Mathf.Abs(Vector3.Dot(rb.linearVelocity, rotationRoot.forward));
        return Mathf.Clamp01(forwardSpeed / topSpeed);
    }
    #endregion

    #region Debug
    public void OnDrawGizmos()
    {
        if (showRig == ShowGizmoEnum.Always) DrawRigGizmos();
        if (showGroundSample == ShowGizmoEnum.Always) DrawGroundSampleGizmos();
        if (showLocalAxes == ShowGizmoEnum.Always) DrawLocalAxesGizmos();
        if (showForces == ShowGizmoEnum.Always) DrawForces();
    }
    public void OnDrawGizmosSelected()
    {
        if (showRig == ShowGizmoEnum.Selected) DrawRigGizmos();
        if (showGroundSample == ShowGizmoEnum.Selected) DrawGroundSampleGizmos();
        if (showLocalAxes == ShowGizmoEnum.Selected) DrawLocalAxesGizmos();
        if (showForces == ShowGizmoEnum.Selected) DrawForces();
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
                rigPoints[2] + Vector3.down * rayCastLength,

                rigPoints[3],
                rigPoints[3] + Vector3.down * rayCastLength
            }
        );
    }
    private void DrawGroundSampleGizmos()
    {
        // Draw ground normal
        Gizmos.color = Color.magenta;

        List<Vector3> points = new List<Vector3>();
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < RIG_POINT_COUNT; i++)
        {
            if (!didHits[i]) continue;
            points.Add(rayCastHPs[i]);
            sum += rayCastHPs[i];
        }

        Gizmos.DrawLineStrip(points.ToArray(), true);
        // Gizmos.DrawLine(
        //     (points[0] + points[1] + points[2]) / 3f,
        //     (points[0] + points[1] + points[2]) / 3f + groundNormal * 2f
        // );
        Gizmos.DrawLine(
            sum / points.Count,
            sum / points.Count + groundNormal * 2f
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
    private void DrawForces()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            transform.position,
            transform.position +
            transform.up * Vector3.Dot(rb.linearVelocity, rotationRoot.forward) * 0.1f
        );
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + 
            transform.right * Vector3.Dot(rb.linearVelocity, rotationRoot.right) * 0.1f
        );
        
        if (isGrounded)
        {
            Gizmos.color = Color.cyan;
            float rightVel = Vector3.Dot(rotationRoot.right, rb.linearVelocity);
            Gizmos.DrawLine(
                transform.position,
                transform.position +
                rotationRoot.right * (-0.1f * maxSidewaysFriction * Mathf.Clamp(rightVel / velocityForMaxForwardFriction, -1, 1))
            );

            Gizmos.color = Color.yellow;
            float forwardVel = Vector3.Dot(rotationRoot.forward, rb.linearVelocity);
            // Don't apply if tank is accelerating
            if (Mathf.Sign(forwardVel) != moveInputVector.y)
            {
                Gizmos.DrawLine(
                    transform.position,
                    transform.position +
                    rotationRoot.up * (-0.1f * maxForwardFriction * Mathf.Clamp(forwardVel / velocityForMaxSidewaysFriction, -1, 1))
                );
            }
        }
    }
    #endregion
}
