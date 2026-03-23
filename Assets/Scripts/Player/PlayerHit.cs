using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerAudio))]
public class PlayerHit : MonoBehaviour
{
    [SerializeField] private float hitSpeedMultiplier;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private Animator anim;
    [SerializeField] private NinaMechanic ninaWrenches;
    [SerializeField] private UnityEvent hit;
    private PlayerAudio playerAudio;

    private float invincibilityTimer;
    private bool isInvincible;

    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    public void HitShield()
    {
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;

        playerAudio.ShieldBreakAudio(); // Play shield break audio
    }
    public void Hit(bool ignoreInvincibility)
    {
        if (isInvincible && !ignoreInvincibility) return;
        invincibilityTimer = 0;

        hit.Invoke();

        Rigidbody rb = transform.root.GetComponentInChildren<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x * hitSpeedMultiplier, rb.linearVelocity.y, rb.linearVelocity.z * hitSpeedMultiplier);
        transform.root.GetComponentInChildren<Vibrations>().TriggerVibration(0.2f, 0.2f, 0.3f);
        transform.root.GetComponentInChildren<PlayerPowerups>().DropGasTanks();
        transform.root.GetComponentInChildren<PlayerMovement>().canTurn = false;
        transform.root.GetComponentInChildren<PlayerShooting>().isShot = true;
        isInvincible = true;
        anim.SetTrigger("Hit");
        ninaWrenches.StartCoroutine(ninaWrenches.LaunchWrenches());
    }

    private void FixedUpdate()
    {
        if (!isInvincible) return;
        invincibilityTimer += Time.fixedDeltaTime;
        if (invincibilityTimer > invincibilityDuration - (invincibilityDuration - 1))
        {
            transform.root.GetComponentInChildren<PlayerMovement>().canTurn = true;
            transform.root.GetComponentInChildren<PlayerShooting>().isShot = false;
            if (invincibilityTimer > invincibilityDuration)
            {
                isInvincible = false;
            }
        }
    }
}
