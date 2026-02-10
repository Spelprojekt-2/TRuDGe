using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelPosition;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Canvas canvas;
    private float timer = 0;
    private bool isShooting = false;
    [SerializeField] private LayerMask excludeLayers;
    [SerializeField] private PlayerCamera playerCam;
    private void Start()
    {
        timer = fireRate;
    }
    public void ShootInput(InputAction.CallbackContext context)
    {
        isShooting = context.performed;
    }

    private void Update()
    {
        if(timer >= fireRate)
        {
            if(isShooting)
            {
                timer = 0;
                Shoot(projectilePrefab);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public void Shoot(GameObject prefab)
    {
        Vector3 targetPoint = GetTargetPoint();

        Vector3 bulletDir = (targetPoint - barrelPosition.position).normalized;
        targetPoint.y = barrelPosition.position.y;
        if(bulletDir.y < 0)
        {
            bulletDir.y = 0;
        }
        GameObject bullet = Instantiate(
            prefab,
            barrelPosition.position,
            Quaternion.LookRotation(bulletDir)
        );

        bullet.GetComponent<Projectile>().PrepareProjectile(gameObject, null);
    }

    public void ShootHomingMissile(GameObject prefab)
    {
        Vector3 targetPoint = GetTargetPoint();

        Vector3 bulletDir = (targetPoint - barrelPosition.position).normalized;
        targetPoint.y = barrelPosition.position.y;
        GameObject bullet = Instantiate(
            prefab,
            barrelPosition.position,
            Quaternion.LookRotation(bulletDir)
        );

        
        bullet.GetComponent<Projectile>().PrepareProjectile(gameObject, GetClosestPlayer());
    }

    private Vector3 GetTargetPoint()
    {
        // Call the new stable ray function from your Camera script
        Ray ray = playerCam.GetStableCrosshairRay();

        RaycastHit hit;

        // Use the LayerMask we set up earlier to ignore the player/vehicle
        if (Physics.Raycast(ray, out hit, 1000f, ~excludeLayers))
        {
            return hit.point;
        }

        return ray.GetPoint(1000f);
    }

    public Transform GetClosestPlayer()
    {
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (PlayerMovement p in players)
        {
            if (p.transform == this.transform) continue;

            float distance = Vector3.Distance(currentPosition, p.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = p.transform;
            }
        }

        return closestTarget;
    }
}
