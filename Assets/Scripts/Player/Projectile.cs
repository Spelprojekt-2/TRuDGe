using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private float currentSpeed;
    private GameObject shooter;
    private bool isFalling = false;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }
    public void PrepareProjectile(GameObject shooter, Transform target, float speedMultiplier)
    {
        if (target == null)
        {
            if(speedMultiplier < 0.5f)
            {
                speedMultiplier = 0.5f;
            }
            currentSpeed = projectileSpeed * speedMultiplier;
            StartCoroutine(DeathTimer());
        }
        else
        {
            PlayerMovement shooterMove = shooter.GetComponent<PlayerMovement>();
            if (shooterMove != null) currentSpeed = shooterMove.GetCurrentSpeed() + 10;
        }
        
        this.shooter = shooter;
    }

    private void FixedUpdate()
    {
        if (isFalling) return;

        rb.linearVelocity = transform.forward * currentSpeed;
        
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = true;
        rb.useGravity = true;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (shooter == null) return;
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }

        if (col.CompareTag("Shield"))
        {
            Destroy(col.gameObject);
        }
        else
        {
            PlayerHit hit = col.transform.root.GetComponentInChildren<PlayerHit>();
            if (hit != null)
            {
                hit.Hit(false);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }
        Destroy(gameObject);
    }
}
