using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class RaceController : MonoBehaviour
{
    public SplineContainer trackSpline;
    [SerializeField, Range(1, 5)] int lapsOnThisTrack = 3;
    [HideInInspector] public List<RacerData> racers;

    [SerializeField] private Transform startingLine;
    private float startLineOffset;

    [SerializeField] private float timeBeforeStartCountdown;
    [SerializeField] private TextMeshProUGUI countdownText;
    private float timeToRaceStart;
    private bool raceStarted;

    //Timer
    private double raceStartTime;
    private bool isPaused = false;
    private double totalPausedTime = 0;
    private double pauseStartTime = 0;
    private int lastCountdownSecond = -1;

    [SerializeField] private GameObject ghostPrefab;
    private TimeTrialReplay ghostReplay;

    void Start()
    {
        timeToRaceStart = timeBeforeStartCountdown;
        raceStarted = false;

        racers = FindObjectsByType<RacerData>(FindObjectsSortMode.None).ToList();
        if (trackSpline) startLineOffset = GetSplineProgress(startingLine.position);

        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].currentValidLap = 0;
            racers[i].lap = 0;
            racers[i].raceProgress = 0;
            racers[i].lapProgress = 0;
            racers[i].racePosition = 0;
            racers[i].TrackLoaded(lapsOnThisTrack);
            racers[i].UpdateLapCount();
            if (trackSpline) UpdateRaceProgress(racers[i]);
        }

        if (!trackSpline) return;

        if (RacingInformation.instance.isTimeTrial && RacingInformation.instance.isTimeTrialWithGhost)
        {
            SpawnPointVisualizer spawn = FindObjectsByType<SpawnPointVisualizer>(FindObjectsSortMode.None)
    .OrderBy(s => s.name)
    .ToArray()[0];

            spawn.transform.GetPositionAndRotation(out Vector3 spawnPos, out Quaternion spawnRot);
            GameObject ghostObj = Instantiate(ghostPrefab, spawnPos, spawnRot);
            ghostReplay = ghostObj.GetComponentInChildren<TimeTrialReplay>();
            ghostReplay.LoadGhostFile(RacingInformation.instance.pathToGhost);

        }
    }

    private void FixedUpdate()
    {
        if (!raceStarted)
        {
            timeToRaceStart -= Time.fixedDeltaTime;
            if (timeToRaceStart <= 3 && timeToRaceStart > 0)
            {
                int currentSecond = Mathf.FloorToInt(timeToRaceStart + 1);

                if (currentSecond != lastCountdownSecond)
                {
                    lastCountdownSecond = currentSecond;
                    countdownText.text = currentSecond.ToString();
                    AudioManager.Instance.PlayCountdownAudio(); // Play countdown audio
                }
            }
            if (timeToRaceStart <= 0)
            {
                AudioManager.Instance.PlayCountdownAudio(true); // Play final countdown audio

                totalPausedTime = 0;
                raceStartTime = Time.realtimeSinceStartupAsDouble;
                raceStarted = true;
                for (int i = 0; i < racers.Count; i++)
                {
                    Debug.Log("Race Started");
                    racers[i].OnRaceStarted();
                    if (RacingInformation.instance.isTimeTrialWithGhost) ghostReplay.PlayGhost();
                    if (countdownText) countdownText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            bool allDone = true;
            for (int i = 0; i < racers.Count; i++)
            {
                if (racers[i].currentValidLap < lapsOnThisTrack) allDone = false;
            }
            if (allDone)
            {
                RacerData[] inorder = racers.ToList().OrderByDescending(x => x.raceProgress).ToArray();
                Leaderboard.SetLeaderboard(inorder);

                StartCoroutine(WaitToAfterRace());
            }
        }
        if (racers.Count == 0 || trackSpline == null) return;

        if (!trackSpline) return;

        for (int i = 0; i < racers.Count; i++)
        {
            UpdateRaceProgress(racers[i]);
        }

        RacerData[] racersInOrder = racers.ToList().OrderByDescending(x => x.raceProgress).ToArray();
        for (int i = 0; i < racersInOrder.Length; i++)
        {
            if (racersInOrder[i].racePosition != i + 1)
            {
                racersInOrder[i].UpdatePosition(i + 1);
            }
        }

    }

    void UpdateRaceProgress(RacerData racer)
    {
        if (racer.lap >= lapsOnThisTrack)
        {
            racer.raceProgress = 1000 - racer.racePosition;
            return;
        }

        float rawProgress = GetSplineProgress(racer.transform.position);

        rawProgress -= startLineOffset;
        if (rawProgress < 0f) rawProgress += 1f;
        float newLapProgress = rawProgress;

        // Lap wrap detection
        if (newLapProgress < 0.01f && racer.lapProgress > 0.99f)
        {
            racer.NextLap();

            if (racer.lap == lapsOnThisTrack)
            {
                racer.OnRaceFinished();
                racer.lapProgress = 0.5f;
                racer.raceProgress = 1000 - racer.racePosition;
                return;
            }
        }
        else if (newLapProgress > 0.99f && racer.lapProgress < 0.01f)
        {
            racer.BackwardsLap();
        }

        racer.lapProgress = newLapProgress;
        racer.raceProgress = racer.lap + newLapProgress;
    }


    public float GetSplineProgress(Vector3 worldPosition)
    {
        float3 localPos = trackSpline.transform.InverseTransformPoint(worldPosition);

        float bestDistance = float.MaxValue;
        float bestProgress = 0f;

        for (int j = 0; j < trackSpline.Splines.Count; j++)
        {
            Spline spline = trackSpline.Splines[j];

            SplineUtility.GetNearestPoint(
                spline,
                localPos,
                out float3 pointOnSpline,
                out float splineProgress
            );

            float dist = math.lengthsq(pointOnSpline - localPos);

            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestProgress = splineProgress;
            }
        }

        return bestProgress;
    }

    private IEnumerator WaitToAfterRace()
    {
        yield return new WaitForSeconds(5);
        if (RacingInformation.instance.isTimeTrial) SceneManager.LoadScene("TrackSelectTimeTrial");
        else SceneManager.LoadScene("AfterRace");
    }

    public double GetRaceTime()
    {
        if (isPaused) return pauseStartTime - raceStartTime - totalPausedTime;
        else return Time.realtimeSinceStartupAsDouble - raceStartTime - totalPausedTime;
    }

    public void PauseRace()
    {
        if (isPaused) return;
        pauseStartTime = Time.realtimeSinceStartupAsDouble;
        isPaused = true;
    }

    public void ResumeRace()
    {
        if (!isPaused) return;
        totalPausedTime += Time.realtimeSinceStartupAsDouble - pauseStartTime;
        isPaused = false;
    }
}