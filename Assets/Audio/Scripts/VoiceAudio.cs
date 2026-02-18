using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Audio/Voice")]
public class VoiceAudio : ScriptableObject
{
    [SerializeField] private EventReference Schlammrennstrecke_AnnouncementRef;

    public void PlayAnnouncement()
    {
        RuntimeManager.PlayOneShot(Schlammrennstrecke_AnnouncementRef);
    }
}
