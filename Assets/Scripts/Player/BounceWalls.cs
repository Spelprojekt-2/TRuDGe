using UnityEngine;

public class BounceWalls : MonoBehaviour
{
    private PlayerMovement movement;

    [Header("Wall Bump Settings")]
    [SerializeField] private float minBumpForce = 30f;
    [SerializeField] private float maxBumpForce = 80f;
    [SerializeField] private float speedDivisor = 2f;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            float speedBeforeHit = rb.linearVelocity.magnitude;

            ContactPoint contact = collision.GetContact(0);
            Vector3 pushDirection = (transform.position - contact.point).normalized;
            pushDirection.y = 0;

            float rawSpeedForce = movement.GetCurrentSpeed() / speedDivisor;
            float clampedForce = Mathf.Clamp(rawSpeedForce, minBumpForce, maxBumpForce);
            rb.linearVelocity *= 0.5f;

            rb.AddForce(pushDirection * clampedForce, ForceMode.Impulse);

            if (rb.linearVelocity.magnitude > speedBeforeHit)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speedBeforeHit;
            }

            rb.linearVelocity *= 0.9f;
        }
    }
}
