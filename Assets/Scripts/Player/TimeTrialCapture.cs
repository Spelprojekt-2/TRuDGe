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

    // ===================== Internal Saving =====================

    private void SaveToFile()
    {
        string path = GetPersistentPath();

        RacerData rd = GetComponent<RacerData>();
        double newTime = rd.GetRaceTime();

        if (File.Exists(path))
        {
            string oldJson = File.ReadAllText(path);
            GhostRecording oldWrapper = JsonUtility.FromJson<GhostRecording>(oldJson);

            if (oldWrapper != null && oldWrapper.time <= newTime)
            {
                Debug.Log("Existing time is better. Not overwriting.");
                return;
            }
        }

        GhostRecording wrapper = new GhostRecording
        {
            time = newTime
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);

        Debug.Log("New best ghost saved! Time: " + newTime);
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
    public string name;
    public double time;
    public List<InputFrame> frames;
}
