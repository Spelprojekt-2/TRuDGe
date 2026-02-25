using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject subtitlePrefab;
    public Transform subtitleParent;
    //public float subtitleDuration = 5f;
    void Awake()
    {
        Napoleon_Carla_Crash();
    }
    void SpawnSubtitle(string subtext, float duration)
    {
        GameObject newSubtitle = Instantiate(subtitlePrefab, subtitleParent, false);
        newSubtitle.SetActive(true);

        TextMeshProUGUI text = newSubtitle.GetComponentInChildren<TextMeshProUGUI>();
        text.text = subtext;

        Destroy(newSubtitle, duration);
    }

    // ======== Voice line subtitles =========
    //Lars-Göran
    public void Lars_Start()
    {
        SpawnSubtitle("I hope no one decides to cheat today!", 5f);
    }

    //Napoleon
    public void Napoleon_DriveBy()
    {
        SpawnSubtitle("Napoleon drive-by", 5f);
    }
    public void Napoleon_DrivenBy()
    {
        SpawnSubtitle("Napoleon driven by", 5f);
    }
    public void Napoleon_Carla_Crash()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Carla</color><color=grey>]</color> Go away!", 5f);
        Invoke(nameof(NapoleonReply_Carla_Crash), 3f);
    }
    void NapoleonReply_Carla_Crash()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Napoleon</color><color=grey>]</color> No u.", 3f);
    }

    //Nina
    public void Nina_Hit()
    {
        SpawnSubtitle("Nina_hit", 5f);
    }

    //Carla
    public void Carla_Win()
    {
        SpawnSubtitle("Carla win", 5f);
    }
}
