using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;

    public void SetDirection(Vector3 dir)
    {
        StartCoroutine(CollisionWakeUp());
        rb.linearVelocity = dir * projectileSpeed;
    }

    IEnumerator CollisionWakeUp()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}
