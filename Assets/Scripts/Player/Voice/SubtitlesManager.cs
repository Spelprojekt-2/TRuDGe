using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject subtitlePrefab;
    public Transform subtitleParent;
    public float verticalSpacing = 10f;
    public float bottomOffset = 50f;
    private List<GameObject> activeSubtitles = new List<GameObject>();

    void Awake()
    {
        Announcement_Intro();
    }

    void SpawnSubtitle(string subtext, float duration)
    {
        GameObject newSubtitle =
            Instantiate(subtitlePrefab, subtitleParent, false);
        newSubtitle.SetActive(true);
        TextMeshProUGUI text =
            newSubtitle.GetComponentInChildren<TextMeshProUGUI>();

        text.text = subtext;

        text.ForceMeshUpdate();

        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform bgRect = newSubtitle.GetComponent<RectTransform>();

        float paddingX = 20f;
        float paddingY = 10f;

        bgRect.sizeDelta = new Vector2(
            text.preferredWidth + paddingX,
            text.preferredHeight + paddingY);

        float yPos = bottomOffset;

        foreach (GameObject sub in activeSubtitles)
        {
            RectTransform r = sub.GetComponent<RectTransform>();
            yPos += r.sizeDelta.y + verticalSpacing;
        }

        bgRect.anchoredPosition = new Vector2(0f, yPos);

        activeSubtitles.Add(newSubtitle);

        Destroy(newSubtitle, duration);
        StartCoroutine(RemoveAfterTime(newSubtitle, duration));
    }

    System.Collections.IEnumerator RemoveAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        activeSubtitles.Remove(obj);
        RearrangeSubtitles();
    }

    void RearrangeSubtitles()
    {
        float yPos = bottomOffset;

        foreach (GameObject sub in activeSubtitles)
        {
            if (sub == null) continue;

            RectTransform r = sub.GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2(0f, yPos);

            yPos += r.sizeDelta.y + verticalSpacing;
        }
    }

    //======= Subtitles =======
    //Announcer
    public void Announcement_Intro()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Announcer</color><color=yellow>]</color> In the year 2060 another few races will take place all over Europe in the", 6f);
        Invoke(nameof(Announcement_Intro1), 5f);
    }
    void Announcement_Intro1()
    {                  
        SpawnSubtitle("beautiful sport tank rally. Schlammrennstrecke and the Cliffs of Dover are just two", 7f);
        Invoke(nameof(Announcement_Intro2), 6f);
    }
    void Announcement_Intro2()
    {
        SpawnSubtitle("of the exciting, revamped tracks we have this year. 2060 also marks an important", 7f);
        Invoke(nameof(Announcement_Intro3), 6f);
    }
    void Announcement_Intro3()
    {
        SpawnSubtitle("milestone in our world's healing process, a decade has passed since the third world", 7f);
        Invoke(nameof(Announcement_Intro4), 6f);
    }
    void Announcement_Intro4()
    {
        SpawnSubtitle("war ended. Together, we shall continue to work toward a happier world, a world with", 7f);
        Invoke(nameof(Announcement_Intro5), 6f);
    }
    void Announcement_Intro5()
    {
        SpawnSubtitle("<color=red>T</color><color=orange>A</color><color=green>N</color><color=yellow>K</color> <color=red>R</color><color=orange>A</color><color=green>L</color><color=yellow>L</color><color=red>Y</color><color=orange>!</color>", 5f);
    }

    //Lars-Göran
    public void Lars_Start()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Lars-Göran</color><color=yellow>]</color> I hope no one decides to cheat today!", 5f);
    }

    //Napoleon
    public void Napoleon_DriveBy()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Napoleon</color><color=yellow>]</color> Napoleon drive-by", 5f);
    }
    public void Napoleon_DrivenBy()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Napoleon</color><color=yellow>]</color> Napoleon driven by", 5f);
    }
    public void Napoleon_Carla_Crash()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Carla</color><color=yellow>]</color> Go away!", 5f);
        Invoke(nameof(NapoleonReply_Carla_Crash), 3f);
    }
    void NapoleonReply_Carla_Crash()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Napoleon</color><color=yellow>]</color> No u.", 3f);
    }

    //Nina, Brass beast
    public void Nina_Hit()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Brass beast</color><color=yellow>]</color> Nina_hit", 5f);
    }

    //Carla, Capôw
    public void Carla_Win()
    {
        SpawnSubtitle("<color=yellow>[</color><color=blue>Capôw</color><color=yellow>]</color> Carla win", 5f);
    }

    //Leonie, Schlammer

    //André, Dragoș

    //Ragana, Vilkmérgele demon

    //Tristano, Harlequini Martinellini
}