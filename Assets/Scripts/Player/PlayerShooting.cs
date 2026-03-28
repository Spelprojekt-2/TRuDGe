using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PlayerShooting : MonoBehaviour
{

    [SerializeField] public Image shootCooldown;
    
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelPosition;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Canvas canvas;
    public float timer = 0;
    private bool isShooting = false;
    [SerializeField] private LayerMask excludeLayers;
    [SerializeField] private PlayerCamera playerCam;
    // Audio refs
    PlayerAudio playerAudio;
    private AutoAimCone autoAim;
    [HideInInspector] public float timeLockedOnTarget;
    [HideInInspector] public float speedMultiplier;
    [HideInInspector] public bool isShot = false; 
    private void Start()
    {
        timer = fireRate;
        shootCooldown.fillAmount = 0f;
        autoAim = GetComponentInChildren<AutoAimCone>();

        // Get player audio
        PlayerAudio plrAudio = GetComponent<PlayerAudio>();
        if (plrAudio != null)
            playerAudio = plrAudio;
    }
    public void ShootInput(InputAction.CallbackContext context)
    {
        isShooting = context.performed;
    }

    private void Update()
    {
        if (timer >= fireRate)
        {
            if (isShooting && !isShot && !GetComponent<PlayerCamera>().isPressingLookBack)
            {
                timer = 0;
                Shoot(projectilePrefab);
                shootCooldown.fillAmount = 0;
                isShooting = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            shootCooldown.fillAmount += Time.deltaTime / fireRate;
        }
    }
    public void Shoot(GameObject prefab)
    {
        Vector3 targetRay = GetTargetDir();
        Vector3 actualWorldTarget = playerCam.cam.transform.position + (targetRay * 100f);

        Vector3 bulletDir = (actualWorldTarget - barrelPosition.position).normalized;

        if (autoAim.GetTarget() != null && !playerCam.IsTargetBlockedBySmoke(autoAim.GetTarget()))
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
        if (playerAudio != null)
            playerAudio.ShootStart();

        //Napoleon
        NapoleonRespect nr = transform.root.GetComponentInChildren<NapoleonRespect>();
        nr.StartCoroutine(nr.LowerRespect());
    }

    private Vector3 GetTargetDir()
    {
        return playerCam.GetStableCrosshairRay().direction;
    }
}
