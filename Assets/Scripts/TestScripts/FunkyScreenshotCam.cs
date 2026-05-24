using UnityEngine;

public class FunkyScreenshotCam : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private int supersize;
    [SerializeField] private float delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                ScreenCapture.CaptureScreenshot(fileName, supersize);
                Debug.Log("Screenshot captured!");
            }
        }
    }
}
