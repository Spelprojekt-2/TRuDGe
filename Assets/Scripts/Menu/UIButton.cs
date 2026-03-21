using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Video;
using System.Collections;

public class UIButton : MonoBehaviour
{
    private Button button;
    public UIButton SwapUp;
    public UIButton SwapDown;
    public UIButton SwapLeft;
    public UIButton SwapRight;

    public GameObject characterDescription;
    private CharacterStats Stats;

    [Header ("Character Info")]
    [SerializeField] private GameObject[] characterBackgrounds;
    [SerializeField] private GameObject[] characterText;
    [SerializeField] private GameObject[] characterStats;
    [SerializeField] private GameObject[] characterSprites;

    //Trackselect video
    [SerializeField] private Camera[] cams;
    private static VideoPlayer currentVid;
    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Click()
    {
        GetComponent<Button>().onClick?.Invoke();
    }

    public UIButton SwapUpSelection() => SwapUp;
    public UIButton SwapDownSelection() => SwapDown;
    public UIButton SwapRightSelection() => SwapRight;
    public UIButton SwapLeftSelection() => SwapLeft;

    public void SetHighlight(bool state, int playerIndex)
    {
        if (SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectRace || SceneController.instance.currentSceneType == SceneController.SceneType.PlayerSelectTimeTrial)
        {
            Stats = GetComponent<CharacterStats>();
            if (!Stats) return; 
            Stats.SwapCharacterStats(playerIndex);
            if (playerIndex >= 0 && playerIndex < characterBackgrounds.Length)
            {
                characterBackgrounds[playerIndex]?.SetActive(state);
                characterText[playerIndex]?.SetActive(state);
                characterStats[playerIndex]?.SetActive(state);
                characterSprites[playerIndex]?.SetActive(state);
            }
        }
        //Play videos on trackselect
        if (SceneController.instance.currentSceneType == SceneController.SceneType.TrackSelectRace || SceneController.instance.currentSceneType == SceneController.SceneType.TrackSelectTimeTrial)
        {
            if (cams[0] == null || cams[1] == null || cams[2] == null) return;
            string tx = GetComponentInChildren<TextMeshProUGUI>().text;
            switch (tx)
            {
                case "Schlammrennstrecke": 
                    cams[0].depth = 101;
                    cams[1].depth = 100;
                    cams[2].depth = 100; break;
                case "Cliffs of Dover":
                    cams[1].depth = 101;
                    cams[0].depth = 100;
                    cams[2].depth = 100; break;
                case "Luminen TRT":
                    cams[2].depth = 101;
                    cams[0].depth = 100;
                    cams[1].depth = 100; break;
            }
        }
    }
}
