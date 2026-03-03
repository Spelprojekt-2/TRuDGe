using FMOD.Studio;
using UnityEngine;

public class TankTestAudio : MonoBehaviour
{
    [SerializeField] private TanksAudio tanksAudio;
    private EventInstance instance;
    private bool isInstance;

    public void GrappleShoot()
    {
        if (isInstance && instance.isValid())
        {
            instance.stop(STOP_MODE.IMMEDIATE);
        }
        isInstance = true;
        instance = tanksAudio.GrappleStartAudio(instance, gameObject);
    }

    public void GrappleHit()
    {
        tanksAudio.SetGrappleState(instance, TanksAudio.GrappleState.Hit);
    }

    public void GrappleReturn()
    {
        tanksAudio.SetGrappleState(instance, TanksAudio.GrappleState.Return);
    }

    public void GrappleEnd()
    {
        tanksAudio.SetGrappleState(instance, TanksAudio.GrappleState.End);
        isInstance = false;
    }
}
