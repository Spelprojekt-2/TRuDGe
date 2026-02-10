using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerPowerups : MonoBehaviour
{
    [SerializeField] private GameObject gasTank;
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private int magnetPickupRange = 30;
    [SerializeField] private GameObject smokeScreen;
    [SerializeField] private float smokeDuration = 7f;
    [SerializeField] private GameObject landMine;

    [SerializeField] private TextMeshProUGUI currPowerUpText;
    [SerializeField] private TextMeshProUGUI gasTankCounter;
    private int gasTankAmount = 0;
    private PowerUpType? type = null;
    private bool usedPowerUp;
    private float normalTopSpeedModifier;
    private bool usingTurbo = false;
    private bool usingMagnet = false;

    private void Start()
    {
        currPowerUpText.text = "";
        gasTankAmount = 0;
        gasTankCounter.text = "Gastanks: 0";
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
        magnet,
        smoke,
        landMine
    };
    public void GainedPowerUp(PowerUpType type)
    {
        if (type == PowerUpType.gasolineTank)
        {
            if (gasTankAmount < 10)
            {
                gasTankAmount++;
                gasTankCounter.text = "Gastanks: " + gasTankAmount;
                if (usingTurbo)
                {
                    normalTopSpeedModifier += 0.1f;
                }
                else
                {
                    GetComponent<PlayerMovement>().externalTopSpeedModifier += 0.1f;
                }
            }
        }
        else
        {
            this.type = type;
            PowerUpUIUpdate();
        }
    }

    private void UsePowerUp()
    {
        usedPowerUp = false;

        switch (type)
        {
            case PowerUpType.homingMissle:
                GetComponent<PlayerShooting>().ShootHomingMissile(homingMissile);
                break;

            case PowerUpType.turbo:
                if (usingTurbo) return;
                StartCoroutine(Turbo());
                break;

            case PowerUpType.magnet:
                StartCoroutine(Magnet());
                break;

            case PowerUpType.smoke:
                Smokescreen();
                break;

            case PowerUpType.landMine:
                Instantiate(landMine, transform.position, Quaternion.identity);
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
        if (gasTankAmount > 0 && SceneManager.GetActiveScene().name == "SelectionScreen")
        {
            gasTankAmount = 0;
        }

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
                    if (Vector3.Distance(transform.position, gasTank.transform.position) <= magnetPickupRange)
                    {
                        gasTank.SetMagnetTarget(transform);
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
        else if(type == PowerUpType.smoke)
        {
            currPowerUpText.text = "Smoke Screen";
        }
        else if(type == PowerUpType.landMine)
        {
            currPowerUpText.text = "Landmine";
        }
        else
        {
            currPowerUpText.text = "";
        }
    }

    public void DropGasTanks()
    {
        if(gasTankAmount == 0) return;
        for (int i = 0; i < gasTankAmount; i++)
        {
            float positionOffset = 10f;
            Vector3 rndPos = new Vector3(Random.Range(transform.position.x - positionOffset, transform.position.x + positionOffset), transform.position.y + 1, Random.Range(transform.position.z - positionOffset, transform.position.z + positionOffset));
            GameObject tanks = Instantiate(gasTank, rndPos, Quaternion.identity);
            StartCoroutine(tanks.GetComponent<Pickup>().DroppedTanks());
        }
        gasTankAmount = 0;
        gasTankCounter.text = "Gastanks: 0";
    }

    IEnumerator Turbo()
    {
        usingTurbo = true;
        var playerMovement = GetComponent<PlayerMovement>();

        var normalAccelerationModifier = playerMovement.externalAccelerationModifier;
        normalTopSpeedModifier = playerMovement.externalTopSpeedModifier;

        playerMovement.externalAccelerationModifier = 1.75f;
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
        yield return new WaitForSeconds(5f);
        usingMagnet = false;
    }

    void Smokescreen()
    {
        GameObject spawnedSmoke = Instantiate(smokeScreen, transform.position, Quaternion.identity);
        Destroy(spawnedSmoke, smokeDuration);
    }

    
}
