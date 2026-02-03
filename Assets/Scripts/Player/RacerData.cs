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

    public int bestLap;
    [SerializeField] TextMeshProUGUI lapCountText;
    [SerializeField] TextMeshProUGUI positionText;
    [SerializeField] private UnityEvent OnFinished;
    [SerializeField] private UnityEvent OnNewLap;

    public void TrackLoaded(int lapsOnTrack)
    {
        trackLaps = lapsOnTrack;
        if (lapProgress > 0.5f) lap = -1;
    }
    public void NextLap()
    {
        if (bestLap > lap)
        {
            lap = bestLap;
        }
        else
        {
            lap++;
            bestLap++;
            OnNewLap?.Invoke();
            lapCountText.text = $"Lap: {lap+1}/{trackLaps}";
        }
    }
    public void RaceFinished()
    {
        OnFinished?.Invoke();
    }
    public void BackwardsLap()
    {
        if (lap == bestLap)
        {
            lap--;
        }
        else {
            lap = bestLap - 1;
        }

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
}