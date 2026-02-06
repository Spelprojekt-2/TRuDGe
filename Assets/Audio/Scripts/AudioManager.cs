using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    #region Configuration
    // Här ligger variabler etc.
    [Tooltip("List of bus paths to pause, e.g., 'bus:/SFX', 'bus:/Music'")]
    [SerializeField] private List<string> busesToMute;

    #endregion

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
        }
    }

    /// <summary>
    /// Pause all FMOD audio
    /// </summary>
    public void PauseAudio()
    {
        TogglePause(true);
        Debug.Log("Audio paused");
    }

    /// <summary>
    /// Resume all FMOD audio
    /// </summary>
    public void ResumeAudio()
    {
        TogglePause(false);
        Debug.Log("Audio resumed");
    }

    private void TogglePause(bool pause)
    {
        foreach (string busPath in busesToMute)
        {
            Bus bus = RuntimeManager.GetBus(busPath); // Convert path string to Bus
            if (bus.isValid())
            {
                bus.setPaused(pause);
            }
            else
            {
                Debug.LogWarning($"Bus not found: {busPath}");
            }
        }
    }

    #region Music
    // Här kan ni skriva musik kod:

    #endregion

    #region SFX
    // Här skriver Oskar SFX kod:

    #endregion
}
