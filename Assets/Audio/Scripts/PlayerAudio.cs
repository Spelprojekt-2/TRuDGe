using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private TanksAudio tanksAudio;
    [SerializeField] private InteractablesAudio interactablesAudio;

    // EventInstances
    private EventInstance grappleInstance;
    private EventInstance shootInstance;
    private EventInstance engineInstance;
    private EventInstance magnetInstance;
    private EventInstance turboInst;

    // GameOBJs
    [SerializeField] private GameObject grapplePos;
    [SerializeField] private GameObject canonPos;

    [SerializeField] private PlayerMovement playerMovement;

    // Checks
    private bool hasGrapple;
    private bool hasShoot;

    // Shield vars
    private bool hasShield;
    private float shieldStopTime;
    private float shieldStartTime;
    private EventInstance shieldInst;
    private Coroutine shieldRoutine;

    private void Awake()
    {
        if (tanksAudio == null)
        {
            Debug.LogWarning("PlayerAudio: TanksAudio is missing!");
        }
    }

    private void Start()
    {
        engineInstance = tanksAudio.VehicleEngineStartAudio(new EventInstance(), gameObject);
    }

    private void LateUpdate()
    {
        engineInstance.setParameterByName("RPM", playerMovement.GetNormalizedSpeed());
    }

    private void OnDisable()
    {
        if (grappleInstance.isValid())
            GrappleEnd();
        if (shootInstance.isValid())
            ShootEnd();
    }

    #region ShootFunctions
    public void ShootStart()
    {
        if (hasShoot && shootInstance.isValid())
        {
            shootInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        hasShoot = true;
        shootInstance = tanksAudio.ShootStartAudio(new EventInstance(), canonPos);
    }

    public void ShootHit(GameObject projectileOBJ)
    {
        if (hasShoot)
            shootInstance = tanksAudio.ShootHitAudio(shootInstance);
    }

    public void ShootEnd()
    {
        if (hasShoot)
        {
            shootInstance = tanksAudio.ShootEndAudio(shootInstance);
            hasShoot= false;
        }
    }
    #endregion

    #region GrappleFunctions
    public void GrappleStart()
    {
        if (hasGrapple && grappleInstance.isValid())
        {
            grappleInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        hasGrapple = true;
        grappleInstance = tanksAudio.GrappleStartAudio(new EventInstance(), grapplePos);
    }

    /*
    public void GrappleHit()
    {
        if (hasGrapple)
            tanksAudio.SetGrappleState(grappleInstance, TanksAudio.GrappleState.Hit);
    }

    public void GrappleReturn()
    {
        if (hasGrapple)
            tanksAudio.SetGrappleState(grappleInstance, TanksAudio.GrappleState.Return);
    }
    */

    public void GrappleEnd()
    {
        if (hasGrapple)
        {
            tanksAudio.SetGrappleState(grappleInstance, TanksAudio.GrappleState.End);
            hasGrapple = false;
        }
    }
    #endregion

    #region PowerupFunctions
    public void PickupAudio(PlayerPowerups.PowerUpType type)
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            return;
        }
        interactablesAudio.PlayPickupAudio(type);
    }

    public void ToggleMagnetAudio(bool mode, GameObject obj)
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            return;
        }

        if (mode)
        {
            // Start magnet audio
            magnetInstance = interactablesAudio.StartMagnetAudio(magnetInstance, obj);
        }
        else
        {
            // Stop magnet audio
            magnetInstance = interactablesAudio.StopMagnetAudio(magnetInstance);
        }
    }

    public void PlayShieldAudio(float shieldTimer)
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            hasShield = false;
            return;
        }

        float newStopTime = Time.time + shieldTimer;

        if (hasShield)
        {
            shieldStopTime = Mathf.Max(shieldStopTime, newStopTime);
            shieldStartTime = Time.time;
        }
        else
        {
            hasShield = true;

            shieldStartTime = Time.time;
            shieldStopTime = newStopTime;

            shieldInst = interactablesAudio.ShieldDurationAudio(shieldInst);

            if (shieldRoutine != null)
                StopCoroutine(shieldRoutine);

            shieldRoutine = StartCoroutine(ShieldHandler());
        }
    }

    public void ShieldBreakAudio()
    {
        shieldStopTime = 0f;
    }

    private IEnumerator ShieldHandler()
    {
        while (Time.time < shieldStopTime)
        {
            float remaining = shieldStopTime - Time.time;
            float totalDuration = shieldStopTime - shieldStartTime;

            float normalized = Mathf.Clamp01(remaining / totalDuration);
            //Debug.Log("NORMAL:" + normalized);

            if (shieldInst.isValid())
            {
                interactablesAudio.SetPowerupDuration(shieldInst, normalized);
            }

            yield return null;
        }

        if (shieldInst.isValid())
            interactablesAudio.SetPowerupDuration(shieldInst, 0f);

        if (shieldInst.isValid())
        {
            shieldInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            shieldInst.release();
        }

        interactablesAudio.PlayShieldBreakAudio();
        hasShield = false;
        shieldRoutine = null;
    }

    public void PlayTurboAudio()
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            return;
        }
        turboInst = interactablesAudio.TurboStartAudio(turboInst);
    }

    public void StopTurboAudio()
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            return;
        }
        turboInst = interactablesAudio.TurboStopAudio(turboInst);
    }
    #endregion
}
