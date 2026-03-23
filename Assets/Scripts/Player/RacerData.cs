using System.Collections;
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
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image lapCountImage;
    [SerializeField] private Image lapCountPopup;
    [SerializeField] private TextMeshProUGUI finishText;
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
        if (!RacingInformation.instance.isTimeTrial || SceneController.instance.IsMenu || isReplayGhost || raceController == null || !isRacing) return;
        TimerText.text = Leaderboard.FormatTime(raceController.GetRaceTime());
    }

    public void TrackLoaded()
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
        bool isOnTrainingGround = (SceneController.instance.currentSceneType == SceneController.SceneType.TrainingGround || SceneController.instance.currentSceneType == SceneController.SceneType.STrainingGround);
        
        TimeTrialUI.SetActive(RacingInformation.instance.isTimeTrial && !isOnTrainingGround);
        RaceUI.SetActive(!RacingInformation.instance.isTimeTrial && !isOnTrainingGround);
        positionImage.gameObject.SetActive(!isOnTrainingGround);
        TimerText.text = "00:00.000";
        
        if (index == 0) GetComponentInChildren<PlayerCamera>()?.MinimapPrep();
        capture = GetComponent<TimeTrialCapture>();
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
        lapCountPopup.sprite = lapCountSprites[lap];
        if (lap > 0) CoroutineRunner.Run(DisplayTemporary(lapCountPopup.gameObject));
    }

    public void OnRaceStarted()
    {
        currentValidLap = 0;
        lap = 0;
        raceProgress = 0;
        lapProgress = 0;
        racePosition = 0;
        transform.root.GetComponentInChildren<PlayerShooting>().timer = 0;
        transform.root.GetComponentInChildren<PlayerShooting>().shootCooldown.fillAmount = 0;
        isRacing = true;
        OnRaceStart?.Invoke();
        if (RacingInformation.instance.isTimeTrial && !isReplayGhost) capture.StartCapture();

    }
    public void OnRaceFinished()
    {
        isRacing = false;
        OnRaceFinish?.Invoke();
        if (RacingInformation.instance.isTimeTrial && !isReplayGhost) capture.StopCapture();
        CoroutineRunner.Run(DisplayTemporary(finishText.gameObject));
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

    public IEnumerator CountdownText(string text, bool hideAfter)
    {
        if (!countdownText.enabled) countdownText.enabled = true;
        countdownText.text = text;
        if (hideAfter)
        {
            yield return new WaitForSeconds(1);
            countdownText.enabled = false;
        }
    }

    private IEnumerator DisplayTemporary(GameObject g)
    {
        g.SetActive(true);
        yield return new WaitForSeconds(2f);
        g.SetActive(false);
    }
}