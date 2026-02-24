using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleBox;
    private float timer;
    private int subtitleIndex = 0;

    //Voice line subtitles
    //Lars-Göran
    public void Lars_Start()
    {
        subtitleBox.text = "I hope no one decides to cheat today!";
    }

    //Napoleon
    public void Napoleon_DriveBy()
    {
        subtitleBox.text = "Napoleon drive-by";
    }
    public void Napoleon_DrivenBy()
    {
        subtitleBox.text = "Napoleon driven by";
    }
    public void Napoleon_Carla_Crash()
    {
        timer += Time.deltaTime;
        if (timer <= 5f)
        {
            subtitleBox.text = "<color=grey>[</color><color=blue>Carla</color><color=grey>]</color> Go away!";
        }
        if (timer >= 5f)
        {
            subtitleBox.text = "No u.";
        }

    }

    //Nina
    public void Nina_Hit()
    {
        subtitleBox.text = "Nina_hit";
    }

    //Carla
    public void Carla_Win()
    {
        subtitleBox.text = "Carla win";
    }

    void Awake()
    {
        Napoleon_Carla_Crash();
    }
}
