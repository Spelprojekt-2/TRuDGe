using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System.Linq;

public class RaceController : MonoBehaviour
{
    public SplineContainer trackSpline;
    [SerializeField, Range(1, 5)] int lapsOnThisTrack = 3;
    private List<RacerData> racers;

    void Start()
    {
        racers = FindObjectsByType<RacerData>(FindObjectsSortMode.None).ToList();
        for (int i = 0; i < racers.Count; i++)
        {
            UpdateRaceProgress(racers[i]);
            racers[i].TrackLoaded(lapsOnThisTrack);
        }
    }

    private void Update()
    {
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

    void UpdateRaceProgress(RacerData racer)
    {
        {
            if (racer.lap >= lapsOnThisTrack)
            {
                racer.raceProgress = 1000 - racer.racePosition;
                return;
            }
            Vector3 racerWorldPos = racer.transform.position;
            float3 localPos =
                trackSpline.transform.InverseTransformPoint(racerWorldPos);

            float bestDistance = float.MaxValue;
            float3 bestPoint = float3.zero;
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
                    bestPoint = pointOnSpline;
                    bestProgress = (j + t) / trackSpline.Splines.Count;
                }
            }

            // Progress point (useful for AI / respawn)
            Vector3 nearestWorldPosition =
                trackSpline.transform.TransformPoint(bestPoint);

            float newLapProgress = bestProgress;

            // Lap wrap detection
            if (newLapProgress < 0.01f && racer.lapProgress > 0.99f)
            {
                racer.NextLap();
                if (racer.lap == lapsOnThisTrack)
                {
                    racer.RaceFinished();
                    racer.lapProgress = 0.5f;
                    racer.raceProgress = 1000 - racer.racePosition;
                }
            }
            else if (newLapProgress > 0.99f && racer.lapProgress < 0.01f)
                racer.BackwardsLap();

            racer.lapProgress = newLapProgress;
            racer.raceProgress = racer.lap + newLapProgress;
        }
    }
}
