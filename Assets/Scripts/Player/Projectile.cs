using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    private GameObject shooter;
    private Transform target = null;
    private bool isFalling = false;

    public void PrepareProjectile(GameObject shooter, Transform target)
    {
        if (target == null)
        {
            StartCoroutine(DeathTimer());
        }
        
        this.shooter = shooter;
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (isFalling) return;

        rb.linearVelocity = transform.forward * projectileSpeed;

        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPos);

            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer))
            {
                transform.position += Vector3.up * 1f;
            }
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = true;
        rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }
        if (col.transform.root.CompareTag("Player"))
        {
            Vector3 force = (transform.position - col.transform.position).normalized * 30f;
            force.y = 0;
            col.GetComponentInParent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
