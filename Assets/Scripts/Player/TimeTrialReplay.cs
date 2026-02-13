using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeTrialReplay : MonoBehaviour
{
    [Header("Replay Settings")]
    public bool isReplaying;

    private List<InputFrame> recordedFrames = new();
    private int playbackIndex = 0;
    private Rigidbody rb;

    // Ghost metadata
    private string ghostName;
    private double totalTime;

    private string FileName => SceneManager.GetActiveScene().name + "_Ghost.ghost";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isReplaying || recordedFrames == null || recordedFrames.Count == 0)
            return;

        if (playbackIndex >= recordedFrames.Count)
        {
            isReplaying = false;
            Debug.Log($"Ghost replay finished: {ghostName} (Time: {totalTime:F2}s)");
            return;
        }

        ApplyMotion(recordedFrames[playbackIndex]);
        VerifyPosition(recordedFrames[playbackIndex]);
        playbackIndex++;
    }

    public void PlayGhost()
    {
        playbackIndex = 0;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForFixedUpdate();
        isReplaying = true;
        Debug.Log($"Playing ghost: {ghostName} (Time: {totalTime:F2}s)");
    }

    public void StopGhost()
    {
        isReplaying = false;
        playbackIndex = 0;
    }

    public void LoadGhostFile(string filePathfileName)
    {
        string json = null;

        if (System.IO.File.Exists(filePathfileName))
            json = System.IO.File.ReadAllText(filePathfileName);
        else
        {
            Debug.LogWarning("No ghost file found for scene " + SceneManager.GetActiveScene().name);
            recordedFrames = new List<InputFrame>();
            ghostName = "Unknown";
            totalTime = 0f;
            return;
        }

        GhostRecording wrapper = JsonUtility.FromJson<GhostRecording>(json);

        recordedFrames = wrapper.frames ?? new List<InputFrame>();
        totalTime = wrapper.time;
        ghostName = string.IsNullOrEmpty(wrapper.name) ? wrapper.name : "You";
    }

    private void ApplyMotion(InputFrame frame)
    {
        rb.linearVelocity = frame.velocity;
        transform.rotation = Quaternion.Euler(frame.rotation);
        transform.position = frame.position;
    }

    private void VerifyPosition(InputFrame frame)
    {
        Vector3 expectedPosition = frame.position;
        Vector3 actualPosition = transform.position;

        if (expectedPosition != actualPosition)
        {
            Debug.LogWarning($"Position mismatch at frame {playbackIndex}: " +
                             $"Expected: {expectedPosition}, Actual: {actualPosition}");
        }
    }
}
