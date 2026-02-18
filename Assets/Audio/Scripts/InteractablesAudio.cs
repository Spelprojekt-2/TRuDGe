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
            Debug.LogWarning("InteractablesAudio: LandminePlaceRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShotAttached(LandminePlaceRef, landmineOBJ);
    }

    public void LandmineTriggerAudio(GameObject landmineOBJ)
    {
        if (LandmineTriggerRef.IsNull)
        {
            Debug.LogWarning("InteractablesAudio: LandmineTriggerRef is missing!");
            return;
        }
        RuntimeManager.PlayOneShotAttached(LandmineTriggerRef, landmineOBJ);
    }
}
