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
    private Camera cam;
    private void Start()
    {
        timer = fireRate;
        cam = GetComponent<PlayerCamera>().cam;
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
                Shoot();
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    /*void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.yellow, 7);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
        }

        Debug.DrawRay(barrelPosition.position, targetPoint, Color.red, 7);

        Vector3 direction = (targetPoint - barrelPosition.position).normalized;

        GameObject spawnedProjectile = Instantiate(projectilePrefab, barrelPosition.position, Quaternion.identity);
        spawnedProjectile.GetComponent<Projectile>().SetDirection(direction);
    }*/
    public void Shoot()
    {
        Vector3 targetPoint = GetAimPoint();

        Vector3 direction = (targetPoint - barrelPosition.position).normalized;

        GameObject bullet = Instantiate(
            projectilePrefab,
            barrelPosition.position,
            Quaternion.LookRotation(direction)
        );

        bullet.GetComponent<Projectile>().SetDirection(direction);
    }

    Vector3 GetAimPoint()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            crosshair.position
        );

        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
        {
            return hit.point;
        }
        return ray.origin + ray.direction * 1000;
    }
}
