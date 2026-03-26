using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    #region Configuration
    public static AudioManager Instance { get; private set; }

    [Header("Pause Configuration")]
    [Tooltip("List of bus paths to pause, e.g., 'bus:/SFX', 'bus:/Music'")]
    [SerializeField] private List<string> busesToMute;
    [SerializeField] private EventReference PauseSnapshotRef;
    private EventInstance pauseSnapInst;
    private bool isPaused = false;

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

    [Header("Voice Configuration")]
    [SerializeField] private VoiceAudio voiceAudio;
    private EventInstance ann_vo_inst;
    private EventInstance char_vo_inst;

    public enum AmbienceID
    {
        None = 0,
        TrainingGround = 1,
        Schlammrennstrecke = 2,
        CliffsOfDover = 3,
        LuminenTRT = 4
    }
    private AmbienceID currentAmbience = AmbienceID.None;

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
    
    #region Music Fade

    [SerializeField] private float musicTransitionDuration = 2f; // fade duration
    private EventInstance nextMusic;

// Call this when you want to go from Main Menu → Selection Screen
    public void CrossfadeMainMenuToSelection()
    {
        if (!musicInstance.isValid())
            return;

        nextMusic = RuntimeManager.CreateInstance(Music_SelectionScreenRef);
        nextMusic.start();
        StartCoroutine(FadeMusicCoroutine(musicInstance, nextMusic, musicTransitionDuration));
        musicInstance = nextMusic;
    }

