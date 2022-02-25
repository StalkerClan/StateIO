using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public static Brush Instance;
    public LineRenderer lineRenderer;

    private Vector3[] positions;

    public Vector3[] Positions { get => positions; set => positions = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
    }

    public void SetUpLine(Vector3[] positions)
    {
        lineRenderer.positionCount = positions.Length;
        this.positions = positions;
    }

    public void DrawLine()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i,positions[i]);
        }
    }
}
