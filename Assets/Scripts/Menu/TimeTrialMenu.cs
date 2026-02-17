using TMPro;
using UnityEngine;

public class TimeTrialMenu : MonoBehaviour
{
    [SerializeField] private GameObject timeTrial;
    [SerializeField] private TextMeshProUGUI track1MyTime;
    [SerializeField] private TextMeshProUGUI track2MyTime;
    [SerializeField] private TextMeshProUGUI track3MyTime;
    [SerializeField] private TextMeshProUGUI track1GhostTime;
    [SerializeField] private TextMeshProUGUI track2GhostTime;
    [SerializeField] private TextMeshProUGUI track3GhostTime;
    void Start()
    {
        if (!PlayerTrackerManager.instance.isTimeTrial)
        {
            timeTrial.SetActive(false);
            return;
        }

    }
}
