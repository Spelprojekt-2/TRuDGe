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
        instance = tanksAudio.CreateGrappleInstance(gameObject);
    }

    public void GrappleHit()
    {
        tanksAudio.ChangeGrappleState(instance, TanksAudio.GrappleState.Hit);
    }

    public void GrappleReturn()
    {
        tanksAudio.ChangeGrappleState(instance, TanksAudio.GrappleState.Return);
    }

    public void GrappleEnd()
    {
        tanksAudio.ChangeGrappleState(instance, TanksAudio.GrappleState.End);
        isInstance = false;
    }
}
