using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleBox;
    public float timer = 0f;
    private bool isShowing = false;
    void Update()
    {
        if (isShowing)
        {
            timer += Time.deltaTime;
            if (timer >= 5f)
            {
                subtitleBox.gameObject.SetActive(false);
                isShowing = false;
                timer = 0f;
            }
        }
    }

    //Voice line subtitles
    //Lars-Göran
    public void Lars_Start()
    {
        subtitleBox.gameObject.SetActive(true);
        subtitleBox.text = "I hope no one decides to cheat today!";
        
        timer = 0f;
        isShowing = true;
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
        Lars_Start();
    }
}
