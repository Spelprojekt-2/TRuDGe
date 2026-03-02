using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RacerData : MonoBehaviour
{
    [SerializeField] private Sprite[] positionSprites;
    [SerializeField] private Sprite[] lapCountSprites;

    public int index;
    public float lapProgress;
    public int lap;
    public float raceProgress;
    public int racePosition;
    public string racername;
    public bool isReplayGhost;
    private RaceController raceController;

    private bool isRacing;
    public int currentValidLap;
    [SerializeField] private Image lapCountImage;
    [SerializeField] private Image positionImage;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject TimeTrialUI;
    [SerializeField] private GameObject RaceUI;
    [SerializeField] private UnityEvent OnRaceFinish;
    [SerializeField] private UnityEvent OnRaceSceneStarted;
    [SerializeField] private UnityEvent OnRaceStart;
    [SerializeField] private UnityEvent OnNewLap;
    private List<double> lapEndTimes = new List<double>();
    [SerializeField] private TimeTrialCapture capture;

    private void Update()
    {
        if (!RacingInformation.instance.isTimeTrial || !isRacing || isReplayGhost) return;
        TimerText.text = Leaderboard.FormatTime(raceController.GetRaceTime());
    }

    public void TrackLoaded(int lapsOnTrack)
    {
        currentValidLap = 0;
        lap = 0;
        raceProgress = 0;
        lapProgress = 0;
        racePosition = 0;

        lapEndTimes.Clear();
        raceController = FindFirstObjectByType<RaceController>();
        if (lapProgress > 0.5f) lap = -1;
        if (isReplayGhost) return;
        TimeTrialUI.SetActive(PlayerTrackerManager.instance.isTimeTrial);
        RaceUI.SetActive(!PlayerTrackerManager.instance.isTimeTrial);
        TimerText.text = "00:00.000";
        if (index == 0) GetComponentInChildren<PlayerCamera>()?.MinimapPrep();
        capture = GetComponent<TimeTrialCapture>();
        positionImage.gameObject.SetActive(true);
        OnRaceSceneStarted?.Invoke();
    }
    public void NextLap()
    {
        if (currentValidLap > lap)
        {
            lap = currentValidLap;
        }
        else
        {
            lapEndTimes.Add(raceController.GetRaceTime());
            lap++;
            currentValidLap++;
            OnNewLap?.Invoke();
            UpdateLapCount();
        }
    }

    public void UpdateLapCount()
    {
        if (isReplayGhost || lap >= lapCountSprites.Length) return;
        lapCountImage.sprite = lapCountSprites[lap];
    }

    public void OnRaceStarted()
    {
        currentValidLap = 0;
        lap = 0;
        raceProgress = 0;
        lapProgress = 0;
        racePosition = 0;

        isRacing = true;
        OnRaceStart?.Invoke();
        if(RacingInformation.instance.isTimeTrial && !isReplayGhost) capture.StartCapture();
    }
    public void OnRaceFinished()
    {
        isRacing = false;
        OnRaceFinish?.Invoke();
        if (RacingInformation.instance.isTimeTrial && !isReplayGhost) capture.StopCapture();
    }

    public void BackwardsLap()
    {
        if (lap == currentValidLap)
        {
            lap--;
        }
        else {
            lap = currentValidLap - 1;
        }

    }

    public void DisablePosition()
    {
        if (positionImage != null) positionImage.gameObject.SetActive(false);
    }
    public void UpdatePosition(int pos)
    {
        racePosition = pos;
        if (!RacingInformation.instance.isTimeTrial) positionImage.sprite = positionSprites[pos - 1];
    }

    public void SetName(string newName)
    {
        racername = newName;
    }

    public double[] GetLapTimes()
    {
        double[] lapTimes = new double[lapEndTimes.Count];
        for (int i = 0; i < lapEndTimes.Count; i++)
        {
            if (i == 0) lapTimes[i] = lapEndTimes[i];
            else lapTimes[i] = lapEndTimes[i] - lapEndTimes[i - 1]; ;
        }
        return lapTimes;
    }
    public double GetRaceTime()
    {
        return lapEndTimes[^1];
    }
}