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
    private string FileName => SceneManager.GetActiveScene().name + "_Ghost.ghost";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LoadGhostFile();
    }

    private void FixedUpdate()
    {
        if (!isReplaying || recordedFrames == null || recordedFrames.Count == 0)
            return;

        if (playbackIndex >= recordedFrames.Count)
        {
            isReplaying = false;
            Debug.Log("Ghost replay finished.");
            return;
        }

        ApplyMotion(recordedFrames[playbackIndex]);
        VerifyPosition(recordedFrames[playbackIndex]);
        playbackIndex++;
    }

    /// <summary>
    /// Call this to start replaying the ghost from the beginning.
    /// </summary>
    public void PlayGhost()
    {
        playbackIndex = 0;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        // Wait one physics frame to sync with player
        yield return new WaitForFixedUpdate();
        isReplaying = true;
    }

    /// <summary>
    /// Stops the ghost replay mid-way.
    /// </summary>
    public void StopGhost()
    {
        isReplaying = false;
        playbackIndex = 0;
    }

    public void LoadGhostFile()
    {
        string persistentPath = System.IO.Path.Combine(Application.persistentDataPath, FileName);
        string streamingPath = System.IO.Path.Combine(Application.streamingAssetsPath, FileName);

        string json = null;

        if (System.IO.File.Exists(persistentPath))
        {
            json = System.IO.File.ReadAllText(persistentPath);
            Debug.Log("Loaded ghost from persistentDataPath: " + persistentPath);
        }
#if UNITY_EDITOR
        else if (System.IO.File.Exists(streamingPath))
        {
            json = System.IO.File.ReadAllText(streamingPath);
            Debug.Log("Loaded ghost from StreamingAssets: " + streamingPath);
        }
#endif
        else
        {
            Debug.LogWarning("No ghost file found for scene " + SceneManager.GetActiveScene().name);
            recordedFrames = new List<InputFrame>();
            return;
        }

        GhostRecording wrapper = JsonUtility.FromJson<GhostRecording>(json);
        recordedFrames = wrapper.frames ?? new List<InputFrame>();
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
