using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Voice")]
public class VoiceAudio : ScriptableObject
{
    [SerializeField] private EventReference SchlammrennstreckeIntroRef;

    public EventInstance SchlammenstreckeIntroAudio(EventInstance instance)
    {
        if (SchlammrennstreckeIntroRef.IsNull)
        {
            Debug.LogError("VoiceAudio: SchlammrennstreckeIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(SchlammrennstreckeIntroRef);
        instance.start();
        return instance;
    }
}
