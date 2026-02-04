using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILineRenderer : MaskableGraphic
{
    [HideInInspector] public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public float thickness = 5f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points.Count < 2) return;

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 dir;
            if (i < points.Count - 1)
                dir = (points[i + 1] - points[i]).normalized;
            else
                dir = (points[i] - points[i - 1]).normalized;

            Vector2 normal = new Vector2(-dir.y, dir.x) * (thickness * 0.5f);


            vh.AddVert(points[i] + normal, color, new Vector2(0.5f, 0.5f)); 
            vh.AddVert(points[i] - normal, color, new Vector2(0.5f, 0.5f)); 

            if (i > 0)
            {
                int vertIndex = i * 2;
                vh.AddTriangle(vertIndex - 2, vertIndex, vertIndex - 1);
                vh.AddTriangle(vertIndex - 1, vertIndex, vertIndex + 1);
            }
            if (points.Count > 2 && Vector2.Distance(points[0], points[points.Count - 1]) < 0.1f)
            {
                int last = (points.Count - 1) * 2;
                vh.AddTriangle(last, 0, last + 1);
                vh.AddTriangle(last + 1, 0, 1);
            }
        }
    }

    public void SetPoints(List<Vector2> newPoints)
    {
        points = newPoints;
        SetAllDirty();
    }
}
