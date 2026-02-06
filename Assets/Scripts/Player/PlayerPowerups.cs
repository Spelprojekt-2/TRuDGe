using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerPowerups : MonoBehaviour
{
    [SerializeField] private GameObject homingMissile;
    private PowerUpType type;
    private bool usedPowerUp;

    public void UsePowerUpInput(InputAction.CallbackContext context)
    {
        usedPowerUp = context.performed;
    }
    public enum PowerUpType
    {
        nothing,
        gasolineTank,
        homingMissle,
        turbo
    };
    public void GainedPowerUp(PowerUpType type)
    {
        this.type = type;
    }

    private void UsePowerUp()
    {
        usedPowerUp = false;

        switch (type)
        {
            case PowerUpType.nothing:
                Debug.Log("Used " + type);
                return;

            case PowerUpType.gasolineTank:
                GetComponent<PlayerMovement>().externalTopSpeedModifier += 0.1f;
                break;

            case PowerUpType.homingMissle:
                GetComponent<PlayerShooting>().Shoot(homingMissile);
                break;

            default:
                break;
        }

        type = PowerUpType.nothing;
    }

    private void Update()
    {
        if (usedPowerUp)
        {
            UsePowerUp();
        }
    }
}
