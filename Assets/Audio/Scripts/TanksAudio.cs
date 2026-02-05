using UnityEngine;
using FMOD.Studio;
using FMODUnity;

[CreateAssetMenu(menuName = "Scriptables/Audio/Tanks")]
public class TanksAudio : ScriptableObject
{
    #region EventReferences
    [SerializeField] private EventReference grappleGunEvent;
    [SerializeField] private EventReference grappleHookEvent;
    [SerializeField] private EventReference vehicleEngineEvent;
    #endregion
}
