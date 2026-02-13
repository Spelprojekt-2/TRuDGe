using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScreenController : MonoBehaviour
{
    [SerializeField] private GameObject trackSelection;
    public TextMeshProUGUI ReadyTextP1;
    public TextMeshProUGUI ReadyTextP2;
    public TextMeshProUGUI ReadyTextP3;
    public TextMeshProUGUI ReadyTextP4;

    [SerializeField] private UIButton[] preselectPlayer;
    public void OpenTrackSelection()
    {
        trackSelection.SetActive(true);
        CoroutineRunner.Run(SelectObject(trackSelection.transform.GetChild(1).GetComponentInChildren<UIButton>()));
    }

    public void Unready()
    {
        trackSelection.SetActive(false);
        PlayerInputManager.instance.EnableJoining();
        PlayerTrackerManager.instance.UnreadyAll();
    }

    public void LoadTrack(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public UIButton GetStartButton(int index)
    {
        return preselectPlayer[index];
    }
    private IEnumerator SelectObject(UIButton button)
    {
        yield return null;
        if (UISelection.playerSelections.Count > 0) UISelection.playerSelections[0].SwapSelection(button);
    }
}
