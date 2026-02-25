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
        Napoleon_Carla_Crash();
    }

    void SpawnSubtitle(string subtext, float duration)
    {
        GameObject newSubtitle =
            Instantiate(subtitlePrefab, subtitleParent, false);

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

        bgRect.anchoredPosition = new Vector2(0, yPos);

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
            r.anchoredPosition = new Vector2(0, yPos);

            yPos += r.sizeDelta.y + verticalSpacing;
        }
    }

    // ======= Subtitles =======

    public void Lars_Start()
    {
        SpawnSubtitle("I hope no one decides to cheat today!", 5f);
    }

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

    public void Nina_Hit()
    {
        SpawnSubtitle("Nina_hit", 5f);
    }

    public void Carla_Win()
    {
        SpawnSubtitle("Carla win", 5f);
    }
}