// Coroutine that handles the fade
    private System.Collections.IEnumerator FadeMusicCoroutine(EventInstance fromMusic, EventInstance toMusic, float duration)
    {
        float timer = 0f;
        toMusic.setVolume(0f);

        while (timer < duration)
        {
            // Ensure we don't go past 'duration'
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration); // t is always between 0 and 1

            fromMusic.setVolume(1f - t); // fade out
            toMusic.setVolume(t);        // fade in

            yield return null;
        }

        // Guarantee final volume values
        fromMusic.setVolume(0f);
        toMusic.setVolume(1f);

        fromMusic.stop(STOP_MODE.ALLOWFADEOUT);
        fromMusic.release();
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
        if (ann_vo_inst.isValid())
        {
            ann_vo_inst.stop(STOP_MODE.IMMEDIATE);
            ann_vo_inst.release();
        }
    }

    public void PlaySchlammenstreckeIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (ann_vo_inst.isValid())
        {
            ann_vo_inst.stop(STOP_MODE.IMMEDIATE);
            ann_vo_inst.release();
        }
        
        ann_vo_inst = voiceAudio.SchlammenstreckeIntroAudio(ann_vo_inst);
    }

    public void PlayCliffsOfDoverIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (ann_vo_inst.isValid())
        {
            ann_vo_inst.stop(STOP_MODE.IMMEDIATE);
            ann_vo_inst.release();
        }
        
        ann_vo_inst = voiceAudio.CliffsOfDoverIntroAudio(ann_vo_inst);
    }

    public void PlayLuminenTRTIntro()
    {
        if (voiceAudio == null)
        {
            Debug.LogError("VoiceAudio is missing!");
            return;
        }
        if (ann_vo_inst.isValid())
        {
            ann_vo_inst.stop(STOP_MODE.IMMEDIATE);
            ann_vo_inst.release();
        }
        
        ann_vo_inst = voiceAudio.LuminenTRTIntroAudio(ann_vo_inst);
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

    public void PlayTakeLeadANN(string name)
    {
        if (ann_vo_inst.isValid())
        {
            ann_vo_inst.stop(STOP_MODE.IMMEDIATE);
            ann_vo_inst.release();
        }

        string lower = name.ToLower();
        switch (lower)
        {
            case "king napoleon iii":
                ann_vo_inst = voiceAudio.ANN_NapoleonTakeLead(ann_vo_inst);
                break;
            case "lars-göran":
                ann_vo_inst = voiceAudio.ANN_LarsTakeLead(ann_vo_inst);
                break;
            case "capôw":
                ann_vo_inst = voiceAudio.ANN_CarlaTakeLead(ann_vo_inst);
                break;
            case "the brass beast":
                ann_vo_inst = voiceAudio.ANN_NinaTakeLead(ann_vo_inst);
                break;
        }
    }

    public void PlayTakeLeadVO(string name)
    {
        if (char_vo_inst.isValid())
        {
            char_vo_inst.stop(STOP_MODE.IMMEDIATE);
            char_vo_inst.release();
        }

        string lower = name.ToLower();
        switch (lower)
        {
            case "king napoleon iii":
                char_vo_inst = voiceAudio.VO_NapoleonTakeLead(char_vo_inst);
                break;
            case "lars-göran":
                char_vo_inst = voiceAudio.VO_LarsTakeLead(char_vo_inst);
                break;
            case "capôw":
                char_vo_inst = voiceAudio.VO_CarlaTakeLead(char_vo_inst);
                break;
            case "the brass beast":
                char_vo_inst = voiceAudio.VO_NinaTakeLead(char_vo_inst);
                break;
        }
    }

    public void PlayLostLeadVO(string name)
    {
        if (char_vo_inst.isValid())
        {
            char_vo_inst.stop(STOP_MODE.IMMEDIATE);
            char_vo_inst.release();
        }

        string lower = name.ToLower();
        switch (lower)
        {
            case "king napoleon iii":
                char_vo_inst = voiceAudio.VO_NapoleonLostLead(char_vo_inst);
                break;
            case "lars-göran":
                char_vo_inst = voiceAudio.VO_LarsLostLead(char_vo_inst);
                break;
            case "capôw":
                char_vo_inst = voiceAudio.VO_CarlaLostLead(char_vo_inst);
                break;
            case "the brass beast":
                char_vo_inst = voiceAudio.VO_NinaLostLead(char_vo_inst);
                break;
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
        ChangeAmbience(currentAmbience);
    }

    private void ChangeAmbience(AmbienceID id)
    {
        if (id == currentAmbience)
        {
            return;
        }
        currentAmbience = id;

        // Fail check
        if (!ambienceInstance.isValid())
        {
            Debug.LogWarning("AmbienceInstance is not valid!");
            return;
        }
        RuntimeManager.StudioSystem.setParameterByName("CurrentLocation", (float)id);
    }
    #endregion

    #region Pause / Resume
    public void TogglePause(bool pause)
    {
        if (pause == isPaused)
        {
            return;
        }
        isPaused = pause;

        // Pause buses
        foreach (string busPath in busesToMute)
        {
            Bus bus = RuntimeManager.GetBus(busPath);

            if (bus.isValid())
            {
                bus.setPaused(pause);
            }
            else
            {
                Debug.LogWarning($"Bus not found: {busPath}");
            }
        }

        if (pause)
        {
            EnablePauseSnapshot();
        }
        else
        {
            DisablePauseSnapshot();
        }
    }

    private void EnablePauseSnapshot()
    {
        if (pauseSnapInst.isValid() || PauseSnapshotRef.IsNull)
        {
            Debug.LogWarning("AudioManager: Audio is either already paused or the PauseSnapshotRef is missing!");
            return;
        }

        pauseSnapInst = RuntimeManager.CreateInstance(PauseSnapshotRef);
        pauseSnapInst.start();
    }

    private void DisablePauseSnapshot()
    {
        if (!pauseSnapInst.isValid())
        {
            return;
        }

        pauseSnapInst.stop(STOP_MODE.IMMEDIATE);
        pauseSnapInst.release();
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
                break;

            case 1:
                ChangeMusic(MusicID.SelectionScreen);
                ChangeAmbience(AmbienceID.None);
                break;

            case 2:
                ChangeMusic(MusicID.SelectionScreen);
                ChangeAmbience(AmbienceID.None);
                break;

            case 3:
                ChangeMusic(MusicID.SelectionScreen);
                ChangeAmbience(AmbienceID.None);
                break;

            case 9:
                ChangeMusic(MusicID.Level1sloped);
                ChangeAmbience(AmbienceID.Schlammrennstrecke);
                break;

            case 10:
                ChangeMusic(MusicID.TrainingGround);
                ChangeAmbience(AmbienceID.TrainingGround);
                break;

            case 12:
                ChangeMusic(MusicID.Dover);
                ChangeAmbience(AmbienceID.CliffsOfDover);
                break;

            case 15:
                ChangeMusic(MusicID.Luminen);
                ChangeAmbience(AmbienceID.LuminenTRT);
                break;
            case 16:
                ChangeMusic(MusicID.TrainingGround);
                ChangeAmbience(AmbienceID.TrainingGround);
                break;
        }
    }
    #endregion
}