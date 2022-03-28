using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public event Action<Building, Vector3> OnSelectedTarget = delegate { };

    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Color selectedBuildingColor;
    [SerializeField] private Color targetBuildingColor;
    [SerializeField] private LayerMask layerMask;

    public List<Building> selectedBuildings;
    private List<Arrow> arrows = new List<Arrow>();
    private HashSet<Vector2> hashSetBuildingPosition = new HashSet<Vector2>();
    private HashSet<BuildingSelector> hashSetPerparingBuildings = new HashSet<BuildingSelector>();
    private HashSet<Building> hashSetSelectedBuildings = new HashSet<Building>();
    public Dictionary<Building, Arrow> arrowSet = new Dictionary<Building, Arrow>();
    private Building targetBuilding;
    private Building prefToRemoveBuilding;
    private Building tempBuilding;
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;

    [SerializeField] private float radius;
    private bool marched;
    private bool setted;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public List<Arrow> Arrows { get => arrows; set => arrows = value; }
    public HashSet<BuildingSelector> HashSetPerparingBuildings { get => hashSetPerparingBuildings; set => hashSetPerparingBuildings = value; }
    public HashSet<Building> HashSetSelectedBuildings { get => hashSetSelectedBuildings; set => hashSetSelectedBuildings = value; }
    public bool Marched { get => marched; set => marched = value; }

    private void Awake()
    {
        hashSetSelectedBuildings = new HashSet<Building>(selectedBuildings);
        marched = false;
        selectedBuildingColor = arrowPrefab.SpriteRenderer.color;
        targetBuildingColor = arrowPrefab.TargetBuildingColor;
    }

    private void Update()
    {
        SelectTarget();
    }

    public void CheckSelectedBuilding(Building building)
    {
        if (hashSetSelectedBuildings.Contains(building))
        {
            hashSetSelectedBuildings.Remove(building);
            Arrow arrowData = arrows.Find(x => x.SelectedBuilding == building);
            if (arrowData != null)
            {
                arrowData.DeSpawn();
                arrows.Remove(arrowData);
            }
        }
    }

    public void SelectTarget()
    {
        #region Mouse
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000f, layerMask);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
                    if (!setted)
                    {
                        radius = buildingSelector.Col.radius;
                        setted = true;
                    }
                    
                    if (buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        if (!buildingSelector.OutOfRadius)
                        {
                            hashSetPerparingBuildings.Add(buildingSelector);
                            CreateArrow(buildingSelector);
                            buildingSelector.Building.CollideDetector.ShowRadius(selectedBuildingColor);
                        }
                        else
                        {
                            prefToRemoveBuilding = buildingSelector.Building;
                            buildingSelector.Building.CollideDetector.ShowRadius(selectedBuildingColor);
                        }
                    }

                    else if (!buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        tempBuilding = buildingSelector.Building;
                        float targetRange = Vector2.Distance(tempBuilding.transform.position, hit.point);
                        if (targetRange < radius)
                        {
                            tempBuilding.CollideDetector.ShowRadius(targetBuildingColor);
                        }
                    }
                }
            }
            else
            {
                if (tempBuilding != null)
                {
                    tempBuilding.CollideDetector.HideRadius(targetBuildingColor);
                }
                if (prefToRemoveBuilding != null)
                {
                    prefToRemoveBuilding.CollideDetector.HideRadius(selectedBuildingColor);
                }
            }

            if (arrows.Count >= 1)
            {
                for (int i = 0; i < arrows.Count; i++)
                {
                    if (arrows[i].Distance > radius)
                    {
                        arrows[i].Selector.OutOfRadius = true;
                        hashSetSelectedBuildings.Add(arrows[i].Selector.Building);
                        arrows[i].SelectedBuilding.CollideDetector.HideRadius(targetBuildingColor);
                    }
                    else if (arrows[i].Distance < radius)
                    {;
                        if (arrows[i].Selector.OutOfRadius)
                        {
                            arrows[i].SelectedBuilding.CollideDetector.ShowRadius(targetBuildingColor);
                            if (arrows.Count > 1)
                            {
                                Arrow temp = arrows[i];
                                temp.DeSpawn();
                                arrows.Remove(arrows[i]);
                                hashSetPerparingBuildings.Remove(arrows[i].Selector);
                                hashSetSelectedBuildings.Remove(arrows[i].Selector.Building);
                            }
                        }
                        hashSetSelectedBuildings.Remove(arrows[i].Selector.Building);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (BuildingSelector selector in hashSetPerparingBuildings)
            {
                selector.Building.CollideDetector.HideRadius(selectedBuildingColor);
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000f, layerMask);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
                    Debug.Log(hashSetSelectedBuildings.Count);
                    if (hashSetSelectedBuildings != null)
                    {
                        targetBuilding = buildingSelector.Building;
                        targetBuilding.CollideDetector.HideRadius(targetBuildingColor);
                        targetPosition = targetBuilding.transform.position;
                        foreach (Building selectedBuilding in hashSetSelectedBuildings)
                        {
                            selectedBuilding.FighterMarching(targetBuilding, targetPosition);
                        }
                        marched = true;
                    }
                }
            }
            Reset();
        }
        #endregion
    }

    public void CreateArrow(BuildingSelector buildingSelector)
    {
        if (arrows.Count < hashSetPerparingBuildings.Count)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
            newArrow.transform.position = buildingSelector.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            arrowData.Selector = buildingSelector;
            arrowData.SelectedBuilding = buildingSelector.Building;
            arrows.Add(arrowData);
        }
    }

    public void Reset()
    {
        if (arrows != null)
        {
            foreach (Arrow arrow in arrows)
            {
                arrow.DeSpawn();
            }
        }   
        arrows.Clear();
        foreach (BuildingSelector buildingSelector in hashSetPerparingBuildings)
        {
            buildingSelector.OutOfRadius = false;
        }
        hashSetPerparingBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }
}
