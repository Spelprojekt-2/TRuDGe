using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(PlayerAudio))]
public class PlayerShooting : MonoBehaviour
{

    [SerializeField] private Image shootCooldown;
    
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
    [HideInInspector] public float timeLockedOnTarget;
    [HideInInspector] public float speedMultiplier;
    private void Start()
    {
        timer = fireRate;
        shootCooldown.fillAmount = 1;
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
                shootCooldown.fillAmount = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;
            shootCooldown.fillAmount += 0.04f;
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
        else
        {
            bulletDir.y = 0;
        }
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
