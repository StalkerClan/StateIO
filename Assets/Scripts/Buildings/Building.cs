using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IInitializeVariables, IReceiver, ISubcriber
{
    public event Action<Building> OnChangingOnwer = delegate { };
    public event Action<int> OnChangingNumberOfFighters = delegate { };

    public class Point
    {
        public Vector3 middlePoint;
        public Vector3 leftPoint;
        public Vector3 rightPoint;
        public Vector3 mostLeftPoint;
        public Vector3 mostRightPoint;
    }

    [SerializeField] private BuildingStats buildingStats;

    [SerializeField] private GameObject fighterPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Color buildingColor;
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;

    protected List<Vector3[]> listOfPositions;
    protected List<Vector3> directions;
    protected List<Vector3> fighterStartPositions;
    protected Vector3 direction;
    protected Vector3 targetPosition;

    [SerializeField] private GlobalVariables.Building buildingType;
    protected Point pointsNearTarget;
    protected Point startPositions;
    protected FighterPosition fighterPosition;
    protected FighterDirection fighterDirection;

    [SerializeField] protected string targetID;
    [SerializeField] protected string buildingID;

    [SerializeField] private float timeToGenerate;
    [SerializeField] private float initializingDelay;
    protected float degree;
    protected float timeSinceGenerated;
    protected float totalTime;  
    protected float spacing;
   
    [SerializeField] private int startFighter;
    [SerializeField] private int maxCapacity;
    [SerializeField] private int currentFighter;
    protected int lineCapacity;

    [SerializeField] protected bool isGenerating;
    [SerializeField] protected bool owned;
    [SerializeField] protected bool taken;
    [SerializeField] protected bool selected;
    [SerializeField] protected bool isMarching;

    public BuildingStats BuildingStats { get => buildingStats; set => buildingStats = value; }
    public GameObject FighterPrefab { get => fighterPrefab; set => fighterPrefab = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public Color FirstColor { get => firstColor; set => firstColor = value; }  
    public Color SecondColor { get => secondColor; set => secondColor = value; }  

    public FighterPosition FighterDirection { get => fighterPosition; set => fighterPosition = value; }
    public GlobalVariables.Building BuildingType { get => buildingType; }

    public string BuildingID { get => buildingID; set => buildingID = value; }

    public float TimeToGenerate { get => timeToGenerate; set => timeToGenerate = value; }

    public int MaxCapacity { get => maxCapacity; set => maxCapacity = value; }
    public int CurrentFighter { get => currentFighter; set => currentFighter = value; }

    public bool Owned { get => owned; set => owned = value; }
    public bool Taken { get => taken; set => taken = value; }
    public bool Selected { get => selected; set => selected = value; }

    public virtual void SubcribeEvent()
    {

    }

    public virtual void UnsubcribeEvent()
    {

    }

    public void InitializeVariables()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        listOfPositions = new List<Vector3[]>();
        directions = new List<Vector3>();
        fighterStartPositions = new List<Vector3>();
        spacing = 0.08f;
        degree = 100f;
        initializingDelay = 0.4f;
        currentFighter = buildingStats.startFighter;
        isGenerating = true;
        isMarching = false;
    }

    public void GetBuildingStats(Building owner)
    {
        buildingStats = owner.BuildingStats;
        fighterPrefab = owner.FighterPrefab;
        spriteRenderer.sprite = owner.buildingStats.avatar;
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(owner.buildingStats.colorSet.keyColor));
        firstColor = Utilities.HexToColor(Utilities.ColorToHex(owner.buildingStats.colorSet.firstColor));
        secondColor = Utilities.HexToColor(Utilities.ColorToHex(owner.buildingStats.colorSet.secondColor));
        buildingType = owner.BuildingType;
        spriteRenderer.color = owner.buildingColor;
        timeToGenerate = owner.buildingStats.timeToGenerate;
        startFighter = owner.buildingStats.startFighter;
        maxCapacity = owner.buildingStats.maxCapacity;     
    }

    public void GetBuildingType()
    {
        switch (buildingType)
        {
            case GlobalVariables.Building.Player:
                owned = true;
                taken = false;
                break;
            case GlobalVariables.Building.Neutral:
                owned = false;
                taken = false;
                break;
            case GlobalVariables.Building.Opponent:
                owned = false;
                taken = true;
                break;
            default:
                break;
        }
    }

    //Tao them fighter theo thoi gian
    public void GenerateFighter()
    {
        if (!isGenerating)
        {
            return;
        }
        LimitGeneratedFighter();   
        timeSinceGenerated += Time.deltaTime;
        if (timeSinceGenerated >= timeToGenerate)
        {
            OnChangingNumberOfFighters?.Invoke(1);
            timeSinceGenerated = 0f;
        }
    }

    //Gioi han so luong fighter
    public void LimitGeneratedFighter()
    {
        if (currentFighter >= maxCapacity)
        {
            isGenerating = false;
        }
    }

    public void ChangeNumberOfFighter(int fighter)
    {
        currentFighter += fighter;
    }

    //Nhan Fighter tu cac building khac
    public void ReceiveFighter(int fighter, Building buildingOwner)
    {
        GlobalVariables.Building invader = buildingOwner.BuildingType;
        if (buildingType.Equals(invader))
        {
            OnChangingNumberOfFighters?.Invoke(fighter);
        }
        else
        {
            if (currentFighter <= 0)
            {      
                if (buildingOwner.BuildingType.Equals(GlobalVariables.Building.Player))
                {
                    owned = true;
                    taken = false;
                    LevelManager.Instance.PlayerBuilding -= 8;
                    
                }
                else if (buildingOwner.BuildingType.Equals(GlobalVariables.Building.Opponent))
                {
                    owned = false;
                    taken = true;
                    LevelManager.Instance.PlayerBuilding--;
                }
                OnChangingOnwer?.Invoke(buildingOwner);
                LevelManager.Instance.CheckPlayerOwnedBuilding();                 
            }
            else
            {
                OnChangingNumberOfFighters?.Invoke(-1 * fighter);
            }
            StartCoroutine(DelayGenerating());
        }
    }

    public void FighterMarching(string currentTargetID, Vector3 currentTargetPosition)
    {
        if (selected)
        {
            this.targetID = currentTargetID;
            this.targetPosition = currentTargetPosition;
            direction = (targetPosition - this.transform.position).normalized;
            GetPointNearTarget();
            GetMarchDirectionByTargetPosition();
            FormArcFormation();
            isMarching = true;
            isGenerating = false;
            InvokeRepeating(nameof(InitializeFighter), 0f, initializingDelay);
        }
        selected = false;
    }

    public void FormArcFormation()
    {
        fighterStartPositions.Clear();

        Vector3 leftVector = Helpers.VectorByRotateAngle(degree, direction);
        Vector3 rightVector = Helpers.VectorByRotateAngle(360 - degree, direction);

        Vector3 middlePoint = this.transform.position;
        Vector3 mostLeftPoint = this.transform.position + (spacing * leftVector);
        Vector3 mostRightPoint = this.transform.position + (spacing * rightVector);
        Vector3 leftPoint = Vector3.Lerp(this.transform.position, mostLeftPoint, 0.5f);
        Vector3 rightPoint = Vector3.Lerp(this.transform.position, mostRightPoint, 0.5f);

        fighterStartPositions.Add(middlePoint);
        fighterStartPositions.Add(leftPoint);
        fighterStartPositions.Add(rightPoint);
        fighterStartPositions.Add(mostLeftPoint);
        fighterStartPositions.Add(mostRightPoint);
    }

    public void GetPointToFormQuadraticCurve()
    {
        direction = (this.targetPosition - this.transform.position).normalized;

        Vector3 leftVector = Helpers.PerpendicularCounterClockwise(direction);
        Vector3 rightVector = Helpers.PerpendicularClockwise(direction);

        pointsNearTarget = new Point
        {
            middlePoint = Vector3.Lerp(this.transform.position, this.targetPosition, 0.9f)
        };

        pointsNearTarget.leftPoint = pointsNearTarget.middlePoint + (spacing * leftVector);
        pointsNearTarget.rightPoint = pointsNearTarget.middlePoint + (spacing *rightVector);
        pointsNearTarget.mostLeftPoint = pointsNearTarget.middlePoint + (1.5f * spacing * leftVector);
        pointsNearTarget.mostRightPoint = pointsNearTarget.middlePoint + (1.5f * spacing * rightVector);
    }

    public void GetMarchPositionByBerzierCurve()
    {
        listOfPositions.Clear();

        fighterPosition = new FighterPosition
        {
            MiddlePositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointsNearTarget.middlePoint, this.targetPosition),
            LeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointsNearTarget.leftPoint, this.targetPosition),
            RightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointsNearTarget.rightPoint, this.targetPosition),
            MostLeftPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointsNearTarget.mostLeftPoint, this.targetPosition),
            MostRightPositions = Curve.GetQuadraticBezierCurve(this.totalTime, this.transform.position, pointsNearTarget.mostRightPoint, this.targetPosition)
        };

        listOfPositions.Add(fighterPosition.MiddlePositions);
        listOfPositions.Add(fighterPosition.LeftPositions);
        listOfPositions.Add(fighterPosition.RightPositions);
        listOfPositions.Add(fighterPosition.MostLeftPositions);
        listOfPositions.Add(fighterPosition.MostRightPositions);
    }

    public void GetPointNearTarget()
    {
        Vector3 oppositeDirection = -direction;
        Vector3 leftVector = Helpers.VectorByRotateAngle(360 - 70, oppositeDirection); 
        Vector3 rightVector = Helpers.VectorByRotateAngle(70, oppositeDirection);
        
        pointsNearTarget = new Point
        {
            middlePoint = targetPosition,
            leftPoint = targetPosition + (spacing * leftVector),
            rightPoint = targetPosition + (spacing * rightVector),
            mostLeftPoint = targetPosition + (2f * spacing * leftVector),
            mostRightPoint = targetPosition + (2f * spacing * rightVector),
        };
    }

    public void GetMarchDirectionByTargetPosition()
    {
        directions.Clear();

        fighterDirection = new FighterDirection
        {
            MiddleDirection = direction,
            LeftDirection = (pointsNearTarget.leftPoint - this.transform.position).normalized,
            RightDirection = (pointsNearTarget.rightPoint - this.transform.position).normalized,
            MostLeftDirection = (pointsNearTarget.mostLeftPoint - this.transform.position).normalized,
            MostRightDirection = (pointsNearTarget.mostRightPoint - this.transform.position).normalized,
        };

        directions.Add(fighterDirection.MiddleDirection);
        directions.Add(fighterDirection.LeftDirection);
        directions.Add(fighterDirection.RightDirection);
        directions.Add(fighterDirection.MostLeftDirection);
        directions.Add(fighterDirection.MostRightDirection);
    }

    public void InitializeFighter()
    {
        lineCapacity = currentFighter > 5 ? lineCapacity = 5 : lineCapacity = currentFighter;
      
        if (currentFighter > 5)
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i);       
            }
            OnChangingNumberOfFighters(-5);
        }
        else
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i);
            }
            OnChangingNumberOfFighters(-1 * currentFighter);
            isMarching = false;
            StartCoroutine(DelayGenerating());
        }
    }

    public void GetFighterFromPool(int index)
    {
        GameObject newFighter = ObjectPooler.Instance.GetObject(fighterPrefab);
        Fighter fighterStats = newFighter.GetComponent<Fighter>();
        newFighter.transform.position = fighterStartPositions[index];
        fighterStats.SpriteRenderer.color = buildingColor;
        fighterStats.BuildingOwner = this;
        fighterStats.TargetID = this.targetID;
        fighterStats.MoveDirection = directions[index];
    }

    IEnumerator DelayGenerating()
    {
        WaitForSeconds delayGenerating = Utilities.GetWaitForSeconds(0.6f);
        yield return delayGenerating;

        isGenerating = true;
    }
}
