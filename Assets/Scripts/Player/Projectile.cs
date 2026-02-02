using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;

    public void SetDirection(Vector3 dir)
    {
        rb.linearVelocity = dir * projectileSpeed;
    }
}
