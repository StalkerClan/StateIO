using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInitializeVariables, ISubcriber, IReceiver
{

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

    public event Action<Owner> OnChangingOnwer = delegate { };
    public event Action<int> OnChangingNumberOfFighters = delegate { };

    [SerializeField] private GameObject fighterPrefab;

    [SerializeField] private Owner defaultOwner;
    [SerializeField] private Owner buildingOwner;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Color buildingColor;

    [SerializeField] private GlobalVariables.Owner ownerType;
    [SerializeField] private GlobalVariables.Owner defaultOwnerType;

    private Dictionary<string, VectorSet> directionDictionary;

    [SerializeField] private List<Building> nearbyBuildings;

    [SerializeField] private string buildingID;

    [SerializeField] private float timeToGenerate;
    [SerializeField] private float initializingDelay;
    [SerializeField] private float generatingDelay;
    [SerializeField] private float multiplier;
    private float degree;
    private float timeSinceGenerated;
    private float spacing;
   
    [SerializeField] private int startFighter;
    [SerializeField] private int maxCapacity;
    [SerializeField] private int currentFighter;
    protected int lineCapacity;

    [SerializeField] private bool isGenerating;
    [SerializeField] private bool owned;
    [SerializeField] private bool taken;
    [SerializeField] private bool isMarching;
    [SerializeField] private bool idle;

    public Owner BuildingOwner { get => buildingOwner; set => buildingOwner = value; }
    public Owner DefaultOwner { get => defaultOwner; set => defaultOwner = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }
    public GlobalVariables.Owner DefaultOwnerType { get => defaultOwnerType; set => defaultOwnerType = value; }
    public List<Building> NearbyBuildings { get => nearbyBuildings; set => nearbyBuildings = value; }
    public string BuildingID { get => buildingID; set => buildingID = value; }
    public float TimeToGenerate { get => timeToGenerate; set => timeToGenerate = value; }
    public int MaxCapacity { get => maxCapacity; set => maxCapacity = value; }
    public int CurrentFighter { get => currentFighter; set => currentFighter = value; }
    public bool IsGenerating { get => isGenerating; set => isGenerating = value; }
    public bool Owned { get => owned; set => owned = value; }
    public bool Taken { get => taken; set => taken = value; }
    public bool IsMarching { get => isMarching; set => isMarching = value; }
    public bool Idle { get => idle; set => idle = value; }

    private void Start()
    {
        idle = false;
        buildingOwner = defaultOwner;
        ownerType = defaultOwnerType;
        SetOwner(defaultOwner, defaultOwner.OwnerType);
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    private void Update()
    {        
        GenerateFighter();
    }

    public void SubcribeEvent()
    {
        OnChangingNumberOfFighters += ChangeNumberOfFighter;
        OnChangingOnwer += StopSpawningFighter;
        buildingOwner.OnChangingColorSet += ChangeBuildingColor;
    }

    public void UnsubcribeEvent()
    {
        OnChangingNumberOfFighters -= ChangeNumberOfFighter;
        OnChangingOnwer -= StopSpawningFighter;
        buildingOwner.OnChangingColorSet -= ChangeBuildingColor;
    }

    public void InitializeVariables()
    {      
        directionDictionary = new Dictionary<string, VectorSet>();    
        spacing = 0.1f;
        multiplier = 2f;
        degree = 100f;
        initializingDelay = 0.5f;
        generatingDelay = 1.5f;
        startFighter = buildingOwner.OwnerStat.startFighter;
        currentFighter = startFighter;
        isGenerating = false;
        isMarching = false; 
    }

    public void GetBuildingStats(Owner owner)
    {
        this.buildingOwner = owner;
        spriteRenderer.sprite = owner.OwnerStat.buildingIcon;
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(owner.OwnerStat.colorSet.keyColor));
        ownerType = owner.OwnerType;
        spriteRenderer.color = buildingColor;
        timeToGenerate = owner.OwnerStat.timeToGenerate;
        maxCapacity = owner.OwnerStat.maxCapacity;     
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

    public void SetOwner(Owner owner, GlobalVariables.Owner owerType)
    {
        GetBuildingStats(owner);
        GetOwnerType(owerType);
        InitializeVariables();
        idle = true;
        OnChangingNumberOfFighters?.Invoke(owner.ownerStat.startFighter);
        OnChangingOnwer?.Invoke(owner);
    }

    public void ChangeBuildingColor(ColorSet newColorSet)
    {
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(newColorSet.keyColor));
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
        if (timeSinceGenerated >= timeToGenerate)
        {
            OnChangingNumberOfFighters?.Invoke(currentFighter + 1);
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

    public void ChangeNumberOfFighter(int fighter)
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
                    LevelManager.Instance.PlayerBuilding++;
                }
                else 
                {
                    owned = false;
                    taken = true;
                    if (this.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        LevelManager.Instance.PlayerBuilding--;
                    }
                }
                OnChangingOnwer?.Invoke(invader);
                this.buildingOwner.RemoveBuilding(this);
                invader.AddBuilding(this);
                LevelManager.Instance.CheckWinCondition();                 
            }
            else
            {
                OnChangingNumberOfFighters?.Invoke(currentFighter - fighter);
            }
            if (idle) return;
            StartCoroutine(DelayGenerating());
        }
    }

    public void FighterMarching(string currentTargetID, Vector3 currentTargetPosition)
    {
        isMarching = true;
        isGenerating = false;
        Vector3 direction = (currentTargetPosition - this.transform.position).normalized;

        if (directionDictionary.TryGetValue(currentTargetID, out VectorSet existedfighterDirecions))
        {
            if (idle) return;
            StartCoroutine(DelayMarching(currentTargetID, existedfighterDirecions));
        }
        else
        {
            VectorSet vectorSet = GetVectorSet(currentTargetPosition);
            if (idle) return;
            StartCoroutine(DelayMarching(currentTargetID, vectorSet));
            directionDictionary.Add(currentTargetID, vectorSet);
        }
    }

    public VectorSet GetVectorSet(Vector3 targetPosition)
    {  
        List<Vector3> tempStartPositions = new List<Vector3>();
        List<Vector3> tempFighterDirections = new List<Vector3>();

        Vector3 direction = (targetPosition - this.transform.position).normalized;

        //Form arc formation
        Vector3 leftVector = Helpers.VectorByRotateAngle(degree, direction);
        Vector3 rightVector = Helpers.VectorByRotateAngle(360 - degree, direction);

        Vector3 middlePoint = this.transform.position;
        Vector3 leftPoint = this.transform.position + (spacing * leftVector);
        Vector3 rightPoint = this.transform.position + (spacing * rightVector);
        Vector3 mostLeftPoint = this.transform.position + (2f * spacing * leftVector);
        Vector3 mostRightPoint = this.transform.position + (2f * spacing * rightVector);

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

    public void InitializeFighter(string currentTargetID, VectorSet fighterDirections)
    {

        lineCapacity = currentFighter > 5 ? lineCapacity = 5 : lineCapacity = currentFighter;

        if (currentFighter > 5)
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i, currentTargetID, fighterDirections);       
            }
            OnChangingNumberOfFighters?.Invoke(currentFighter - lineCapacity);
        }
        else
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i, currentTargetID, fighterDirections);
            }
            OnChangingNumberOfFighters?.Invoke(currentFighter - lineCapacity);
            isMarching = false;
            if (idle) return;
            StartCoroutine(DelayGenerating());
        }
    }

    public void GetFighterFromPool(int index, string targetID, VectorSet fighterDirections)
    {
        GameObject newFighter = ObjectPooler.Instance.GetObject(fighterPrefab);
        Fighter fighterStat = newFighter.GetComponent<Fighter>();
        newFighter.transform.position = fighterDirections.startPositions[index];
        fighterStat.SpriteRenderer.color = buildingColor;
        fighterStat.Owner = buildingOwner;
        fighterStat.MoveDirection = fighterDirections.fighterDirections[index];
        fighterStat.TargetID = targetID;
        fighterStat.MoveSpeed = fighterStat.DefaultMoveSpeed;
    }

    IEnumerator DelayMarching(string currentTargetID, VectorSet fighterDirections)
    {
        WaitForSeconds delayMarchcing = Utilities.GetWaitForSeconds(initializingDelay);
        while(isMarching)
        {
            InitializeFighter(currentTargetID, fighterDirections);
            yield return delayMarchcing;     
        }       
    }

    IEnumerator DelayGenerating()
    {
        WaitForSeconds delayGenerating = Utilities.GetWaitForSeconds(generatingDelay);
        yield return delayGenerating;

        isGenerating = true;
    }
}
