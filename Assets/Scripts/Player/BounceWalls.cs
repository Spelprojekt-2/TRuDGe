using UnityEngine;

public class BounceWalls : MonoBehaviour
{
    private PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            ContactPoint contact = collision.GetContact(0);
            Vector3 hitPoint = contact.point;

            Vector3 pushDirection = (transform.position - hitPoint).normalized;

            pushDirection.y = 0;

            float speedForce = movement.GetCurrentSpeed() / 2f;
            GetComponent<Rigidbody>().AddForce(pushDirection * speedForce, ForceMode.VelocityChange);
        }
    }
}
