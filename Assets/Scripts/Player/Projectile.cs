using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    private float currentSpeed;
    private GameObject shooter;
    private Transform target = null;
    private bool isFalling = false;

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
            currentSpeed = shooter.GetComponent<PlayerMovement>().GetCurrentSpeed() + 10;
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
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }
        
        PlayerHit hit = col.transform.root.GetComponentInChildren<PlayerHit>();
        if (hit != null)
        {
            hit.Hit();
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }
        Debug.Log(col.gameObject.name);
        Destroy(gameObject);
    }
}
