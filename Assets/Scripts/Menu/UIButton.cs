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
            VideoPlayer newVid = GetComponent<VideoPlayer>();
            if (newVid == null) return;

            float time = Random.Range(0, (float)newVid.length - 10);
            newVid.time = time;
            newVid.Play();
            StartCoroutine(Transition(newVid));
        }
    }
    private IEnumerator Transition(VideoPlayer newVid)
    {
        yield return null;
        if (currentVid != null && currentVid != newVid)
        {
            //yield return new WaitForSeconds(0.35f);
            currentVid.Stop();
        }
        currentVid = newVid;
    }
}
