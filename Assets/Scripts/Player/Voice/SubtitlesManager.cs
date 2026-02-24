using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleBox;

    public void ChangeSubtitle()
    {
        subtitleBox.text = Lars_Start;
    }

    //Voice line subtitles
    //Lars-Göran
    private string Lars_Start = "I hope no one decides to cheat today!";

    //Napoleon
    private string Napoleon_DriveBy = "Napoleon drive-by";
    private string Napoleon_DrivenBy = "Napoleon driven by";

    //Nina
    private string Nina_hit = "Nina_hit";

    void Awake()
    {
        ChangeSubtitle();
    }
}
