using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class Speltest_CaptureAll : MonoBehaviour
{
    public bool capture;

    private List<InputFrame> recordedFrames = new();

    private void FixedUpdate()
    {
        if (!capture) return;

        InputFrame frame = new InputFrame
        {
            velocity = GetComponent<Rigidbody>().linearVelocity,
            rotation = transform.rotation.eulerAngles,
            position = transform.position
        };

        recordedFrames.Add(frame);
    }


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
    private void SaveToFile()
    {
        string fileName = Guid.NewGuid().ToString() + ".json";
        string directory = Path.Combine(Application.persistentDataPath, "Saved");
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        string path = Path.Combine(directory, fileName);


        RacerData rd = GetComponent<RacerData>();
        double newTime = rd.GetRaceTime();

        GhostRecording wrapper = new GhostRecording
        {
            time = newTime,
            frames = recordedFrames,
            name = "SpeltestSpelare"
        };

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json);
    }
}