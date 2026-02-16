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
    [SerializeField] private EventReference shootEvent;
    #endregion

    public enum GrappleState
    {
        Hit = 1,
        Return = 2,
        End = 3
    }

    public EventInstance GrappleStartAudio(EventInstance instance, GameObject obj)
    {
        // Fail check
        if (grappleEvent.IsNull)
        {
            Debug.LogWarning("TanksAudio: grappleEvent is missing!");
            return instance;
        }

        // Create instance
        instance = RuntimeManager.CreateInstance(grappleEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        Debug.LogWarning("START");
        return instance;
    }

    public EventInstance SetGrappleState(EventInstance instance, GrappleState newstate)
    {
        // Fail check
        if (!instance.isValid())
        {
            Debug.LogWarning("TanksAudio: given EventInstance is not valid!");
            return instance;
        }

        switch (newstate)
        {
                // ___Hit state:
            case GrappleState.Hit:
                instance.setParameterByName("GrappleState", 1f);
                break;
                // ___Return state:
            case GrappleState.Return:
                instance.setParameterByName("GrappleState", 2f);
                break;
                // ___End state:
            case GrappleState.End:
                instance.setParameterByName("GrappleState", 3f);
                Debug.LogWarning("END");
                instance.release();
                break;
        }
        return instance;
    }

    public EventInstance ShootStartAudio(EventInstance instance, GameObject obj)
    {
        // Fail check
        if (shootEvent.IsNull)
        {
            Debug.LogWarning("TanksAudio: shootEvent is missing!");
            return instance;
        }

        // Create instance
        instance = RuntimeManager.CreateInstance(shootEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        return instance;
    }

    public EventInstance ShootHitAudio(EventInstance instance)
    {
        // Fail check
        if (!instance.isValid())
        {
            Debug.LogWarning("TanksAudio: given EventInstance is not valid!");
            return instance;
        }
        instance.setParameterByName("Hit", 1f);
        instance.release();
        return instance;
    }

    public EventInstance ShootEndAudio(EventInstance instance)
    {
        // Fail check
        if (!instance.isValid())
        {
            Debug.LogWarning("TanksAudio: given EventInstance is not valid!");
            return instance;
        }
        instance.keyOff();
        return instance;
    }
}
