using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class TimeTrialCapture : MonoBehaviour
{
    public bool capture;

    private List<InputFrame> recordedFrames = new();

    // Automatically uses the current scene name for the ghost file
    private string FileName => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_Ghost.ghost";

    private string GetPersistentPath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }

    private string GetStreamingPath()
    {
        return Path.Combine(Application.streamingAssetsPath, FileName);
    }

    private void FixedUpdate()
    {
        if (!capture) return;

        // Capture velocity, rotation, and position
        InputFrame frame = new InputFrame
        {
            velocity = GetComponent<Rigidbody>().linearVelocity,  // Capture velocity
            rotation = transform.rotation.eulerAngles,       // Capture rotation (Euler angles)
            position = transform.position                    // Capture position
        };

        recordedFrames.Add(frame);
    }

    // ===================== Public Control Methods =====================

    public void StartCapture()
    {
        recordedFrames.Clear();
        capture = true;
        Debug.Log("Time trial capture started.");
    }

    public void StopCapture()
    {
        capture = false;
        SaveToFile();
        Debug.Log($"Time trial capture stopped. Recorded {recordedFrames.Count} frames.");
    }

    public void CancelCapture()
    {
        capture = false;
        recordedFrames.Clear();
        Debug.Log("Time trial capture cancelled.");
    }

    /// <summary>
    /// Loads a previously saved ghost recording from file.
    /// Checks persistentDataPath first, then StreamingAssets.
    /// </summary>
    public List<InputFrame> LoadRecording()
    {
        string persistentPath = GetPersistentPath();
        string streamingPath = GetStreamingPath();

        if (File.Exists(persistentPath))
            return LoadFromFile(persistentPath);

        if (File.Exists(streamingPath))
            return LoadFromFile(streamingPath);

        Debug.LogWarning("No ghost recording found.");
        return null;
    }

    // ===================== Internal Saving =====================

    private void SaveToFile()
    {
        string path = GetPersistentPath();

        // Check old recording length
        int oldCount = 0;
        if (File.Exists(path))
        {
            string oldJson = File.ReadAllText(path);
            GhostRecording oldWrapper = JsonUtility.FromJson<GhostRecording>(oldJson);
            oldCount = oldWrapper.frames.Count;
        }

        if (recordedFrames.Count < oldCount)
        {
            Debug.Log("New recording is shorter than existing ghost. Not overwriting.");
            return;
        }

        GhostRecording wrapper = new GhostRecording
        {
            frames = recordedFrames
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);

#if UNITY_EDITOR
        // Save also to StreamingAssets in editor
        Directory.CreateDirectory(Path.GetDirectoryName(GetStreamingPath()));
        File.WriteAllText(GetStreamingPath(), json);
#endif

        Debug.Log("Ghost saved to: " + path);
    }

    private List<InputFrame> LoadFromFile(string path)
    {
        string json = File.ReadAllText(path);
        GhostRecording wrapper = JsonUtility.FromJson<GhostRecording>(json);
        return wrapper.frames;
    }
}

[System.Serializable]
public struct InputFrame
{
    public Vector3 velocity;
    public Vector3 rotation;
    public Vector3 position;
}

[System.Serializable]
public class GhostRecording
{
    public List<InputFrame> frames;
}
