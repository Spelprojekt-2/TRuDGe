using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
public class PlayerPowerups : MonoBehaviour
{
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private int magnetPickupRange = 30;
    [SerializeField] private TextMeshProUGUI currPowerUpText;
    private PowerUpType? type = null;
    private bool usedPowerUp;
    private float normalTopSpeedModifier;
    private bool usingTurbo = false;
    private bool usingMagnet = false;

    private void Start()
    {
        currPowerUpText.text = "";
    }
    public void UsePowerUpInput(InputAction.CallbackContext context)
    {
        usedPowerUp = context.performed;
    }
    public enum PowerUpType
    {
        gasolineTank,
        homingMissle,
        turbo,
        magnet
    };
    public void GainedPowerUp(PowerUpType type)
    {
        this.type = type;
        PowerUpUIUpdate();
        if (type == PowerUpType.gasolineTank)
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
                if (usingTurbo) return;
                StartCoroutine(Turbo());
                break;

            case PowerUpType.magnet:
                StartCoroutine(Magnet());
                break;

            default:
                return;
        }
        Debug.Log("Used " + type);
        type = null;
        PowerUpUIUpdate();
    }

    private void Update()
    {
        if (usedPowerUp)
        {
            UsePowerUp();
        }

        if (usingMagnet)
        {
            Pickup[] gasolineTanks = FindObjectsOfType<Pickup>();

            foreach (var gasTank in gasolineTanks)
            {
                if(gasTank.powerUpType == PowerUpType.gasolineTank)
                {
                    if (Vector2.Distance(transform.position, gasTank.transform.position) <= magnetPickupRange)
                    {
                        var t = 0.1f;
                        gasTank.transform.position = Vector3.Lerp(gasTank.transform.position, transform.position, t);
                    }
                }
            }
        }
    }

    void PowerUpUIUpdate()
    {
        if(type == PowerUpType.homingMissle)
        {
            currPowerUpText.text = "Homing Missile";
        }
        else if(type == PowerUpType.turbo)
        {
            currPowerUpText.text = "Turbo";
        }
        else if (type == PowerUpType.magnet)
        {
            currPowerUpText.text = "Magnet";
        }
        else
        {
            currPowerUpText.text = "";
        }
    }

    IEnumerator Turbo()
    {
        usingTurbo = true;
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
        usingTurbo = false;
    }

    IEnumerator Magnet()
    {
        usingMagnet = true;
        yield return new WaitForSeconds(10f);
        usingMagnet = false;
    }
}
