using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionUIList : MonoBehaviour
{
    [SerializeField] private GameObject trackSelection;
    public TextMeshProUGUI ReadyTextP1;
    public TextMeshProUGUI ReadyTextP2;
    public TextMeshProUGUI ReadyTextP3;
    public TextMeshProUGUI ReadyTextP4;

    public void OpenTrackSelection()
    {
        trackSelection.SetActive(true);
        EventSystem.current.SetSelectedGameObject(trackSelection.transform.GetComponentInChildren<Button>().gameObject);
    }
    public void Unready()
    {
        trackSelection.SetActive(false);
        FindFirstObjectByType<PlayerTrackerManager>().UnreadyAll();
    }
}
