using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Linq;

public class HomingMissile : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 80f;
    public float lockOnRange = 30f;
    public float hitRange = 3f;

    private SplineContainer track;
    private Transform targetPlayer;
    private float currentProgress; // Back to 0-1 for compatibility
    private bool isHoming = false;
    private GameObject shooter;

    public void Initialize(SplineContainer splineTrack, float startProgress, GameObject shooter)
    {
        this.track = splineTrack;
        this.currentProgress = startProgress;
        this.shooter = shooter;

        UpdatePositionOnTrack();
    }

    void Update()
    {
        if (track == null) return;

        if (!isHoming)
        {
            // 1. Manually calculate how much 0-1 progress to add based on speed
            float totalLength = track.CalculateLength();
            float progressToAdd = (speed * Time.deltaTime) / totalLength;
            currentProgress = (currentProgress + progressToAdd) % 1f;

            UpdatePositionOnTrack();
            CheckForTargets();
        }
        else
        {
            HomeToPlayer();
        }
    }

    void UpdatePositionOnTrack()
    {
        // 2. Use EvaluatePosition (Supported in all Unity versions)
        // This returns local coordinates relative to the child object (500, 0, 800)
        float3 localPos = track.EvaluatePosition(currentProgress);
        float3 localTangent = track.EvaluateTangent(currentProgress);

        // 3. Convert to World Space using the child's transform
        transform.position = track.transform.TransformPoint(localPos);

        Vector3 worldDir = track.transform.TransformDirection(localTangent);
        if (worldDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(worldDir);
    }

    void CheckForTargets()
    {
        // Find the RaceController to get the list of racers
        RaceController rc = FindFirstObjectByType<RaceController>();
        if (rc == null) return;

        foreach (var racer in rc.racers)
        {
            if (racer.gameObject == shooter) continue;

            float dist = Vector3.Distance(transform.position, racer.transform.position);
            if (dist < lockOnRange)
            {
                targetPlayer = racer.transform;
                isHoming = true;
                break;
            }
        }
    }

    void HomeToPlayer()
    {
        if (targetPlayer == null) { isHoming = false; return; }

        Vector3 dir = (targetPlayer.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPlayer.position) < hitRange)
        {
            // Trigger your explosion VFX here
            Destroy(gameObject);
        }
    }
}