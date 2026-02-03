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
    public void Shoot()
    {
        Vector3 targetPoint = GetAimPoint();
        Vector3 direction = (targetPoint - barrelPosition.position).normalized;
        GameObject bullet = Instantiate(
            projectilePrefab,
            barrelPosition.position,
            Quaternion.LookRotation(direction)
        );

        bullet.GetComponent<Projectile>().SetDirection(direction, gameObject);
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
        return ray.origin + ray.direction * 100000;
    }
}
