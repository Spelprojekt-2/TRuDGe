using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Splines;
using System.Linq;
using System.Net.Mime;
using Unity.Mathematics;
using UnityEngine.Events;

public class PlayerPowerups : MonoBehaviour
{
    [Header("---Power Up Events---")]
    [SerializeField] private UnityEvent onTurbo;
    [SerializeField] private UnityEvent onMagnet;
    [SerializeField] private UnityEvent onSmoke;
    [SerializeField] private UnityEvent onLandmine;
    [SerializeField] private UnityEvent onAirStrike;
    [SerializeField] private UnityEvent onDeployWall;
    [SerializeField] private UnityEvent onScatterShot;
    [SerializeField] private UnityEvent onShield;

    [Header("---Power Up Settings---")]
    [SerializeField] private GameObject gasTank;
    [SerializeField] private int magnetPickupRange = 30;
    [SerializeField] private GameObject smokeScreen;
    [SerializeField] private float smokeDuration = 7f;
    [SerializeField] private GameObject landMine;
    [SerializeField] private GameObject airstrike;
    [SerializeField] private float airstrikeForwardOffset;
    [SerializeField] private GameObject deployedWall;
    [SerializeField] private GameObject scatterShot;
    [SerializeField] private Transform barrelPosition;
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldTimer = 4f;

    [SerializeField] private TextMeshProUGUI currPowerUpText;
    [SerializeField] private Image useKeyController;
    [SerializeField] private Image useKeyKBM;
   // [SerializeField] private TextMeshProUGUI gasTankCounter;
    public int gasTankAmount = 0;
    private PowerUpType? type = null;
    private bool usedPowerUp;
    private float normalTopSpeedModifier = 1;
    private bool usingTurbo = false;
    private bool usingMagnet = false;
    private GameObject shieldSpawned;

    private RaceController raceController;
    
    // Audio
    private PlayerAudio playerAudio;

    private void Start()
    {
        // Get player audio
        PlayerAudio plrAudio = GetComponent<PlayerAudio>();
        if (plrAudio != null)
            playerAudio = plrAudio;

        currPowerUpText.text = "";
        gasTankAmount = 0;
        
        //gasTankCounter.text = "Gastanks: 0";
    }
    public void UsePowerUpInput(InputAction.CallbackContext context)
    {
        usedPowerUp = context.performed;
    }
    public enum PowerUpType
    {
        gasolineTank,
        turbo,
        magnet,
        smoke,
        landMine,
        airstrike,
        deployWall,
        scatterShot,
        shield
    };
    public void GainedPowerUp(PowerUpType type)
    {
        //Change use key
        if (type != PowerUpType.gasolineTank)
        {
            bool isController = GetComponent<PlayerInput>().currentControlScheme == "Gamepad";
            if (isController)
            {
                useKeyController.gameObject.SetActive(true);
                useKeyKBM.gameObject.SetActive(false);
            }
            else
            {
                useKeyController.gameObject.SetActive(false);
                useKeyKBM.gameObject.SetActive(true);
            }
        }
        
        // Play audio
        if (playerAudio != null)
            playerAudio.PickupAudio(type);

        if (type == PowerUpType.gasolineTank)
        {
            if (gasTankAmount < 10)
            {
                gasTankAmount++;

                //gasTankCounter.text = "Gastanks: " + gasTankAmount;
                if (usingTurbo)
                {
                    normalTopSpeedModifier += 0.05f;
                }
                else
                {
                    GetComponent<PlayerMovement>().externalTopSpeedModifier += 0.05f;
                    GetComponent<PlayerMovement>().AccelerationGasModifier += 0.015f;
                }
            }
        }
        else
        {
            if(this.type != null)
            {
                return;
            }
            else
            {
                this.type = type;
                if(this.type == PowerUpType.airstrike)
                {
                    AirstrikeGlobalCooldown.canUseAirstrike = false;
                    //Debug.Log("Cannot use airstrike");
                }
                PowerUpUIUpdate();
            }
        }
    }

