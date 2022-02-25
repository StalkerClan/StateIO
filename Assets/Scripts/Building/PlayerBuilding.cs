using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBuilding : Building
{
    private Vector3 pointToCurve;
    private Vector3 leftPointToCurve;
    private Vector3 rightPointToCurve;
    private Vector3 target;

    private int lineCapacity = 5;
    private int numberOfLines;

    private void Awake()
    {
        InitializeVariables();
        spacing = 0.5f;
        totalTime = 20f;
        GetBuildingType();

    }

    private void Update()
    {
        GenerateFighter();
        PlayerInput.Instance.GetDirection();
        if (PlayerInput.Instance.Released)
        {
            target = PlayerInput.Instance.TargetPosition;
            GetPointToFormQuadraticCurve();
            GetFighterMarchDirection();
            InitializeFighter();
        }
        PlayerInput.Instance.Released = false;
    }

    public void GetNumberOfLines(int numberOfFighters)
    {
        if (numberOfFighters <= 0)
        {
            return;
        }
        int result = numberOfFighters % lineCapacity;
        if (result == 0)
        {
            numberOfLines = numberOfFighters / lineCapacity;
        }
        else
        {
            numberOfLines = (numberOfFighters / lineCapacity) + 1;
        }
    }

    public void GetPointToFormQuadraticCurve()
    {
        pointToCurve = (target - this.transform.position) * 0.75f;
        Debug.Log(pointToCurve);
        direction = pointToCurve - target;
        Debug.Log(direction);
        Vector3 tempVector = Helpers.PerpendicularClockwise(direction);
        leftPointToCurve = tempVector / 10;
        GameObject go = new GameObject();
        go.transform.position = leftPointToCurve;
        rightPointToCurve = Helpers.VectorByRotateAngle(-90, direction);
    }

    public void GetFighterMarchDirection()
    {
        fighterDirection = new FighterDirection();
        fighterDirection.MiddlePositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointToCurve, target);
        fighterDirection.LeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, leftPointToCurve, target);
        fighterDirection.RightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, rightPointToCurve, target);
    }

    public void InitializeFighter()
    {
        GameObject go = Instantiate(fighterPrefab, this.transform.position, Quaternion.identity, transform);
        go.GetComponent<Fighter>().March(fighterDirection.RightPositions);//Positions = fighterDirection.LeftPositions;
    }
}
