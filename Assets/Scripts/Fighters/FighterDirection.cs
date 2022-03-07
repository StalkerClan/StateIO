using UnityEngine;

public class FighterDirection
{
    private Vector3 middleDirection;
    private Vector3 leftDirection;
    private Vector3 rightDirection;
    private Vector3 mostLeftDirection;
    private Vector3 mostRightDirection;

    public Vector3 MiddleDirection { get => middleDirection; set => middleDirection = value; }
    public Vector3 LeftDirection { get => leftDirection; set => leftDirection = value; }
    public Vector3 RightDirection { get => rightDirection; set => rightDirection = value; }
    public Vector3 MostLeftDirection { get => mostLeftDirection; set => mostLeftDirection = value; }
    public Vector3 MostRightDirection { get => mostRightDirection; set => mostRightDirection = value; }

    public FighterDirection()
    {

    }
}

