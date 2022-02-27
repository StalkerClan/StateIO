using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBuilding : Building
{
    private Vector3 pointToCurve;
    private Vector3 leftPointToCurve;
    private Vector3 rightPointToCurve;
    private Vector3 mostLeftPointToCurve;
    private Vector3 mostRightPointToCurve;
    private Vector3 target;

    private int lineCapacity = 5;
    private int numberOfLines;

    private void Awake()
    {
        InitializeVariables();
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
            isMarching = true;
            isGenerating = false;
            InvokeRepeating("InitializeFighter", 0f, 0.5f);
        }
        PlayerInput.Instance.Released = false;

        if (!isMarching)
        {
            CancelInvoke("InitializeFighter");
        }
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
        pointToCurve = Vector3.Lerp(this.transform.position, target, 0.75f);
        direction = (target - this.transform.position).normalized;

        //leftDirection = Helpers.VectorByRotateAngle(2.5f, direction);
        //rightDirection = Helpers.VectorByRotateAngle(-2.5f, direction);
        //mostLeftDirection = Helpers.VectorByRotateAngle(5f, direction);
        //mostRightDirection = Helpers.VectorByRotateAngle(-5f, direction);

        //directions.Add(direction);
        //directions.Add(leftDirection);
        //directions.Add(rightDirection);
        //directions.Add(mostLeftDirection);
        //directions.Add(mostRightDirection);

        Vector3 leftVector = Helpers.PerpendicularCounterClockwise(direction);
        Vector3 rightVector = Helpers.PerpendicularClockwise(direction);
        leftPointToCurve = pointToCurve + (leftVector * spacing);
        rightPointToCurve = pointToCurve + (rightVector * spacing);
        mostLeftPointToCurve = pointToCurve + (leftVector * spacing * 2);
        mostRightDirection = pointToCurve + (rightVector * spacing * 2);
    }

    public void GetFighterMarchDirection()
    {
        fighterDirection = new FighterDirection();

        fighterDirection.MiddlePositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointToCurve, target);
        fighterDirection.LeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, leftPointToCurve, target);
        fighterDirection.RightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, rightPointToCurve, target);
        fighterDirection.MostLeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, mostLeftPointToCurve, target);
        fighterDirection.MostRightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, mostRightDirection, target);

        listOfPositions.Add(fighterDirection.MiddlePositions);
        listOfPositions.Add(fighterDirection.LeftPositions);
        listOfPositions.Add(fighterDirection.RightPositions);
        listOfPositions.Add(fighterDirection.MostLeftPositions);
        listOfPositions.Add(fighterDirection.MostRightPositions);
    }

    IEnumerator DelaySpawning()
    {
        WaitForSeconds delayTime = Utilities.GetWaitForSeconds(0.2f);
        yield return delayTime;

        InitializeFighter();
    }

    public void InitializeFighter()
    {     
        if (currentFighter > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject go = Instantiate(fighterPrefab, this.transform.position, Quaternion.identity, transform);
                go.GetComponent<Fighter>().March(listOfPositions[i]);
            }
            currentFighter -= 5;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject go = Instantiate(fighterPrefab, this.transform.position, Quaternion.identity, transform);
                go.GetComponent<Fighter>().March(listOfPositions[i]);
            }
            currentFighter = 0;
            isMarching = false;
            StartCoroutine(DelayGenerating()); 
        }
    }   
    
    IEnumerator DelayGenerating()
    {
        WaitForSeconds delayGenerating = Utilities.GetWaitForSeconds(1f);
        yield return delayGenerating;

        isGenerating = true;
    }    
}
