using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] LayerMask groundLayer;
    private float currentSpeed;
    private GameObject shooter;
    private Transform target = null;
    private bool isFalling = false;
    private bool homing = false;

    public void PrepareProjectile(GameObject shooter, Transform target)
    {
        if (target == null)
        {
            currentSpeed = projectileSpeed;
            StartCoroutine(DeathTimer());
        }
        else
        {
            StartCoroutine(HomingMissile());
            currentSpeed = shooter.GetComponent<PlayerMovement>().GetCurrentSpeed() + 10;
        }
        
        this.shooter = shooter;
        this.target = target;
        homing = false;
    }

    private void FixedUpdate()
    {
        if (isFalling) return;

        rb.linearVelocity = transform.forward * currentSpeed;

        if (target != null && homing)
        {
            Vector3 targetPos = target.position;
            targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
            transform.LookAt(targetPos);

            /*Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 15f, groundLayer))
            {
                transform.position += Vector3.up * 3f;
            }*/
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = true;
        rb.useGravity = true;
    }
    
    IEnumerator HomingMissile()
    {
        yield return new WaitForSeconds(0.5f);
        homing = true;
        currentSpeed = projectileSpeed;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.IsChildOf(shooter.transform))
        {
            return;
        }
        if (col.transform.root.CompareTag("Player"))
        {
            Vector3 force = (transform.position - col.transform.position).normalized * 30f;
            force.y = 0;
            col.gameObject.GetComponentInParent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            col.gameObject.GetComponentInParent<Vibrations>().TriggerVibration(0.2f, 0.2f, 0.3f);
            col.gameObject.GetComponentInParent<PlayerPowerups>().DropGasTanks();
        }
        Destroy(gameObject);
    }
}
