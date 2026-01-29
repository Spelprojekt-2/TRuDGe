using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class DrivingBehaviour : MonoBehaviour
{
    [SerializeField] private Collider groundCollider;
    [SerializeField] private float turningSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    private Vector2 inputVector;
    private Rigidbody rb;
    [SerializeField]private bool isGrounded = true;

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        Debug.Log("Input Vector: " + inputVector);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.angularVelocity = rb.rotation * new Vector3(0f, inputVector.x * turningSpeed, 0f);
            rb.AddRelativeForce(Vector3.forward * inputVector.y * acceleration, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
