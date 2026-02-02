using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class RaceController : MonoBehaviour
{
    public SplineContainer trackSpline;
    private RacerData[] racers;

    void Start()
    {
        racers = FindObjectsByType<RacerData>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (racers.Length == 0 || trackSpline == null)
            return;

        int splineCount = trackSpline.Splines.Count;

        for (int i = 0; i < racers.Length; i++)
        {
            Vector3 racerWorldPos = racers[i].transform.position;
            float3 localPos =
                trackSpline.transform.InverseTransformPoint(racerWorldPos);

            float bestDistance = float.MaxValue;
            float3 bestPoint = float3.zero;
            float bestProgress = 0f;

            for (int j = 0; j < splineCount; j++)
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

                    // Normalize progress across entire container
                    bestProgress = (j + t) / splineCount;
                }
            }

            // World-space point (useful for AI / debug / respawn)
            Vector3 nearestWorldPosition =
                trackSpline.transform.TransformPoint(bestPoint);

            float newLapProgress = bestProgress;

            // Lap wrap detection
            if (newLapProgress < 0.01f && racers[i].lapProgress > 0.99f)
                racers[i].lap++;
            else if (newLapProgress > 0.99f && racers[i].lapProgress < 0.01f)
                racers[i].lap--;

            racers[i].lapProgress = newLapProgress;
            racers[i].raceProgress = racers[i].lap + newLapProgress;
        }
    }
}
