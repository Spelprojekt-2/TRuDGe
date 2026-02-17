using UnityEngine;

public class SpinningWheel : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Rigidbody vehicleRigidBody;
    [SerializeField] float wheelRadius;
    [SerializeField] float xLocationOverride;

    void Update()
    {
        float velocity = playerMovement.GetCurrentSpeed(false);
        velocity += vehicleRigidBody.angularVelocity.y * xLocationOverride;
        velocity *= Time.deltaTime;
        float wheelCircumference = wheelRadius * 2 * Mathf.PI;
        
        Vector3 eulers = new Vector3(
            velocity/wheelCircumference * 360,
            0, 0
        );

        transform.Rotate(eulers);
    }
}
