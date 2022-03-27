using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInitializeVariables, ISubcriber, IReceiver
{

    public event Action<Owner> OnChangingOnwer = delegate { };
    public event Action<float> OnChangingNumberOfFighters = delegate { };
    public event Action<float> OnEnableBuilding = delegate { };
    public event Action OnDisableBuilding = delegate { };

    public class VectorSet
    {
        public List<Vector3> startPositions;
        public List<Vector3> fighterDirections;

        public VectorSet(List<Vector3> newStartPositions, List<Vector3> newFighterDirections)
        {
            startPositions = newStartPositions;
            fighterDirections = newFighterDirections;
        }
    }

    [SerializeField] private GameObject fighterPrefab;
    [SerializeField] private CollideDetector collideDetector;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Owner defaultOwner;
    [SerializeField] private Owner buildingOwner;

    [SerializeField] private Color buildingColor;

    [SerializeField] private GlobalVariables.Owner ownerType;
    [SerializeField] private GlobalVariables.Owner defaultOwnerType;

    private Dictionary<Building, VectorSet> directionDictionary;

    [SerializeField] private List<Building> nearbyBuildings;

    [SerializeField] private float produceSpeed;
    [SerializeField] private float fighterPerTick;
    [SerializeField] private float initializingDelay;
    [SerializeField] private float generatingDelay;
    [SerializeField] private float multiplier;
    private float degree;
    private float timeSinceGenerated;
    private float spacing;
   
    [SerializeField] private int startFighter;
    [SerializeField] private int maxCapacity;
    [SerializeField] private float currentFighter;
    protected int lineCapacity;

    [SerializeField] private bool isGenerating;
    [SerializeField] private bool owned;
    [SerializeField] private bool taken;
    [SerializeField] private bool isMarching;
    [SerializeField] private bool active;
    [SerializeField] private bool paused;

    public CollideDetector CollideDetector { get => collideDetector; set => collideDetector = value; }
    public Owner BuildingOwner { get => buildingOwner; set => buildingOwner = value; }
    public Owner DefaultOwner { get => defaultOwner; set => defaultOwner = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }
    public GlobalVariables.Owner DefaultOwnerType { get => defaultOwnerType; set => defaultOwnerType = value; }
    public List<Building> NearbyBuildings { get => nearbyBuildings; set => nearbyBuildings = value; }
    public float ProduceSpeed { get => produceSpeed; set => produceSpeed = value; }
    public float FighterPerTick { get => fighterPerTick; set => fighterPerTick = value; }
    public int StartFighter { get => startFighter; set => startFighter = value; }
    public int MaxCapacity { get => maxCapacity; set => maxCapacity = value; }
    public float CurrentFighter { get => currentFighter; set => currentFighter = value; }
    public bool IsGenerating { get => isGenerating; set => isGenerating = value; }
    public bool Owned { get => owned; set => owned = value; }
    public bool Taken { get => taken; set => taken = value; }
    public bool IsMarching { get => isMarching; set => isMarching = value; }
    public bool Active { get => active; set => active = value; }

    private void Start()
    {
        SetOwner(defaultOwner, defaultOwner.OwnerType);
        SubcribeEvent();
        paused = false;
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    private void Update()
    {
        if (paused) return;
        GenerateFighter();
    }

    public void SubcribeEvent()
    {
        OnChangingNumberOfFighters += ChangeNumberOfFighter;
        OnChangingOnwer += StopSpawningFighter;
    }

    public void UnsubcribeEvent()
    {
        OnChangingNumberOfFighters -= ChangeNumberOfFighter;
        OnChangingOnwer -= StopSpawningFighter;
        buildingOwner.OnChangingColorSet -= ChangeBuildingColor;

        if (buildingOwner is Player)
        {
            (buildingOwner as Player).OnUpgradeStartUnits -= UpgradeStartUnits;
            (buildingOwner as Player).OnUpgradeProduceSpeed -= UpgradeProduceSpeed;
        }
    }

    public void SetOwner(Owner owner, GlobalVariables.Owner owerType)
    {
        GetDefaultOwner(owner);
        GetBuildingStats(owner);        
        GetOwnerType(owerType);
        InitializeVariables();
        OnChangingNumberOfFighters?.Invoke(owner.ownerStat.StartFighter);
        OnChangingOnwer?.Invoke(owner);
        if (currentFighter >= startFighter) currentFighter = startFighter;
        collideDetector.Building = this;
        Utilities.ChangeColorAlpha(spriteRenderer.color, collideDetector.SpriteRenderer.color);
    }

    public void GetBuildingStats(Owner owner)
    {
        if (buildingOwner != null)
        {
            if (buildingOwner is Player)
            {
                (buildingOwner as Player).OnUpgradeStartUnits -= UpgradeStartUnits;
                (buildingOwner as Player).OnUpgradeProduceSpeed -= UpgradeProduceSpeed;
            }
            buildingOwner.OnChangingColorSet -= ChangeBuildingColor;    
        }

        buildingOwner = owner;
        buildingOwner.OnChangingColorSet += ChangeBuildingColor;
        if (buildingOwner is Player)
        {
            (buildingOwner as Player).OnUpgradeStartUnits += UpgradeStartUnits;
            (buildingOwner as Player).OnUpgradeProduceSpeed += UpgradeProduceSpeed;
        }
        ownerType = buildingOwner.OwnerType;
        spriteRenderer.sprite = defaultOwner.OwnerStat.BuildingIcon;
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(buildingOwner.ColorSet.KeyColor));
        spriteRenderer.color = buildingColor;
        produceSpeed = owner.OwnerStat.ProduceSpeed;
        maxCapacity = owner.OwnerStat.MaxCapacity;
    } 

    public void GetDefaultOwner(Owner owner)
    {
        defaultOwner = owner;
        buildingOwner = defaultOwner;
        defaultOwnerType = defaultOwner.OwnerType;
    }

    public void GetOwnerType(GlobalVariables.Owner type)
    {
        switch (type)
        {
            case GlobalVariables.Owner.Player:
                owned = true;
                taken = false;
                break;
            case GlobalVariables.Owner.Neutral:
                owned = false;
                taken = false;
                break;
            case GlobalVariables.Owner.Enemy:
                owned = false;
                taken = true;
                break;
            default:
                break;
        }
    }

    public void InitializeVariables()
    {
        directionDictionary = new Dictionary<Building, VectorSet>();
        spacing = 0.12f;
        multiplier = 2f;
        degree = 100f;
        fighterPerTick = buildingOwner.OwnerStat.FighterPerTick;
        initializingDelay = 0.3f;
        generatingDelay = 1.8f;
        startFighter = buildingOwner.OwnerStat.StartFighter;
        currentFighter = startFighter;
        isGenerating = false;
        isMarching = false;
        active = false;
    }

    public void UpgradeStartUnits(int value)
    {
        OnChangingNumberOfFighters?.Invoke(value);
    }

    public void UpgradeProduceSpeed (float value)
    {
        fighterPerTick = value;
    }

    public void ChangeBuildingColor(ColorSet newColorSet)
    {
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.KeyColor));
        spriteRenderer.color = buildingColor;
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
        if (timeSinceGenerated >= produceSpeed)
        {
            OnChangingNumberOfFighters?.Invoke(currentFighter + fighterPerTick);
            timeSinceGenerated = 0f;
        }
    }

    public void StopSpawningFighter(Owner owner)
    {
        isMarching = false;
    }

    //Gioi han so luong fighter
    public void LimitGeneratedFighter()
    {
        if (currentFighter >= maxCapacity)
        {
            isGenerating = false;
        }
    }

    public void ChangeNumberOfFighter(float fighter)
    {
        currentFighter = fighter;
    }

    //Nhan Fighter tu cac building khac
    public void ReceiveFighter(int fighter, Owner invader)
    {
        isGenerating = false;
        GlobalVariables.Owner invaderType = invader.OwnerType;
        if (buildingOwner == invader)
        {
            OnChangingNumberOfFighters?.Invoke(currentFighter + fighter);
        }
        else
        {
            if (currentFighter <= 0)
            {
                if (invader.OwnerType.Equals(GlobalVariables.Owner.Player))
                {
                    owned = true;
                    taken = false;
                }
                else
                {
                    owned = false;
                    taken = true;
                    PlayerInput.Instance.CheckSelectedBuilding(this);
                }
                this.buildingOwner.RemoveBuilding(this);
                invader.AddBuilding(this);
                LevelManager.Instance.LevelGenerator.CheckWinCondition();
                OnChangingOnwer?.Invoke(invader);
            }
            else
            {
                OnChangingNumberOfFighters?.Invoke(currentFighter - fighter);
            }
        }
        StartCoroutine(DelayGenerating());
    }

    public void FighterMarching(Building targetBuilding, Vector3 currentTargetPosition)
    {
        if (isMarching) return;
        isMarching = true;
        isGenerating = false;
        Vector3 direction = (currentTargetPosition - this.transform.position).normalized;

        if (directionDictionary.TryGetValue(targetBuilding, out VectorSet existedfighterDirecions))
        {
            StartCoroutine(DelayMarching(targetBuilding, existedfighterDirecions));
        }
        else
        {
            VectorSet vectorSet = GetVectorSet(currentTargetPosition);
            directionDictionary.Add(targetBuilding, vectorSet);
            StartCoroutine(DelayMarching(targetBuilding, vectorSet));
        }
    }

    public VectorSet GetVectorSet(Vector3 targetPosition)
    {
        float additionDegree = 10f;

        List<Vector3> tempStartPositions = new List<Vector3>();
        List<Vector3> tempFighterDirections = new List<Vector3>();

        Vector3 direction = (targetPosition - this.transform.position).normalized;

        //Form arc formation
        Vector3 leftVector = Helpers.VectorByRotateAngle(degree, direction);
        Vector3 rightVector = Helpers.VectorByRotateAngle(360 - degree, direction);
        Vector3 mostLeftVector = Helpers.VectorByRotateAngle(degree + additionDegree, direction);
        Vector3 mostRightVector = Helpers.VectorByRotateAngle(360 - (degree + additionDegree), direction);


        Vector3 middlePoint = this.transform.position;
        Vector3 leftPoint = this.transform.position + (spacing * leftVector);
        Vector3 rightPoint = this.transform.position + (spacing * rightVector);
        Vector3 mostLeftPoint = this.transform.position + (2f * spacing * mostLeftVector);
        Vector3 mostRightPoint = this.transform.position + (2f * spacing * mostRightVector);

        tempStartPositions.Add(middlePoint);
        tempStartPositions.Add(leftPoint);
        tempStartPositions.Add(rightPoint);
        tempStartPositions.Add(mostLeftPoint);
        tempStartPositions.Add(mostRightPoint);

        //Get points near target
        Vector3 oppositeDirection = -direction;
        Vector3 leftVectorNearTarget = Helpers.VectorByRotateAngle(360 - 70, oppositeDirection);
        Vector3 rightVectorNearTarget = Helpers.VectorByRotateAngle(70, oppositeDirection);

        Vector3 middlePointNearTarget = targetPosition;
        Vector3 leftPointNearTarget = targetPosition + (spacing * leftVectorNearTarget);
        Vector3 rightPointNearTarget = targetPosition + (spacing * rightVectorNearTarget);
        Vector3 mostLeftPointNearTarget = targetPosition + (multiplier * spacing * leftVectorNearTarget);
        Vector3 mostRightPointNearTarget = targetPosition + (multiplier * spacing * rightVectorNearTarget);

        //Get directions 
        Vector3 middleDirection = direction;
        Vector3 leftDirection = (leftPointNearTarget - this.transform.position).normalized;
        Vector3 rightDirection = (rightPointNearTarget - this.transform.position).normalized;
        Vector3 mostLeftDirection = (mostLeftPointNearTarget - this.transform.position).normalized;
        Vector3 mostRightDirection = (mostRightPointNearTarget - this.transform.position).normalized;

        tempFighterDirections.Add(middleDirection);
        tempFighterDirections.Add(leftDirection);
        tempFighterDirections.Add(rightDirection);
        tempFighterDirections.Add(mostLeftDirection);
        tempFighterDirections.Add(mostRightDirection);

        return new VectorSet(tempStartPositions, tempFighterDirections);
    }

    public void InitializeFighter(Building targetBuilding, VectorSet fighterDirections)
    {
        lineCapacity = currentFighter > 5 ? lineCapacity = 5 : lineCapacity = (int) currentFighter;

        if (currentFighter > 5)
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i, targetBuilding, fighterDirections);       
            }
            OnChangingNumberOfFighters?.Invoke(currentFighter - lineCapacity);
        }
        else
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i, targetBuilding, fighterDirections);
            }
            OnChangingNumberOfFighters?.Invoke(currentFighter - lineCapacity);
            isMarching = false;
            StartCoroutine(DelayGenerating()); ;    
        }
    }

    public void GetFighterFromPool(int index, Building targetBuilding, VectorSet fighterDirections)
    {
        GameObject newFighter = ObjectPooler.Instance.GetObject(fighterPrefab);
        Fighter fighterStat = newFighter.GetComponent<Fighter>();
        newFighter.transform.position = fighterDirections.startPositions[index];
        fighterStat.SpriteRenderer.color = buildingColor;
        fighterStat.Owner = buildingOwner;
        fighterStat.MoveDirection = fighterDirections.fighterDirections[index];
        fighterStat.TargetBuilding = targetBuilding;
        fighterStat.MoveSpeed = fighterStat.DefaultMoveSpeed;
    }

    IEnumerator DelayMarching(Building targetBuilding, VectorSet fighterDirections)
    {
        WaitForSeconds delayMarchcing = Utilities.GetWaitForSeconds(initializingDelay);
        while(isMarching)
        {
            InitializeFighter(targetBuilding, fighterDirections);
            yield return delayMarchcing;     
        }   
    }

    IEnumerator DelayGenerating()
    {
        WaitForSeconds delayGenerating = Utilities.GetWaitForSeconds(generatingDelay);
        yield return delayGenerating;
        
        if (active)
        {
            isGenerating = true;
        }
    }

    public void DisableBuilding()
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = 0.2f;
        spriteRenderer.color = tempColor;
        circleCollider.enabled = false;
        OnDisableBuilding?.Invoke();
    }

    public void EnableBuilding()
    {
        Color tempColor = spriteRenderer.color;
        tempColor.a = 1f;
        spriteRenderer.color = tempColor; 
        circleCollider.enabled = true;
        OnEnableBuilding?.Invoke(currentFighter);
    }
}
