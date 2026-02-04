using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Component refs
    private Rigidbody rb;
    [SerializeField] private Transform RotationRoot;
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
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private bool showGizmosSelected = false;

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
    #endregion
    
    #region Movement vars
    [Header("Movement")]
    [SerializeField] private float topSpeed = 100f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField][Range(0f, 1f)] private float inAirAccelerationModifier = 0.1f;
    [SerializeField] private float turningSpeed = 3f;
    [SerializeField][Range(0f, 1f)] private float inAirTurningModifier = 0.1f;
    [Tooltip("How fast the vehicle uprights itself when in the air")]
    [SerializeField] private float inAirUprightingSpeed = 1f;
    private Vector3 groundNormal;
    private bool isGrounded;
    #endregion

    #region Input
    public void TurnInput(InputAction.CallbackContext context)
    {
        moveInputVector.x = context.ReadValue<float>();
    }

    public void GasInput(InputAction.CallbackContext context)
    {
        moveInputVector.y = context.performed ? 1 : 0;
    }

    public void ReverseInput(InputAction.CallbackContext context)
    {
        moveInputVector.y = context.performed ? -1 : 0;
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
            groundNormal = Vector3.Lerp(
                groundNormal,
                Vector3.up,
                Time.fixedDeltaTime * inAirUprightingSpeed
            ).normalized;
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

        if (rb.linearVelocity.magnitude > topSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * topSpeed;
        }
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
