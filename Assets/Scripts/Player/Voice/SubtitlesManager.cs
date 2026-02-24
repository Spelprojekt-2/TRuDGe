using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleBox;

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
        subtitleBox.text = "Go away!";
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
        Lars_Start();
    }
}
