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

    public void PlayGasolinePickup(GameObject gasolinePickup)
    {
        Debug.LogWarning("PlayGasolinePickup!");
        RuntimeManager.PlayOneShot(GasolinePickupEvent, gasolinePickup.transform.position);
    }
}
