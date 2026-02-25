using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject subtitlePrefab;
    private TextMeshProUGUI subtext;
    public float subtitleDuration = 5f;
    private bool isShowing = false;
    void Awake()
    {
        Lars_Start();
    }
    void Update()
    {
        /*if (isShowing)
        {
            subtitleDuration += Time.deltaTime;
            if (subtitleDuration >= 5f)
            {
                subtitles.gameObject.SetActive(false);
                isShowing = false;
                subtitleDuration = 0f;
            }
        }*/
    }
    void SpawnSubtitle(string subtext)
    {
        GameObject newSubtitle = Instantiate(subtitlePrefab);
        TextMeshProUGUI text = newSubtitle.GetComponentInChildren<TextMeshProUGUI>();
        text.text = subtext;

        //Destroy(newSubtitle, subtitleDuration);
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
        subtext.text = "Napoleon drive-by";
    }
    public void Napoleon_DrivenBy()
    {
        subtext.text = "Napoleon driven by";
    }
    public void Napoleon_Carla_Crash()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Carla</color><color=grey>]</color> Go away!");
        Napoleon_Carla_Crash2();
    }
    void Napoleon_Carla_Crash2()
    {
        SpawnSubtitle("<color=grey>[</color><color=blue>Napoleon</color><color=grey>]</color> No u.");
    }

    //Nina
    public void Nina_Hit()
    {
        subtext.text = "Nina_hit";
    }

    //Carla
    public void Carla_Win()
    {
        subtext.text = "Carla win";
    }
}
