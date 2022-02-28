using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IInitializeVariables, IReceiver
{
    public GameObject fighterPrefab;
    public SpriteRenderer spriteRenderer;

    public Color buildingColor;
    protected List<Vector3[]> listOfPositions;
    protected List<Vector3> directions;

    protected Vector3 direction;
    protected Vector3 leftDirection;
    protected Vector3 rightDirection;
    protected Vector3 mostLeftDirection;
    protected Vector3 mostRightDirection;
    protected Vector3 targetPosition;

    protected Vector3 pointToCurve;
    protected Vector3 leftPointToCurve;
    protected Vector3 rightPointToCurve;
    protected Vector3 mostLeftPointToCurve;
    protected Vector3 mostRightPointToCurve;

    public GlobalVariables.Building buildingType;
    protected PlayerInput playerInput;
    protected FighterDirection fighterDirection;

    public string targetID;
    public string BuildingID;

    protected float timeToGenerate;
    protected float timer;
    protected float totalTime;
    public float generatingCooldown;
    public float spacing;

    public int startFighter;
    public int maxCapacity;
    public int currentFighter;

    public bool isGenerating;
    public bool isMarching;
    public bool owned;
    public bool selected;

    public GameObject FighterPrefab { get => fighterPrefab; set => fighterPrefab = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public FighterDirection FighterDirection { get => fighterDirection; set => fighterDirection = value; }
    public GlobalVariables.Building BuildingType { get => buildingType; }
    public float GeneratingCooldown { get => generatingCooldown; set => generatingCooldown = value; }
    public float TimeToGenerate { get => timeToGenerate; set => timeToGenerate = value; }
    public bool Owned { get => owned; set => owned = value; }
    public bool Selected { get => selected; set => selected = value; }

    public void InitializeVariables()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingColor = spriteRenderer.color;
        playerInput = PlayerInput.Instance;
        listOfPositions = new List<Vector3[]>();
        directions = new List<Vector3>();
        BuildingID = gameObject.name;
        spacing = 0.5f;
        totalTime = 10f;
        isGenerating = true;
        isMarching = false;
    }

    public void GetBuildingType()
    {
        switch (buildingType)
        {
            case GlobalVariables.Building.Player:
                startFighter = 10;
                maxCapacity = 50;
                break;
            case GlobalVariables.Building.Neutral:
                startFighter = 10;
                maxCapacity = 50;
                break;
            case GlobalVariables.Building.Opponent:
                startFighter = 10;
                maxCapacity = 50;
                break;
            default:
                break;
        }
        currentFighter = startFighter;
    }

    public void GenerateFighter()
    {
        if (!isGenerating)
        {
            return;
        }
        LimitGeneratedFighter();
        timer += Time.deltaTime;
        if (timer >= timeToGenerate)
        {
            currentFighter++;
            timeToGenerate += generatingCooldown;
        }
    }

    public void LimitGeneratedFighter()
    {
        if (currentFighter >= maxCapacity)
        {
            isGenerating = false;
        }
    }

    public void ReceiveFighter(int fighter, Building buildingOwner)
    {
        GlobalVariables.Building invader = buildingOwner.BuildingType;
        if (buildingType.Equals(invader))
        {
            currentFighter += fighter;
        }
        else
        {
            if (currentFighter <= 0)
            {
                if (!invader.Equals(GlobalVariables.Building.Player)) return;
                fighterPrefab = buildingOwner.FighterPrefab;
                buildingType = invader; 
                Utilities.SetBuildingColor(buildingType, spriteRenderer);
                SetBuildingStats(buildingOwner);
                owned = true;
            }
            else
            {
                currentFighter -= fighter;
            }
        }
    }

    private void SetBuildingStats(Building buildingOwner)
    {
        this.generatingCooldown = buildingOwner.GeneratingCooldown;
        this.timeToGenerate = buildingOwner.TimeToGenerate;
    }

    public void FighterMarching(string currentTargetID, Vector3 currentTargetPosition)
    {
        if (selected)
        {
            Debug.Log("hehe");
            this.targetID = currentTargetID;
            this.targetPosition = currentTargetPosition;
            GetPointToFormQuadraticCurve();
            GetFighterMarchDirectionByPositions();
            isMarching = true;
            isGenerating = false;
            InvokeRepeating("InitializeFighter", 0f, 0.5f);
        }
        selected = false;
    }

    public void GetPointToFormQuadraticCurve()
    {
        pointToCurve = Vector3.Lerp(this.transform.position, this.targetPosition, 0.9f);
        direction = (this.targetPosition - this.transform.position).normalized;

        Vector3 leftVector = Helpers.PerpendicularCounterClockwise(direction);
        Vector3 rightVector = Helpers.PerpendicularClockwise(direction);
        leftPointToCurve = pointToCurve + (leftVector * spacing);
        rightPointToCurve = pointToCurve + (rightVector * spacing);
        mostLeftPointToCurve = pointToCurve + (2 * spacing * leftVector);
        mostRightPointToCurve = pointToCurve + (2 * spacing * rightVector);
    }

    private void GetFighterMarchDirectionWithVector()
    {
        directions.Clear();

        leftDirection = Helpers.VectorByRotateAngle(2.5f, direction);
        rightDirection = Helpers.VectorByRotateAngle(-2.5f, direction);
        mostLeftDirection = Helpers.VectorByRotateAngle(5f, direction);
        mostRightDirection = Helpers.VectorByRotateAngle(-5f, direction);

        directions.Add(direction);
        directions.Add(leftDirection);
        directions.Add(rightDirection);
        directions.Add(mostLeftDirection);
        directions.Add(mostRightDirection);
    }

    public void GetFighterMarchDirectionByPositions()
    {
        listOfPositions.Clear();

        fighterDirection = new FighterDirection();

        fighterDirection.MiddlePositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointToCurve, this.targetPosition);
        fighterDirection.LeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, leftPointToCurve, this.targetPosition);
        fighterDirection.RightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, rightPointToCurve, this.targetPosition);
        fighterDirection.MostLeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, mostLeftPointToCurve, this.targetPosition);
        fighterDirection.MostRightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, mostRightPointToCurve, this.targetPosition);

        listOfPositions.Add(fighterDirection.MiddlePositions);
        listOfPositions.Add(fighterDirection.LeftPositions);
        listOfPositions.Add(fighterDirection.RightPositions);
        listOfPositions.Add(fighterDirection.MostLeftPositions);
        listOfPositions.Add(fighterDirection.MostRightPositions);
    }

    public void InitializeFighter()
    {
        if (currentFighter > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject go = Instantiate(fighterPrefab, this.transform.position, Quaternion.identity, transform);
                Fighter fighter = go.GetComponent<Fighter>();
                fighter.BuildingOwner = this;
                fighter.TargetID = this.targetID;
                fighter.March(listOfPositions[i]);

            }
            currentFighter -= 5;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject go = Instantiate(fighterPrefab, this.transform.position, Quaternion.identity, transform);
                Fighter fighter = go.GetComponent<Fighter>();

                fighter.BuildingOwner = this;
                fighter.TargetID = this.targetID;
                fighter.March(listOfPositions[i]);
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
