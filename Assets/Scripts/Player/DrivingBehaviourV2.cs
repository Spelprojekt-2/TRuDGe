using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingBehaviourV2 : MonoBehaviour
{
    [SerializeField] private float turningSpeedMultiplier = 2.5f;
    [SerializeField] private float minimumTurningSpeed = 10f;
    [SerializeField] private float acceleration = 0.05f;
    [SerializeField] private float topSpeed = 50f;
    [SerializeField] private float deceleration = 10f;
    private Vector3 velocity;
    private Vector2 driveInputVector;
    private Rigidbody rb;
    private bool isGrounded = true;

    public void MoveInput(InputAction.CallbackContext context)
    {
        driveInputVector = context.ReadValue<Vector2>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (driveInputVector.y != 0)
        {
            velocity += transform.forward * driveInputVector.y * acceleration; // Gas
            if (velocity.magnitude > topSpeed)
            {
                velocity = velocity.normalized * topSpeed;
            }
        }
        else // Break
        {
            velocity /= deceleration;
        }
        if (velocity.magnitude < 0.01f) velocity = Vector3.zero;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if (driveInputVector.x != 0) // Turn
        {
            Vector3 rotate = new Vector3(0f, driveInputVector.x * turningSpeedMultiplier * (1 / Mathf.Max(velocity.magnitude, minimumTurningSpeed)), 0f);
            transform.Rotate(rotate);
            velocity = Quaternion.Euler(rotate) * velocity;
        }

        Debug.Log("Speed: " + velocity.magnitude);
    }
}
