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
    private AutoAimCone autoAim;
    public float timeLockedOnTarget;
    public float speedMultiplier;
    private void Start()
    {
        timer = fireRate;
        autoAim = GetComponentInChildren<AutoAimCone>();

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
        Vector3 targetRay = GetTargetDir();
        Vector3 actualWorldTarget = playerCam.cam.transform.position + (targetRay * 100f);

        Vector3 bulletDir = (actualWorldTarget - barrelPosition.position).normalized;

        if (autoAim.GetTarget() != null)
        {
            bulletDir = (autoAim.GetTarget().position - barrelPosition.position).normalized;
        }

        bulletDir.y = 0;
        GameObject bullet = Instantiate(
            prefab,
            barrelPosition.position,
            Quaternion.LookRotation(bulletDir)
        );

        bullet.GetComponent<Projectile>().PrepareProjectile(gameObject, null, speedMultiplier);

        // Play shoot audio
        playerAudio.ShootStart();
    }

    private Vector3 GetTargetDir()
    {
        return playerCam.GetStableCrosshairRay().direction;
    }
}
