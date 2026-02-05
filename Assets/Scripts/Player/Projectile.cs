using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private GameObject shooter;
    private Transform target = null;

    public void PrepareProjectile(GameObject shooter, Transform target)
    {
        StartCoroutine(DeathTimer());
        this.shooter = shooter;
        this.target = target;
    }

    private void FixedUpdate()
    {
        Debug.Log(target == null);
        rb.linearVelocity = transform.forward * projectileSpeed;
        if (target != null)
        {
            target.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(target.position);
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }

        Debug.Log("");
        if (col.transform.root.CompareTag("Player"))
        {
            Vector3 force = (transform.position - col.transform.position).normalized * 30f;
            force.y = 0;
            col.GetComponentInParent<Rigidbody>().AddForceAtPosition(force, transform.position, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
