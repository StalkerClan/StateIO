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


    [SerializeField] private Owner buildingOwner;

    [SerializeField] private GameObject fighterPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Color buildingColor;

    [SerializeField] private GlobalVariables.Owner ownerType;

    private Dictionary<string, VectorSet> directionDictionary;

    [SerializeField] private List<Building> nearbyBuildings;

    [SerializeField] private string buildingID;

    [SerializeField] private float timeToGenerate;
    [SerializeField] private float initializingDelay;
    [SerializeField] private float generatingDelay;
    [SerializeField] private float multiplier;
    [SerializeField] private float radiusCheck;
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

    public Owner BuildingOwner { get => buildingOwner; set => buildingOwner = value; }
    public GameObject FighterPrefab { get => fighterPrefab; set => fighterPrefab = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color BuildingColor { get => buildingColor; set => buildingColor = value; }
    public GlobalVariables.Owner OwnerType { get => ownerType; set => ownerType = value; }
    public List<Building> NearbyBuildings { get => nearbyBuildings; set => nearbyBuildings = value; }
    public string BuildingID { get => buildingID; set => buildingID = value; }
    public float TimeToGenerate { get => timeToGenerate; set => timeToGenerate = value; }
    public float RadiusCheck { get => radiusCheck; set => radiusCheck = value; }
    public int MaxCapacity { get => maxCapacity; set => maxCapacity = value; }
    public int CurrentFighter { get => currentFighter; set => currentFighter = value; }
    public bool Owned { get => owned; set => owned = value; }
    public bool Taken { get => taken; set => taken = value; }

    private void Start()
    {
        InitializeVariables();
        GetOwnerType();
        SubcribeEvent();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }

    private void Update()
    {
        GenerateFighter();
        if (!isMarching)
        {
            CancelInvoke("InitializeFighter");
        }
    }

    public void SubcribeEvent()
    {
        OnChangingNumberOfFighters += ChangeNumberOfFighter;
    }

    public void UnsubcribeEvent()
    {
        OnChangingNumberOfFighters -= ChangeNumberOfFighter;
    }

    public void InitializeVariables()
    {      
        directionDictionary = new Dictionary<string, VectorSet>();    
        spacing = 0.13f;
        multiplier = 2f;
        degree = 100f;
        initializingDelay = 0.4f;
        generatingDelay = 1.5f;
        radiusCheck = 4f;
        startFighter = buildingOwner.OwnerStat.startFighter;
        currentFighter = startFighter;
        isGenerating = true;
        isMarching = false;
    }

    public void GetBuildingStats(Owner owner)
    {
        this.buildingOwner = owner;
        if (owner.FighterPrefab != null)
        {
            fighterPrefab = owner.FighterPrefab;
        }
        spriteRenderer.sprite = owner.OwnerStat.buildingIcon;
        buildingColor = Utilities.HexToColor(Utilities.ColorToHex(owner.OwnerStat.colorSet.keyColor));
        ownerType = owner.OwnerType;
        spriteRenderer.color = buildingColor;
        timeToGenerate = owner.OwnerStat.timeToGenerate;
        maxCapacity = owner.OwnerStat.maxCapacity;     
    }

    public void GetOwnerType()
    {
        switch (ownerType)
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
    public void ReceiveFighter(int fighter, Owner invader)
    {
        GlobalVariables.Owner invaderType = invader.OwnerType;
        if (ownerType.Equals(invaderType))
        {
            OnChangingNumberOfFighters?.Invoke(fighter);
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
                else if (invader.OwnerType.Equals(GlobalVariables.Owner.Enemy))
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
        isMarching = true;
        Vector3 direction = (currentTargetPosition - this.transform.position).normalized;

        if (directionDictionary.TryGetValue(currentTargetID, out VectorSet existedfighterDirecions))
        {
            StartCoroutine(DelayMarching(currentTargetID, existedfighterDirecions));
        }
        else
        {
            VectorSet vectorSet = GetVectorSet(currentTargetPosition);
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
        Vector3 mostLeftPoint = this.transform.position + (spacing * leftVector);
        Vector3 mostRightPoint = this.transform.position + (spacing * rightVector);
        Vector3 leftPoint = Vector3.Lerp(this.transform.position, mostLeftPoint, 0.6f);
        Vector3 rightPoint = Vector3.Lerp(this.transform.position, mostRightPoint, 0.6f);

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
            OnChangingNumberOfFighters?.Invoke(-5);
        }
        else
        {
            for (int i = 0; i < lineCapacity; i++)
            {
                GetFighterFromPool(i, currentTargetID, fighterDirections);
            }
            OnChangingNumberOfFighters?.Invoke(-1 * currentFighter);          
            isMarching = false;
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
        fighterStat.TargetID = targetID;
        fighterStat.MoveDirection = fighterDirections.fighterDirections[index];
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
