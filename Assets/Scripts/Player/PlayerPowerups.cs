using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Splines;
using System.Linq;
using Unity.Mathematics;

[RequireComponent(typeof(PlayerAudio))]
public class PlayerPowerups : MonoBehaviour
{
    [SerializeField] private GameObject gasTank;
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private int magnetPickupRange = 30;
    [SerializeField] private GameObject smokeScreen;
    [SerializeField] private float smokeDuration = 7f;
    [SerializeField] private GameObject landMine;
    [SerializeField] private GameObject airstrike;
    [SerializeField] private float airstrikeForwardOffset;

    [SerializeField] private TextMeshProUGUI currPowerUpText;
    [SerializeField] private TextMeshProUGUI gasTankCounter;
    private int gasTankAmount = 0;
    private PowerUpType? type = null;
    private bool usedPowerUp;
    private float normalTopSpeedModifier = 1;
    private bool usingTurbo = false;
    private bool usingMagnet = false;

    private RaceController raceController;

    // Audio
    private PlayerAudio playerAudio;

    private void Start()
    {
        // Get player audio
        playerAudio = GetComponent<PlayerAudio>();

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
        landMine,
        airstrike,
        deployWall,
        eMP
    };
    public void GainedPowerUp(PowerUpType type)
    {
        // Play audio
        playerAudio.PickupAudio(type);
        
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
                GameObject landmine = Instantiate(landMine, transform.position, Quaternion.identity);
                playerAudio.PlayLandminePlaceAudio(landmine); // Play audio
                break;

            case PowerUpType.airstrike:
                Airstrike();
                break;

            case PowerUpType.deployWall:
                break;

            case PowerUpType.eMP:
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
            gasTankCounter.text = "Gastanks: 0";
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
        else if (type == PowerUpType.airstrike)
        {
            currPowerUpText.text = "Airstrike";
        }
        else if (type == PowerUpType.deployWall)
        {
            currPowerUpText.text = "Deploy Wall";
        }
        else if (type == PowerUpType.eMP)
        {
            currPowerUpText.text = "EMP";
        }
        else
        {
            currPowerUpText.text = "";
        }
    }

    public void DropGasTanks()
    {
        if(gasTankAmount == 0) return;

        //Debug.Log("GastanksAmount: " + gasTankAmount);
        int gasTanksToDrop = Mathf.CeilToInt(gasTankAmount / 2f);

        if (gasTankAmount <= 2)
        {
            gasTanksToDrop = gasTankAmount;
        }
        gasTankAmount -= gasTanksToDrop;
        gasTankCounter.text = "Gastanks: " + gasTankAmount;
        //Debug.Log("GastanksToDrop: " + gasTanksToDrop);
        for (int i = 0; i < gasTanksToDrop; i++) //Spawnar så många gastanks som behövs, get dem en rand pos och sätter ui och topspeed värdena till halverade värden
        {
            float positionOffset = 10f;
            Vector3 rndPos = new Vector3(UnityEngine.Random.Range(transform.position.x - positionOffset, transform.position.x + positionOffset), transform.position.y + 1, UnityEngine.Random.Range(transform.position.z - positionOffset, transform.position.z + positionOffset));
            GameObject tanks = Instantiate(gasTank, rndPos, Quaternion.identity);

            StartCoroutine(tanks.GetComponent<Pickup>().DroppedTanks());
        }

        //Debug.Log("ExternalTopSpeed before changes: " + GetComponent<PlayerMovement>().externalTopSpeedModifier);

        GetComponent<PlayerMovement>().externalTopSpeedModifier = 1f + (0.1f * gasTankAmount); //Halverar topspeed
        normalTopSpeedModifier = 1f + +(0.1f * gasTankAmount);

        //Debug.Log("ExternalTopSpeed after changes: " + GetComponent<PlayerMovement>().externalTopSpeedModifier);
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

    void Airstrike()
    {
        raceController = FindFirstObjectByType<RaceController>();
        if (raceController == null || raceController.trackSpline == null) return;

        RacerData leader = raceController.racers.OrderByDescending(x => x.raceProgress).FirstOrDefault();

        if (leader != null)
        {
            float currentProgress = raceController.GetSplineProgress(leader.transform.position);

            float3 localPos = raceController.trackSpline.EvaluatePosition(currentProgress);
            Vector3 worldPos = raceController.trackSpline.transform.TransformPoint(localPos);

            float3 localTangent = raceController.trackSpline.EvaluateTangent(currentProgress);
            Vector3 worldDirection = raceController.trackSpline.transform.TransformDirection(localTangent);
            worldDirection.y = 0;
            worldDirection.Normalize();

            float distanceAhead = 100f;
            Vector3 spawnWorldPos = leader.transform.position + (worldDirection * distanceAhead);
            GameObject strike = Instantiate(airstrike, spawnWorldPos, Quaternion.LookRotation(worldDirection));
        }
    }
}