    private void UsePowerUp()
    {
        usedPowerUp = false;

        switch (type)
        {
            case PowerUpType.turbo:
                onTurbo.Invoke();
                if (usingTurbo) return;
                StartCoroutine(Turbo());
                break;

            case PowerUpType.magnet:
                onMagnet.Invoke();
                if (playerAudio != null)
                    playerAudio.ToggleMagnetAudio(true, gameObject); // Start magnet audio
                StartCoroutine(Magnet());
                break;

            case PowerUpType.smoke:
                onSmoke.Invoke();
                Smokescreen();
                break;

            case PowerUpType.landMine:
                onLandmine.Invoke();
                GameObject landmine = Instantiate(landMine, transform.position, Quaternion.identity);
                break;

            case PowerUpType.airstrike:
                onAirStrike.Invoke();
                Airstrike();
                break;

            case PowerUpType.deployWall:
                onDeployWall.Invoke();
                Instantiate(deployedWall, new Vector3(transform.position.x, transform.position.y + -7, transform.position.z) - transform.forward * 10, Quaternion.LookRotation(transform.forward));
                break;

            case PowerUpType.scatterShot:
                onScatterShot.Invoke();
                GameObject scatterShotSpawned = Instantiate(scatterShot, barrelPosition.position, barrelPosition.rotation);
                foreach (var projectile in scatterShotSpawned.GetComponentsInChildren<Projectile>())
                {
                    projectile.PrepareProjectile(gameObject, null, 1);
                }
                break;

            case PowerUpType.shield:
                if (shieldSpawned != null) return;
                onShield.Invoke();
                shieldSpawned = Instantiate(shield, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
                shieldSpawned.transform.parent = gameObject.transform;
                if (playerAudio != null)
                    playerAudio.PlayShieldAudio(shieldTimer); // Play shield audio
                StartCoroutine(Shield(shieldSpawned));
                break;

            default:
                return;
        }
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
            foreach (var pickup in Pickup.AllPickups)
            {
                if (pickup.powerUpType == PowerUpType.gasolineTank)
                {
                    if (Vector3.Distance(transform.position, pickup.transform.position) <= magnetPickupRange)
                    {
                        pickup.SetMagnetTarget(transform);
                    }
                }
            }
        }
    }

    void PowerUpUIUpdate()
    {
        if(type == PowerUpType.turbo)
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
        else if (type == PowerUpType.scatterShot)
        {
            currPowerUpText.text = "Scatter Shot";
        }
        else if (type == PowerUpType.shield)
        {
            currPowerUpText.text = "Shield";
        }
        else
        {
            currPowerUpText.text = "";
            useKeyController.gameObject.SetActive(false);
            useKeyKBM.gameObject.SetActive(false);
        }
    }

    public void ResetGasTanks()
    {
        gasTankAmount = 0;
        type = null;
        usedPowerUp = false;
        PowerUpUIUpdate();
        GetComponent<PlayerMovement>().externalTopSpeedModifier = 1f;
        normalTopSpeedModifier = 1;
        GetComponent<PlayerMovement>().AccelerationGasModifier = 1f;
        
        //gasTankCounter.text = "Gastanks: 0";
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
        
        //gasTankCounter.text = "Gastanks: " + gasTankAmount;
        //Debug.Log("GastanksToDrop: " + gasTanksToDrop);
        for (int i = 0; i < gasTanksToDrop; i++) //Spawnar s� m�nga gastanks som beh�vs, get dem en rand pos och s�tter ui och topspeed v�rdena till halverade v�rden
        {
            float positionOffset = 10f;
            Vector3 rndPos = new Vector3(UnityEngine.Random.Range(transform.position.x - positionOffset, transform.position.x + positionOffset), transform.position.y + 1, UnityEngine.Random.Range(transform.position.z - positionOffset, transform.position.z + positionOffset));
            GameObject tanks = Instantiate(gasTank, rndPos, Quaternion.identity);
            tanks.GetComponent<Pickup>().canRespawn = false;
        }

        //Debug.Log("ExternalTopSpeed before changes: " + GetComponent<PlayerMovement>().externalTopSpeedModifier);

        GetComponent<PlayerMovement>().externalTopSpeedModifier = 1f + (0.05f * gasTankAmount); //Halverar topspeed
        normalTopSpeedModifier = 1f + +(0.05f * gasTankAmount);
        GetComponent<PlayerMovement>().AccelerationGasModifier = 1f + (0.015f * gasTankAmount); //Halverar topspeed
        

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
        yield return new WaitForSeconds(5f);

        playerMovement.externalAccelerationModifier = normalAccelerationModifier;
        playerMovement.externalTopSpeedModifier = normalTopSpeedModifier;
        playerMovement.externalIgnoreInAirAccelerationModifier = false;
        usingTurbo = false;

        // Stop turbo audio
        if (playerAudio != null)
            playerAudio.StopTurboAudio();
    }

    IEnumerator Magnet()
    {
        usingMagnet = true;
        yield return new WaitForSeconds(5f);
        usingMagnet = false;
        if (playerAudio != null)
            playerAudio.ToggleMagnetAudio(false, gameObject); // Stop magnet audio
    }

    void Smokescreen()
    {
        GameObject spawnedSmoke = Instantiate(smokeScreen, transform.position, Quaternion.identity);
        Destroy(spawnedSmoke, smokeDuration);
    }

    IEnumerator Shield(GameObject shieldSpawned)
    {
        yield return new WaitForSeconds(shieldTimer);
        Destroy(shieldSpawned);
        //playerAudio.ShieldBreakAudio(false);
    }

    void Airstrike()
    {
        raceController = FindFirstObjectByType<RaceController>();
        if (raceController == null || raceController.trackSpline == null) return;

        RacerData leader = raceController.racers.OrderByDescending(x => x.raceProgress).FirstOrDefault();

        if (leader != null)
        {
            GameObject strike = Instantiate(airstrike, Vector3.down * 1000f, Quaternion.identity);
            strike.GetComponent<Airstrike>().SetTarget(leader);
        }
    }
}
