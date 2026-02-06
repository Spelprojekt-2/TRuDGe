using UnityEngine;
using FMOD.Studio;
using FMODUnity;

[CreateAssetMenu(menuName = "Scriptables/Audio/Tanks")]
public class TanksAudio : ScriptableObject
{
    #region EventReferences
    [SerializeField] private EventReference grappleEvent;
    [SerializeField] private EventReference grappleHookEvent;
    [SerializeField] private EventReference vehicleEngineEvent;
    #endregion

    public enum GrappleState
    {
        Hit = 1,
        Return = 2,
        End = 3
    }

    public EventInstance CreateGrappleInstance(GameObject obj)
    {
        if (grappleEvent.IsNull)
        {
            Debug.LogWarning("TanksAudio: grappleEvent is missing!");
            return new EventInstance();
        }

        EventInstance instance = RuntimeManager.CreateInstance(grappleEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        return instance;
    }

    public EventInstance ChangeGrappleState(EventInstance instance, GrappleState newstate)
    {
        if (!instance.isValid())
        {
            Debug.LogWarning("TanksAudio: given EventInstance is not valid!");
            return instance;
        }

        switch (newstate)
        {
            case GrappleState.Hit:
                instance.setParameterByName("GrappleState", 1f);
                break;
            case GrappleState.Return:
                instance.setParameterByName("GrappleState", 2f);
                break;
            case GrappleState.End:
                instance.setParameterByName("GrappleState", 3f);
                //instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();
                break;
        }
        return instance;
    }
}
