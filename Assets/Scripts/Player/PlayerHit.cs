using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private float hitSpeedMultiplier;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private Animator anim;
    private float invincibilityTimer;
    private bool isInvincible;

    public void HitShield()
    {
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;
    }
    public void Hit(bool ignoreInvincibility)
    {
        if (isInvincible && !ignoreInvincibility) return;
        invincibilityTimer = 0;
        Rigidbody rb = transform.root.GetComponentInChildren<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x * hitSpeedMultiplier, rb.linearVelocity.y, rb.linearVelocity.z * hitSpeedMultiplier);
        transform.root.GetComponentInChildren<Vibrations>().TriggerVibration(0.2f, 0.2f, 0.3f);
        transform.root.GetComponentInChildren<PlayerPowerups>().DropGasTanks();
        transform.root.GetComponentInChildren<PlayerMovement>().enabled = false;
        isInvincible = true;
        anim.Play("Spin");
    }

    private void FixedUpdate()
    {
        if (!isInvincible) return;
        invincibilityTimer += Time.fixedDeltaTime;
        if (invincibilityTimer > invincibilityDuration - (invincibilityDuration - 1))
        {
            transform.root.GetComponentInChildren<PlayerMovement>().enabled = true;
            if (invincibilityTimer > invincibilityDuration)
            {
                isInvincible = false;
            }
        }
    }
}
