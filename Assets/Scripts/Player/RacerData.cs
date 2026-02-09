using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RacerData : MonoBehaviour
{
    public float lapProgress;
    public int lap;
    public float raceProgress;
    public int racePosition;
    private int trackLaps;
    public string racername;
    private RaceController raceController;

    public int currentValidLap;
    [SerializeField] TextMeshProUGUI lapCountText;
    [SerializeField] TextMeshProUGUI positionText;
    [SerializeField] private UnityEvent OnRaceFinish;
    [SerializeField] private UnityEvent OnRaceSceneStarted;
    [SerializeField] private UnityEvent OnRaceStart;
    [SerializeField] private UnityEvent OnNewLap;
    private List<double> lapEndTimes = new List<double>();


    public void TrackLoaded(int lapsOnTrack)
    {
        lapEndTimes.Clear();
        raceController = FindFirstObjectByType<RaceController>();
        trackLaps = lapsOnTrack;
        if (lapProgress > 0.5f) lap = -1;
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
        OnRaceStart?.Invoke();
    }
    public void OnRaceFinished()
    {
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
        positionText.text = GetPosString();
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