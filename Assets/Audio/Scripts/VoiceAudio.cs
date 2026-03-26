using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Voice")]
public class VoiceAudio : ScriptableObject
{
    [Header("ANN_Intros")]
    [SerializeField] private EventReference SchlammrennstreckeIntro;
    [SerializeField] private EventReference CliffsOfDoverIntro;
    [SerializeField] private EventReference LuminenTRTIntro;

    [Header("ANN_TakeLead")]
    [SerializeField] private EventReference ANN_Napoleon_TakeLead;
    [SerializeField] private EventReference ANN_Lars_TakeLead;
    [SerializeField] private EventReference ANN_Carla_TakeLead;
    [SerializeField] private EventReference ANN_Nina_TakeLead;

    [Header("Character_TakeLead")]
    [SerializeField] private EventReference Char_Napoleon_TakeLead;
    [SerializeField] private EventReference Char_Lars_TakeLead;
    [SerializeField] private EventReference Char_Carla_TakeLead;
    [SerializeField] private EventReference Char_Nina_TakeLead;

    [Header("Character_LostLead")]
    [SerializeField] private EventReference Char_Napoleon_LostLead;
    [SerializeField] private EventReference Char_Lars_LostLead;
    [SerializeField] private EventReference Char_Carla_LostLead;
    [SerializeField] private EventReference Char_Nina_LostLead;

    #region Intro
    public EventInstance SchlammenstreckeIntroAudio(EventInstance instance)
    {
        if (SchlammrennstreckeIntro.IsNull)
        {
            Debug.LogError("VoiceAudio: SchlammrennstreckeIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(SchlammrennstreckeIntro);
        instance.start();
        return instance;
    }

    public EventInstance CliffsOfDoverIntroAudio(EventInstance instance)
    {
        if (CliffsOfDoverIntro.IsNull)
        {
            Debug.LogError("VoiceAudio: CliffsOfDoverIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(CliffsOfDoverIntro);
        instance.start();
        return instance;
    }

    public EventInstance LuminenTRTIntroAudio(EventInstance instance)
    {
        if (LuminenTRTIntro.IsNull)
        {
            Debug.LogError("VoiceAudio: LuminenTRTIntroRef is missing!");
            return instance;
        }
        instance = RuntimeManager.CreateInstance(LuminenTRTIntro);
        instance.start();
        return instance;
    }
    #endregion

    #region Napoleon
    public EventInstance ANN_NapoleonTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(ANN_Napoleon_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_NapoleonTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Napoleon_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_NapoleonLostLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Napoleon_LostLead);
        instance.start();
        return instance;
    }
    #endregion

    #region Lars
    public EventInstance ANN_LarsTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(ANN_Lars_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_LarsTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Lars_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_LarsLostLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Lars_LostLead);
        instance.start();
        return instance;
    }
    #endregion

    #region Carla
    public EventInstance ANN_CarlaTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(ANN_Carla_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_CarlaTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Carla_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_CarlaLostLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Carla_LostLead);
        instance.start();
        return instance;
    }
    #endregion

    #region Nina
    public EventInstance ANN_NinaTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(ANN_Nina_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_NinaTakeLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Nina_TakeLead);
        instance.start();
        return instance;
    }

    public EventInstance VO_NinaLostLead(EventInstance instance)
    {
        instance = RuntimeManager.CreateInstance(Char_Nina_LostLead);
        instance.start();
        return instance;
    }
    #endregion
}