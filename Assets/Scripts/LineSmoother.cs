using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineSmoother : MonoBehaviour
{
    [SerializeField]
    private int iterations = 1;

    
    [ContextMenu("Apply Smoothing")]
    public void ApplySmoothing()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();;
        
        if (lineRenderer == null)
        {
            return;
        }
        
        if (lineRenderer.positionCount <= 2)
        {
            return;
        }
        
        if (iterations < 1)
        {
            return;
        }
        
        Undo.RecordObject(lineRenderer, "Smooth Line");
        
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);
        
        List<Vector3> newPoints = new List<Vector3>(points);

        bool isClosed = lineRenderer.loop;

        for (int k = 0; k < iterations; k++)
        {
            List<Vector3> refined = new List<Vector3>();

            int count = newPoints.Count;
            int end = count - (isClosed ? 0 : 1);
            
            for (int i = 0; i < end; i++)
            {
                Vector3 p0 = newPoints[i];
                Vector3 p1 = newPoints[(i + 1) % count];

                // Chaikin’s rule: cut the corner
                Vector3 Q = Vector3.Lerp(p0, p1, 0.25f); // 25% point
                Vector3 R = Vector3.Lerp(p0, p1, 0.75f); // 75% point

                refined.Add(Q);
                refined.Add(R);
            }

            if (!isClosed)
            {
                // Keep endpoints if open curve
                refined.Insert(0, newPoints[0]);
                refined.Add(newPoints[newPoints.Count - 1]);
            }

            newPoints = refined;
        }

        Vector3[] newPointsArray = newPoints.ToArray();
        
        lineRenderer.positionCount = newPointsArray.Length;
        lineRenderer.SetPositions(newPointsArray);
    }
}
