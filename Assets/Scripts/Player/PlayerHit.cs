using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] private float hitStrength;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private Animator anim;
    private float invincibilityTimer;
    private bool isInvincible;
    public void Hit(bool ignoreInvincibility)
    {
        if (isInvincible && !ignoreInvincibility) return;
        invincibilityTimer = 0;
        transform.root.GetComponentInChildren<Rigidbody>().linearVelocity = Vector3.up * hitStrength;
        transform.root.GetComponentInChildren<Vibrations>().TriggerVibration(0.2f, 0.2f, 0.3f);
        transform.root.GetComponentInChildren<PlayerPowerups>().DropGasTanks();
        isInvincible = true;
        anim.Play("Spin");
    }

    private void FixedUpdate()
    {
        if (!isInvincible) return;
        invincibilityTimer += Time.fixedDeltaTime;
        if (invincibilityTimer > invincibilityDuration)
        {
            isInvincible = false;
        }
    }
}
