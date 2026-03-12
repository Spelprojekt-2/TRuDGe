using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Voice")]
public class VoiceAudio : ScriptableObject
{
    [Header("Announcements")]
    [SerializeField] private EventReference SchlammrennstreckeIntroRef;
    [SerializeField] private EventReference CliffsOfDoverIntroRef;
    [SerializeField] private EventReference LuminenTRTIntroRef;

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

    public EventInstance CliffsOfDoverIntroAudio(EventInstance instance)
    {
        if (CliffsOfDoverIntroRef.IsNull)
        {
            Debug.LogError("VoiceAudio: CliffsOfDoverIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(CliffsOfDoverIntroRef);
        instance.start();
        return instance;
    }

    public EventInstance LuminenTRTIntroAudio(EventInstance instance)
    {
        if (LuminenTRTIntroRef.IsNull)
        {
            Debug.LogError("VoiceAudio: LuminenTRTIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(LuminenTRTIntroRef);
        instance.start();
        return instance;
    }
}