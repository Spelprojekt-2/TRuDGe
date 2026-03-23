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

    [Header("ANN_FirstPlace")]
    [SerializeField] private EventReference ANN_Napoleon_First;

    [Header("VO_FirstPlace")]
    [SerializeField] private EventReference VO_Napoleon_First;

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
    
    public EventInstance ANN_NapoleonFirstPlace(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(ANN_Napoleon_First);
        instance.start();
        return instance;
    }

    public EventInstance VO_NapoleonFirstPlace(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(VO_Napoleon_First);
        instance.start();
        return instance;
    }
}