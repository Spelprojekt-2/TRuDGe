using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private GameObject shooter;

    public void SetDirection(Vector3 dir, GameObject shooter)
    {
        StartCoroutine(DeathTimer());
        this.shooter = shooter;
        if(dir.y < 0)
        {
            dir.y = 0;
        }
        rb.linearVelocity = dir * projectileSpeed;
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
            col.GetComponentInParent<Rigidbody>().AddForce((transform.position - col.transform.position).normalized * 15f, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
