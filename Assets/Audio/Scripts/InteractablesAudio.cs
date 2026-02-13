using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Scriptables/Audio/Interactables")]
public class InteractablesAudio : ScriptableObject
{
    #region EventReferences
    [SerializeField] private EventReference GasolinePickupEvent;
    #endregion

    public void PlayPickupAudio(GameObject obj, PlayerPowerups.PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PlayerPowerups.PowerUpType.gasolineTank:
                RuntimeManager.PlayOneShot(GasolinePickupEvent, obj.transform.position);
                break;
        }
    }
}
