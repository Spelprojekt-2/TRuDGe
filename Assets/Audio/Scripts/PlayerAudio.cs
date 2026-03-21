using FMOD.Studio;
using FMODUnity;
using UnityEngine;

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

    public void PlayShieldAudio()
    {
        if (interactablesAudio == null)
        {
            Debug.LogWarning("PlayerAudio: interactablesAudio is missing!");
            return;
        }
        interactablesAudio.PlayShieldAudio();
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
