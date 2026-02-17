using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Feedback")]
public class FeedbackAudio : ScriptableObject
{
    [SerializeField] EventReference CountdownRef;

    public void CountdownAudio(bool endCountdown = false)
    {
        if (CountdownRef.IsNull)
        {
            Debug.LogError("FeedbackAudio: CountdownRef is missing!");
            return;
        }

        // Play oneshot countdown audio
        if (endCountdown)
        {
            RuntimeManager.StudioSystem.setParameterByName("EndCountdown", 1);
        }
        else
        {
            RuntimeManager.StudioSystem.setParameterByName("EndCountdown", 0);
        }
        RuntimeManager.PlayOneShot(CountdownRef);
    }
}
