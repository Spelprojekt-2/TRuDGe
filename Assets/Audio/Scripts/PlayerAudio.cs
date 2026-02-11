using FMOD.Studio;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private TanksAudio tanksAudio;

    // EventInstances
    private EventInstance grappleInstance;
    private EventInstance shootInstance;

    // GameOBJs
    [SerializeField] private GameObject grapplePos;

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

    private void OnDisable()
    {
        if (grappleInstance.isValid())
            GrappleEnd();
        if (shootInstance.isValid())
            ShootEnd();
    }

    #region ShootProjectileFunctions
    public void ShootStart(GameObject projectileOBJ)
    {
        if (hasShoot && shootInstance.isValid())
        {
            shootInstance.stop(STOP_MODE.IMMEDIATE);
        }
        hasShoot = true;
        shootInstance = tanksAudio.ShootStartAudio(new EventInstance(), projectileOBJ);
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
        Debug.LogError("called");
        if (hasGrapple && shootInstance.isValid())
        {
            grappleInstance.stop(STOP_MODE.IMMEDIATE);
        }
        hasGrapple = true;
        grappleInstance = tanksAudio.GrappleStartAudio(new EventInstance(), grapplePos);
    }

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

    public void GrappleEnd()
    {
        if (hasGrapple)
        {
            tanksAudio.SetGrappleState(grappleInstance, TanksAudio.GrappleState.End);
            hasGrapple = false;
        }
    }
    #endregion
}
