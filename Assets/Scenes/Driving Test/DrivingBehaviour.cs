using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingBehaviour : MonoBehaviour
{
    [SerializeField] private float turningSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    private Vector2 inputVector;
    private Rigidbody rb;
    private bool isGrounded = true;

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.angularVelocity = new Vector3(0f, inputVector.x * turningSpeed, 0f);
            rb.AddRelativeForce(Vector3.left * inputVector.y * acceleration, ForceMode.Acceleration);
        }
        Debug.Log("Speed: " + rb.linearVelocity.magnitude);
    }
}
