using UnityEngine;

public class FighterDirection 
{
    private Vector3[] middlePositions;
    private Vector3[] leftPositions;
    private Vector3[] mostLeftPositions;
    private Vector3[] rightPositions;
    private Vector3[] mostRightPositions;

    public Vector3[] MiddlePositions { get => middlePositions; set => middlePositions = value; }
    public Vector3[] LeftPositions { get => leftPositions; set => leftPositions = value; }
    public Vector3[] MostLeftPositions { get => mostLeftPositions; set => mostLeftPositions = value; }
    public Vector3[] RightPositions { get => rightPositions; set => rightPositions = value; }
    public Vector3[] MostRightPositions { get => mostRightPositions; set => mostRightPositions = value; }

    public FighterDirection()
    {
        
    }
}
