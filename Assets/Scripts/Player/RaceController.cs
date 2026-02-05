using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RaceController : MonoBehaviour
{
    public SplineContainer trackSpline;
    [SerializeField, Range(1, 5)] int lapsOnThisTrack = 3;
    private List<RacerData> racers;

    [SerializeField] private Transform startingLine;
    private float startLineOffset;

    [SerializeField] private float timeBeforeStartCountdown;
    [SerializeField] private TextMeshProUGUI countdownText;
    private float timeToRaceStart;
    private bool raceStarted;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SummaryScene;
        timeToRaceStart = timeBeforeStartCountdown;
        raceStarted = false;

        racers = FindObjectsByType<RacerData>(FindObjectsSortMode.None).ToList();
        startLineOffset = GetSplineProgress(startingLine.position);

        for (int i = 0; i < racers.Count; i++)
        {
            UpdateRaceProgress(racers[i]);
            racers[i].TrackLoaded(lapsOnThisTrack);
        }
    }

    private void Update()
    {
        if (!raceStarted)
        {
            timeToRaceStart -= Time.deltaTime;
            if (timeToRaceStart < 3) countdownText.text = Mathf.FloorToInt(timeToRaceStart + 1).ToString();
            if (timeToRaceStart < 0)
            {
                raceStarted = true;
                for (int i = 0; i < racers.Count; i++)
                {
                    Debug.Log("Race Started");
                    racers[i].OnRaceStarted();
                    countdownText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            bool allDone = true;
            for (int i = 0; i < racers.Count; i++)
            {
                if (racers[i].bestLap < lapsOnThisTrack) allDone = false;
            }
            if (allDone)
            {
                SceneManager.LoadScene("AfterRace");
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
            }
        }

    }


    public void SummaryScene(Scene scene, LoadSceneMode loadmode)
    {
        string leaderboard = "";
        RacerData[] racersInOrder = racers.ToList().OrderByDescending(x => x.raceProgress).ToArray();
        for (int i = 0; i < racersInOrder.Length; ++i)
        {
            racersInOrder[i].DisablePosition();
            leaderboard += $"{i+1}: Player{racersInOrder[i].GetComponent<PlayerInput>().playerIndex + 1}";
        }

        FindFirstObjectByType<TextMeshProUGUI>().text = leaderboard;
        Debug.Log(scene.name);
        Destroy(this);
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


    float GetSplineProgress(Vector3 worldPosition)
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
                out float t
            );

            float dist = math.lengthsq(pointOnSpline - localPos);
            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestProgress = (j + t) / trackSpline.Splines.Count;
            }
        }

        return bestProgress; // 0–1 across entire container
    }

}