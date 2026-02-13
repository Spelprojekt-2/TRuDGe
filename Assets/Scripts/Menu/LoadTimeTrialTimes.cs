using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTimeTrialTimes : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private string level1SceneName;
    [SerializeField] private TextMeshProUGUI level1Text;
    [SerializeField] private string level2SceneName;
    [SerializeField] private TextMeshProUGUI level2Text;
    [SerializeField] private string level3SceneName;
    [SerializeField] private TextMeshProUGUI level3Text;

    [SerializeField] private GameObject GhostSelectionMenu;
    [SerializeField] private UIButton GhostSelectionMenuFirstSelection;
    [SerializeField] private TextMeshProUGUI ghostTrackName;
    private UIButton lastSelection;
    void Start()
    {
        DisplayLevelTimes(level1SceneName, level1Text);
        DisplayLevelTimes(level2SceneName, level2Text);
        DisplayLevelTimes(level3SceneName, level3Text);
    }

    private void DisplayLevelTimes(string sceneName, TextMeshProUGUI textField)
    {
        GhostRecording official = LoadOfficialGhostFile(sceneName);
        GhostRecording personal = LoadPlayerGhostFile(sceneName);

        string officialTime = "N/A";
        string officialAuthor = "Unknown";
        string personalTime = "N/A";

        if (official != null)
        {
            officialTime = Leaderboard.FormatTime(official.time);
            officialAuthor = string.IsNullOrEmpty(official.name) ? "Unknown" : official.name;
        }

        if (personal != null)
            personalTime = Leaderboard.FormatTime(personal.time);

        textField.text = $"Gold: {officialTime}\nby: {officialAuthor}\n\nYour Time: {personalTime}";
    }

    /// <summary>
    /// Loads the official ghost stored in StreamingAssets
    /// </summary>
    private GhostRecording LoadOfficialGhostFile(string sceneName)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, sceneName + "_Ghost.ghost");

        if (!System.IO.File.Exists(path))
        {
            Debug.LogWarning("No official ghost found for scene " + sceneName);
            return null;
        }

        string json = System.IO.File.ReadAllText(path);
        GhostRecording wrapper = JsonUtility.FromJson<GhostRecording>(json);
        return wrapper;
    }

    /// <summary>
    /// Loads the player's personal ghost stored in PersistentDataPath
    /// </summary>
    private GhostRecording LoadPlayerGhostFile(string sceneName)
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, sceneName + "_Ghost.ghost");

        if (!System.IO.File.Exists(path))
        {
            Debug.Log("No personal ghost found for scene " + sceneName);
            return null;
        }

        string json = System.IO.File.ReadAllText(path);
        GhostRecording wrapper = JsonUtility.FromJson<GhostRecording>(json);
        return wrapper;
    }

    public void OpenSelectionMenu(string level)
    {
        ghostTrackName.text = level;
        lastSelection = UISelection.playerSelections[0].selection;
        GhostSelectionMenu.SetActive(true);
        CoroutineRunner.Run(SelectObject(GhostSelectionMenuFirstSelection));
    }

    public void CloseSelectionMenu()
    {
        GhostSelectionMenu.SetActive(false);
        CoroutineRunner.Run(SelectObject(lastSelection));
    }

    private IEnumerator SelectObject(UIButton button)
    {
        while (!button.gameObject.activeInHierarchy)
            yield return null;
        UISelection.playerSelections[0].SwapSelection(button);
    }
}
