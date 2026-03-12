using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    #region Configuration
    public static AudioManager Instance { get; private set; }

    [Tooltip("List of bus paths to pause, e.g., 'bus:/SFX', 'bus:/Music'")]
    [SerializeField] private List<string> busesToMute;

    // Music config
    [Header("Music Configuration")]
    [SerializeField] private EventReference Music_MainMenuRef;
    [SerializeField] private EventReference Music_SelectionScreenRef;
    [SerializeField] private EventReference Music_TimeTrailRef;
    [SerializeField] private EventReference Music_Level1_slopedRef;
    [SerializeField] private EventReference Music_TrainingGround;
    [SerializeField] private EventReference Music_Level2; // Dover
    [SerializeField] private EventReference Music_Level3; // Luminen

    [Header("Music_Victory")]
    [SerializeField] private EventReference Music_Napoleon;
    [SerializeField] private EventReference Music_Lars;
    [SerializeField] private EventReference Music_Carla;
    [SerializeField] private EventReference Music_Nina;

    private EventInstance musicInstance;

    public enum MusicID
    {
        None = 0,
        MainMenu = 1,
        SelectionScreen = 2,
        TimeTrail = 3,
        Level1sloped = 4,
        TrainingGround = 5,
        Dover = 6,
        Luminen = 7
    }

    private MusicID currentMusic = MusicID.None;

    // SFX config
    [Header("SFX Configuration")]
    [SerializeField] private FeedbackAudio feedbackAudio;
    private EventInstance countDownInst;

    [Header("VO Configuration")]
    [SerializeField] private VoiceAudio voiceAudio;
    private EventInstance VOinst_Announcer;
    private EventInstance VOinst_Character;

    public enum AmbienceID
    {
        None = 0,
        TrainingGround = 1,
        Schlammrennstrecke = 2,
        CliffsOfDover = 3,
        LuminenTRT = 4
    }

    // Ambience config
    [SerializeField] private EventReference AmbienceManagerRef;
    private EventInstance ambienceInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        StartAmbience();
        ChangeMusic(MusicID.MainMenu);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region Music
    public void ChangeMusic(MusicID musicID)
    {
        if (musicID == currentMusic)
        {
            return;
        }
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }

        switch (musicID)
        {
            case MusicID.MainMenu:
                musicInstance = RuntimeManager.CreateInstance(Music_MainMenuRef);
                Debug.Log("MAIN MENU MUSIC");
                break;

            case MusicID.SelectionScreen:
                musicInstance = RuntimeManager.CreateInstance(Music_SelectionScreenRef);
                break;

            case MusicID.TimeTrail:
                musicInstance = RuntimeManager.CreateInstance(Music_TimeTrailRef);
                break;

            case MusicID.Level1sloped:
                musicInstance = RuntimeManager.CreateInstance(Music_Level1_slopedRef);
                break;
            
            case MusicID.TrainingGround:
                musicInstance = RuntimeManager.CreateInstance(Music_TrainingGround);
                break;

            case MusicID.Dover:
                musicInstance = RuntimeManager.CreateInstance(Music_Level2);
                break;
            case MusicID.Luminen:
                musicInstance = RuntimeManager.CreateInstance(Music_Level3);
                break;
        }
        currentMusic = musicID;
        musicInstance.start();
    }
    #endregion

    #region SFX
    public void PlayCountdownAudio(bool endCountdown = false)
    {
        if (feedbackAudio == null)
        {
            Debug.LogWarning("AudioManager: FeedbackAudio is missing!");
            return;
        }
        feedbackAudio.CountdownAudio(endCountdown);
    }
    #endregion

    #region VO
    public void StopIntro()
    {
        if (VOinst_Announcer.isValid())
        {
            VOinst_Announcer.stop(STOP_MODE.IMMEDIATE);
            VOinst_Announcer.release();
        }
    }

    public void PlaySchlammenstreckeIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (VOinst_Announcer.isValid())
        {
            VOinst_Announcer.stop(STOP_MODE.IMMEDIATE);
            VOinst_Announcer.release();
        }
        
        VOinst_Announcer = voiceAudio.SchlammenstreckeIntroAudio(VOinst_Announcer);
    }

    public void PlayCliffsOfDoverIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (VOinst_Announcer.isValid())
        {
            VOinst_Announcer.stop(STOP_MODE.IMMEDIATE);
            VOinst_Announcer.release();
        }
        
        VOinst_Announcer = voiceAudio.CliffsOfDoverIntroAudio(VOinst_Announcer);
    }

    public void PlayLuminenTRTIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (VOinst_Announcer.isValid())
        {
            VOinst_Announcer.stop(STOP_MODE.IMMEDIATE);
            VOinst_Announcer.release();
        }
        
        VOinst_Announcer = voiceAudio.LuminenTRTIntroAudio(VOinst_Announcer);
    }

    public void PlayVictoryVoice(string characterName)
    {
        // Audio instance setup
        if (musicInstance.isValid())
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }

        // Select character
        string lower = characterName.ToLower();
        switch (lower)
        {
            case "king napoleon iii":
                if (!Music_Napoleon.IsNull)
                musicInstance = RuntimeManager.CreateInstance(Music_Napoleon);
                break;
            case "lars-göran":
            if (!Music_Lars.IsNull)
                musicInstance = RuntimeManager.CreateInstance(Music_Lars);
                break;
            case "capôw":
                if (!Music_Carla.IsNull)
                musicInstance = RuntimeManager.CreateInstance(Music_Carla);
                break;
            case "the brass beast":
                if (!Music_Nina.IsNull)
                musicInstance = RuntimeManager.CreateInstance(Music_Nina);
                break;
        }

        // Did we create a valid audio inst?
        if (musicInstance.isValid())
        {
            musicInstance.start();
        }
    }
    #endregion

    #region Ambience
    private void StartAmbience()
    {
        if (ambienceInstance.isValid())
        {
            return;
        }
        ambienceInstance = RuntimeManager.CreateInstance(AmbienceManagerRef);
        ambienceInstance.start();
    }

    private void ChangeAmbience(AmbienceID id)
    {
        if (!ambienceInstance.isValid())
        {
            Debug.LogWarning("AmbienceInstance is not valid!");
            return;
        }
        ambienceInstance.setParameterByName("CurrentLocation", (float)id);
    }
    #endregion

    #region Pause / Resume
    public void PauseAudio()
    {
        TogglePause(true);
        Debug.Log("Audio paused");
    }

    public void ResumeAudio()
    {
        TogglePause(false);
        Debug.Log("Audio resumed");
    }

    private void TogglePause(bool pause)
    {
        foreach (string busPath in busesToMute)
        {
            Bus bus = RuntimeManager.GetBus(busPath);

            if (bus.isValid())
                bus.setPaused(pause);
            else
                Debug.LogWarning($"Bus not found: {busPath}");
        }
    }
    #endregion

    #region Scene Handling
    private void OnSceneLoaded(Scene next, LoadSceneMode mode)
    {
        switch (next.buildIndex)
        {
            case 0:
                ChangeMusic(MusicID.MainMenu);
                ChangeAmbience(AmbienceID.None);
                Debug.Log("Ambience set: NONE");
                break;

            case 1:
                ChangeMusic(MusicID.MainMenu);
                ChangeAmbience(AmbienceID.None);
                Debug.Log("Ambience set: NONE");
                break;

            case 2:
                ChangeMusic(MusicID.MainMenu);
                ChangeAmbience(AmbienceID.None);
                Debug.Log("Ambience set: NONE");
                break;

            case 3:
                ChangeMusic(MusicID.MainMenu);
                ChangeAmbience(AmbienceID.None);
                Debug.Log("Ambience set: NONE");
                break;

            case 9:
                ChangeMusic(MusicID.Level1sloped);
                ChangeAmbience(AmbienceID.Schlammrennstrecke);
                Debug.Log("Ambience set: Schlammrennstrecke");
                break;

            case 10:
                ChangeMusic(MusicID.TrainingGround);
                ChangeAmbience(AmbienceID.TrainingGround);
                Debug.Log("Ambience set: TrainingGround");
                break;

            case 12:
                ChangeMusic(MusicID.Dover);
                ChangeAmbience(AmbienceID.CliffsOfDover);
                Debug.Log("Ambience set: CliffsOfDover");
                break;

            case 15:
                ChangeMusic(MusicID.Luminen);
                ChangeAmbience(AmbienceID.LuminenTRT);
                Debug.Log("Ambience set: LuminenTRT");
                break;
        }
    }
    #endregion
}