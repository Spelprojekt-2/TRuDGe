using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionUIList : MonoBehaviour
{
    private bool ignoreNextSubmit;
    [SerializeField] private GameObject trackSelection;
    public TextMeshProUGUI ReadyTextP1;
    public TextMeshProUGUI ReadyTextP2;
    public TextMeshProUGUI ReadyTextP3;
    public TextMeshProUGUI ReadyTextP4;

    public void OpenTrackSelection()
    {
        ignoreNextSubmit = true;
        trackSelection.SetActive(true);
        EventSystem.current.SetSelectedGameObject(trackSelection.transform.GetComponentInChildren<Button>().gameObject);
    }
    public void Unready()
    {
        trackSelection.SetActive(false);
        PlayerTrackerManager.instance.UnreadyAll();
    }

    public void LoadTrack(string sceneName)
    {
        if (ignoreNextSubmit) 
        { 
            ignoreNextSubmit = false;
            return; 
        }
        SceneManager.LoadScene(sceneName);
    }
}
