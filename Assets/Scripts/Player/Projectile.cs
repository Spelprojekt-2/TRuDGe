using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private GameObject shooter;

    public void SetDirection(GameObject shooter)
    {
        StartCoroutine(DeathTimer());
        this.shooter = shooter;
        rb.linearVelocity =  transform.forward * projectileSpeed;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Vector3 force = (transform.position - col.transform.position).normalized * 30f;
            force.y = 0;
            col.GetComponentInParent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
