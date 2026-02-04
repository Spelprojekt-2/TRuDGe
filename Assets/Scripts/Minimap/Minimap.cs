using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
    [Header("---Minimap Visuals---")]
    [SerializeField] private int resolution = 150;
    [SerializeField] private float trackWidth = 10f;
    [SerializeField] private Color trackColor = Color.white;

    [Header("---Line renderer---")]
    [SerializeField] private UILineRenderer uiLine;
    private RaceController raceData;
    private Vector3 trackMin;
    private Vector3 trackMax;

    void Start()
    {
        raceData = FindFirstObjectByType<RaceController>();
        DrawTrack();
    }
    void DrawTrack()
    {
        SplineContainer container = raceData.trackSpline;
        if(container == null)
        {
            return;
        }

        Vector3 min = new Vector3(float.MaxValue, 0, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);

        for (int i = 0; i <= resolution; i++)
        {
            Vector3 p = container.EvaluatePosition(i / (float)resolution);
            if(p.x < min.x) min.x = p.x;
            if (p.z < min.z) min.z = p.z;
            if (p.x > max.x) max.x = p.x;
            if (p.z > max.z) max.z = p.z;
        }

        trackMin = min;
        trackMax = max;
        uiLine.thickness = trackWidth;
        uiLine.color = trackColor;

        List<Vector2> uiPoints = new List<Vector2>();
        RectTransform rect = uiLine.rectTransform;

        for (int i = 0; i <= resolution; i++)
        {
            Vector3 worldPos = container.EvaluatePosition(i / (float)resolution);

            float normX = Mathf.InverseLerp(min.x, max.x, worldPos.x);
            float normY = Mathf.InverseLerp(min.z, max.z, worldPos.z);

            float uiX = (normX - 0.5f) * rect.rect.width;
            float uiY = (normY - 0.5f) * rect.rect.height;

            uiPoints.Add(new Vector2(uiX, uiY));
        }

        uiLine.SetPoints(uiPoints);
    }

    public Vector2 GetWorldToMinimap(Vector3 worldPos)
    {
        float normX = Mathf.InverseLerp(trackMin.x, trackMax.x, worldPos.x);
        float normY = Mathf.InverseLerp(trackMin.z, trackMax.z, worldPos.z);
        Rect rect = uiLine.rectTransform.rect;
        float uiX = (normX - 0.5f) * rect.width;
        float uiY = (normY - 0.5f) * rect.height;

        return new Vector2(uiX, uiY);
    }
}