using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject subtitlePrefab;
    public Transform subtitleParent;
    public float subtitleDuration = 5f;
    void Awake()
    {
        //Image image = subtitlePrefab.GetComponentInChildren<Image>();

        Napoleon_Carla_Crash();
    }
    void SpawnSubtitle(string subtext)
    {
        GameObject newSubtitle = Instantiate(subtitlePrefab, subtitleParent);
        RectTransform rect = newSubtitle.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI text = newSubtitle.GetComponentInChildren<TextMeshProUGUI>();
        text.text = subtext;

        Destroy(newSubtitle, subtitleDuration);
    }

    // ======== Voice line subtitles =========
    //Lars-Göran
    public void Lars_Start()
    {
        SpawnSubtitle("I hope no one decides to cheat today!");
    }

    //Napoleon
    public void Napoleon_DriveBy()
    {
        SpawnSubtitle("Napoleon drive-by");
    }
    public void Napoleon_DrivenBy()
    {
        SpawnSubtitle("Napoleon driven by");
    }
    public void Napoleon_Carla_Crash()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Carla</color><color=grey>]</color> Go away!");
        Invoke(nameof(NapoleonReply_Carla_Crash), 5f);
    }
    void NapoleonReply_Carla_Crash()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Napoleon</color><color=grey>]</color> No u.");
    }

    //Nina
    public void Nina_Hit()
    {
        SpawnSubtitle("Nina_hit");
    }

    //Carla
    public void Carla_Win()
    {
        SpawnSubtitle("Carla win");
    }
}
