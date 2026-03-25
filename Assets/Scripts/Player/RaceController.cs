using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Random = UnityEngine.Random;

public class RaceController : MonoBehaviour
{
    public SplineContainer trackSpline;
    [SerializeField, Range(1, 5)] int lapsOnThisTrack = 3;
    [HideInInspector] public List<RacerData> racers;

    [SerializeField] private Transform startingLine;
    private float startLineOffset;

    [SerializeField] private float timeBeforeStartCountdown;
    private float timeToRaceStart;
    private bool raceStarted;
    private Coroutine finishPlayersRoutine;
    private Coroutine allDoneRoutine;
    [SerializeField] private float SwapSceneAfterRaceTime = 5;
    [SerializeField] private float FinishLastRacerTimer = 30;

    //Timer
    private double raceStartTime;
    private bool isPaused = false;
    private double totalPausedTime = 0;
    private double pauseStartTime = 0;
    private int lastCountdownSecond = -1;
    private float timeForVO;
    private float VODelay = 5f;

    [SerializeField] private GameObject ghostPrefab;
    private TimeTrialReplay ghostReplay;

    void Awake()
    {
        timeToRaceStart = timeBeforeStartCountdown;
        raceStarted = false;

        racers = FindObjectsByType<RacerData>(FindObjectsSortMode.None).ToList();
        if (trackSpline) startLineOffset = GetSplineProgress(startingLine.position);

        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].TrackLoaded();
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

    void Start()
    {
        timeForVO = Time.time + VODelay;
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
                    for (int i = 0; i < racers.Count; i++)
                    {
                        CoroutineRunner.Run(racers[i].CountdownText(currentSecond.ToString(), currentSecond == 1));
                    }
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
                    racers[i].OnRaceStarted();
                    if (RacingInformation.instance.isTimeTrialWithGhost) ghostReplay.PlayGhost();
                    if (countdownText) countdownText.gameObject.SetActive(false);
                    timeForVO = Time.time; // Voice lines time reset
                }
            }
        }
        else
        {
            bool allDone = true;
            int racersDone = racers.Count;
            for (int i = 0; i < racers.Count; i++)
            {
                if (racers[i].currentValidLap < lapsOnThisTrack)
                {
                    allDone = false;
                    racersDone--;
                }
            }
            if (racersDone == racers.Count - 1 && racers.Count > 1 && finishPlayersRoutine == null)
            {
                finishPlayersRoutine = StartCoroutine(FinishPlayerAfterSec(FinishLastRacerTimer));
            }
            else if (allDone && allDoneRoutine == null)
            {
                RacerData[] inorder = racers.ToList().OrderByDescending(x => x.raceProgress).ToArray();
                Leaderboard.SetLeaderboard(inorder);
                
                if (finishPlayersRoutine != null)
                {
                    StopCoroutine(finishPlayersRoutine);
                    finishPlayersRoutine = null;
                }
                allDoneRoutine = StartCoroutine(WaitToAfterRace(SwapSceneAfterRaceTime));
            }
        }
        if (racers.Count == 0 || trackSpline == null) return;

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
                // Play Voice line
                if (Time.time > timeForVO)
                {
                    timeForVO = Time.time + VODelay;
                    int r = Random.Range(0, 2);
                    switch (r)
                {
                    case 0: AudioManager.Instance.PlayFirstVO(racersInOrder[0].racername); break;
                    case 1: AudioManager.Instance.PlayFirstANN(racersInOrder[0].racername); break;
                    //default: AudioManager.Instance.PlayMetalPipe(); break;
                }
                }
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
        else if (Mathf.Abs(newLapProgress - racer.lapProgress) > 0.15f) return;

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

    private IEnumerator FinishPlayerAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec-5);
        RacerData lastRacer = racers.FirstOrDefault(r => r.lap < lapsOnThisTrack);
        for (int i = 5; i > 0; i--)
        {
            CoroutineRunner.Run(lastRacer.CountdownText(i.ToString(), i == 1));
            yield return new WaitForSeconds(1);
        }
        if (lastRacer != null)
        {
            lastRacer.OnRaceFinished();
            lastRacer.lapProgress = 0.5f;
            lastRacer.raceProgress = 1000 - lastRacer.racePosition;
        }
        finishPlayersRoutine = null;
        RacerData[] inorder = racers.ToList().OrderByDescending(x => x.raceProgress).ToArray();
        Leaderboard.SetLeaderboard(inorder);

        if (allDoneRoutine != null) yield break;
        allDoneRoutine = StartCoroutine(WaitToAfterRace(SwapSceneAfterRaceTime));
    }

    private IEnumerator WaitToAfterRace(float sec)
    {
        yield return new WaitForSeconds(sec);
        allDoneRoutine = null;
        if (RacingInformation.instance.isTimeTrial) SceneManager.LoadScene("TrackSelectTimeTrial");
        else SceneManager.LoadScene("AfterRace");
    }

    public double GetRaceTime()
    {
        if (!raceStarted) return 0;
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