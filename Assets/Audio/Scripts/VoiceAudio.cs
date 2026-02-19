using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptables/Audio/Voice")]
public class VoiceAudio : ScriptableObject
{
    [SerializeField] private EventReference Schlammrennstrecke_AnnouncementRef;
    EventInstance announcementInst;

    public void PlayAnnouncement()
    {
        if (Schlammrennstrecke_AnnouncementRef.IsNull)
        {
            return;
        }

        if (announcementInst.isValid())
        {
            StopAnnouncment();
        }
        announcementInst = RuntimeManager.CreateInstance(Schlammrennstrecke_AnnouncementRef);
        announcementInst.start();
    }

    public void StopAnnouncment()
    {
        announcementInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        announcementInst.release();
    }
}
