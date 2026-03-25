using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.PlayerSettings;

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
    [SerializeField] private EventReference SmokeRef;
    [SerializeField] private EventReference ShieldRef;
    [SerializeField] private EventReference ShielBreakdRef;
    [SerializeField] private EventReference WallPlaceRef;
    [SerializeField] private EventReference ScatterShotRef;
    [SerializeField] private EventReference TurboRef;
    [SerializeField] private EventReference AirstrikeRef;

    [Header("Duration handlers")]
    [SerializeField] private EventReference ShieldDurationRef;
    #endregion

    public EventInstance SetPowerupDuration(EventInstance instance, float duration)
    {
        instance.setParameterByName("PowerupDuration", duration);
        return instance;
    }

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

    public void LandminePlaceAudio(GameObject tankOBJ)
    {
        if (LandminePlaceRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: LandminePlaceRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShotAttached(LandminePlaceRef, tankOBJ);
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

    public void PlaySmokeAudio(Transform pos)
    {
        if (SmokeRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: SmokeRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(SmokeRef, pos.position);
    }

    public void PlayShieldUseAudio()
    {
        if (ShieldRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: ShieldRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ShieldRef);
    }

    public EventInstance ShieldDurationAudio(EventInstance instance)
    {
        if (ShieldDurationRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: ShieldDurationRef is missing!");
            return instance;
        }

        instance = RuntimeManager.CreateInstance(ShieldDurationRef);
        instance.start();
        return instance;
    }

    public void PlayShieldBreakAudio()
    {
        if (ShielBreakdRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: ShielBreakdRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ShielBreakdRef);
    }

    public void PlaceWallAudio(Transform pos)
    {
        if (WallPlaceRef.IsNull)
        {
            Debug.LogError("InteractablesAudio: WallPlaceRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(WallPlaceRef, pos.position);
    }

    public void ScatterShotAudio()
    {
        if (ScatterShotRef.IsNull)
        {
            Debug.LogError("ScatterShotRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(ScatterShotRef);
    }

    public EventInstance TurboStartAudio(EventInstance instance)
    {
        if (TurboRef.IsNull)
        {
            Debug.LogError("TurboRef is missing!");
            return instance;
        }

        if (instance.isValid())
        {
            TurboStopAudio(instance);
        }

        instance = RuntimeManager.CreateInstance(TurboRef);
        instance.start();
        return instance;
    }

    public EventInstance TurboStopAudio(EventInstance instance)
    {
        instance.keyOff();
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        return instance;
    }

    public void PlayAirstrikeAudio()
    {
        if (AirstrikeRef.IsNull)
        {
            Debug.LogError("AirstrikeRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShot(AirstrikeRef);
    }
}
