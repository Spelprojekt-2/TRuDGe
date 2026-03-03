using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Scriptables/Audio/Interactables")]
public class InteractablesAudio : ScriptableObject
{
    #region EventReferences
    [Header("Pickups")]
    [SerializeField] private EventReference BoxPickupEvent;
    [SerializeField] private EventReference GasolinePickupEvent;

    [Header("Usage")]
    [SerializeField] private EventReference LandminePlaceRef;
    [SerializeField] private EventReference LandmineTriggerRef;
    [SerializeField] private EventReference MagnetRef;
    #endregion

    public void PlayPickupAudio(PlayerPowerups.PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            default:
                RuntimeManager.PlayOneShot(BoxPickupEvent);
                break;
            case PlayerPowerups.PowerUpType.gasolineTank:
                RuntimeManager.PlayOneShot(GasolinePickupEvent);
                break;
        }
    }

    public void LandminePlaceAudio(GameObject landmineOBJ)
    {
        if (LandminePlaceRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: LandminePlaceRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShotAttached(LandminePlaceRef, landmineOBJ);
    }

    public void LandmineTriggerAudio(GameObject landmineOBJ)
    {
        if (LandmineTriggerRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: LandmineTriggerRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShotAttached(LandmineTriggerRef, landmineOBJ);
    }

    public EventInstance StartMagnetAudio(EventInstance instance, GameObject obj)
    {
        if (MagnetRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: MagnetRef is missing!");
            return instance;
        }

        if (instance.isValid())
        {
            instance = StopMagnetAudio(instance);
        }

        // Create magnet instance
        instance = RuntimeManager.CreateInstance(MagnetRef);
        RuntimeManager.AttachInstanceToGameObject(instance, obj);
        instance.start();
        return instance;
    }

    public EventInstance StopMagnetAudio(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.release();
        return instance;
    }
}
