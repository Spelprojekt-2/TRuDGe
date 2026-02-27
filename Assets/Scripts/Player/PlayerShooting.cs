using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAudio))]
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

    // Audio refs
    PlayerAudio playerAudio;

    private void Start()
    {
        timer = fireRate;

        // Get PlayerAudio.
        playerAudio = GetComponent<PlayerAudio>();
    }
    public void ShootInput(InputAction.CallbackContext context)
    {
        isShooting = context.performed;
    }

    private void Update()
    {
        if (timer >= fireRate)
        {
            if (isShooting)
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
        bulletDir.y = 0;
        GameObject bullet = Instantiate(
            prefab,
            barrelPosition.position,
            Quaternion.LookRotation(bulletDir)
        );

        bullet.GetComponent<Projectile>().PrepareProjectile(gameObject, null);

        // Play shoot audio
        playerAudio.ShootStart();
    }

    private Vector3 GetTargetPoint()
    {
        return playerCam.GetStableCrosshairRay().direction;
    }
}
