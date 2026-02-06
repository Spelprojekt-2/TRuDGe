using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerPowerups : MonoBehaviour
{
    [SerializeField] private GameObject homingMissile;
    private PowerUpType? type = null;
    private bool usedPowerUp;
    private float normalTopSpeedModifier;
    private bool usingTurbo = false;
    public void UsePowerUpInput(InputAction.CallbackContext context)
    {
        usedPowerUp = context.performed;
    }
    public enum PowerUpType
    {
        gasolineTank,
        homingMissle,
        turbo
    };
    public void GainedPowerUp(PowerUpType type)
    {
        this.type = type;
        if(type == PowerUpType.gasolineTank)
        {
            if (usingTurbo)
            {
                normalTopSpeedModifier += 0.1f;
            }
            else
            {
                GetComponent<PlayerMovement>().externalTopSpeedModifier += 0.1f;
            }
            this.type = null;
        }
    }

    private void UsePowerUp()
    {
        usedPowerUp = false;

        switch (type)
        {
            case PowerUpType.homingMissle:
                GetComponent<PlayerShooting>().Shoot(homingMissile);
                break;

            case PowerUpType.turbo:
                StartCoroutine(Turbo());
                break;

            default:
                break;
        }
        Debug.Log("Used " + type);
        type = null;
    }

    private void Update()
    {
        if (usedPowerUp)
        {
            UsePowerUp();
        }
    }

    IEnumerator Turbo()
    {
        var playerMovement = GetComponent<PlayerMovement>();

        var normalAccelerationModifier = playerMovement.externalAccelerationModifier;
        normalTopSpeedModifier = playerMovement.externalTopSpeedModifier;

        playerMovement.externalAccelerationModifier = 2f;
        playerMovement.externalTopSpeedModifier = 2f;
        playerMovement.externalIgnoreInAirAccelerationModifier = true;
        yield return new WaitForSeconds(2f);

        playerMovement.externalAccelerationModifier = normalAccelerationModifier;
        playerMovement.externalTopSpeedModifier = normalTopSpeedModifier;
        playerMovement.externalIgnoreInAirAccelerationModifier = false;
    }
}
