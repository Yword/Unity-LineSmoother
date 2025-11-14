using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LineSmoother lineSmoother;
    
    private Vector3[] originalPoints;


    private void Awake()
    {
        originalPoints = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(originalPoints);
    }
    
    
    public void GenerateLine()
    {
        int pointCount = Random.Range(3, 8);
        Vector3[] points = new Vector3[pointCount];
        
        for (int i = 0; i < pointCount; i++)
        {
            points[i] = new Vector3(Random.Range(-20f, 20f), Random.Range(-15f, 15f), 0);
        }
        
        lineRenderer.loop = Random.value > 0.5f ? true : false;
        
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPositions(points);
        
        originalPoints = points;
    }
    

    public void ApplySmoothing()
    {
        lineSmoother.ApplySmoothing();
    }
    
    
    public void Revert()
    {
        lineRenderer.positionCount = originalPoints.Length;
        lineRenderer.SetPositions(originalPoints);
    }
}
