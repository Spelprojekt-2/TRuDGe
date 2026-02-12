using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class RacerData : MonoBehaviour
{
    public int index;
    public float lapProgress;
    public int lap;
    public float raceProgress;
    public int racePosition;
    private int trackLaps;
    public string racername;
    private RaceController raceController;

    private bool isRacing;
    public int currentValidLap;
    [SerializeField] private TextMeshProUGUI lapCountText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private GameObject TimeTrialUI;
    [SerializeField] private GameObject RaceUI;
    [SerializeField] private UnityEvent OnRaceFinish;
    [SerializeField] private UnityEvent OnRaceSceneStarted;
    [SerializeField] private UnityEvent OnRaceStart;
    [SerializeField] private UnityEvent OnNewLap;
    private List<double> lapEndTimes = new List<double>();


    private void Update()
    {
        if (!PlayerTrackerManager.instance.isTimeTrial || !isRacing) return;
        TimerText.text = Leaderboard.FormatTime(raceController.GetRaceTime());
    }

    public void TrackLoaded(int lapsOnTrack)
    {
        lapEndTimes.Clear();
        raceController = FindFirstObjectByType<RaceController>();
        trackLaps = lapsOnTrack;
        if (lapProgress > 0.5f) lap = -1;
        TimeTrialUI.SetActive(PlayerTrackerManager.instance.isTimeTrial);
        RaceUI.SetActive(!PlayerTrackerManager.instance.isTimeTrial);
        TimerText.text = "00:00.000";
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
        lapCountText.text = $"Lap: {lap + 1}/{trackLaps}";
    }

    public void OnRacetrackScene()
    {
        OnRaceSceneStarted?.Invoke();
        positionText.gameObject.SetActive(true);
    }
    public void OnRaceStarted()
    {
        isRacing = true;
        OnRaceStart?.Invoke();
    }
    public void OnRaceFinished()
    {
        isRacing = false;
        OnRaceFinish?.Invoke();
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
        if (positionText != null) positionText.gameObject.SetActive(false);
    }
    public void UpdatePosition(int pos)
    {
        racePosition = pos;
        if (!PlayerTrackerManager.instance.isTimeTrial) positionText.text = GetPosString();
    }

    private string GetPosString()
    {
        switch (racePosition)
        {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return racePosition + "th";
        }
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