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

    [SerializeField] private List<BuildingSelector> prepBuildings = new List<BuildingSelector>();
    private HashSet<Building> hashSetSelectedBuildings = new HashSet<Building>();
    private Building targetBuilding;
    private Building prepToRemoveBuilding;
    private Building tempBuilding;
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;

    private RaycastHit2D mousePosition;

    [SerializeField] private float radius;
    private bool marched;
    private bool setted;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public List<BuildingSelector> HashSetPerparingBuildings { get => prepBuildings; set => prepBuildings = value; }
    public HashSet<Building> HashSetSelectedBuildings { get => hashSetSelectedBuildings; set => hashSetSelectedBuildings = value; }
    public bool Marched { get => marched; set => marched = value; }

    private void Awake()
    {
        selectedBuildingColor = arrowPrefab.SpriteRenderer.color;
        targetBuildingColor = arrowPrefab.TargetBuildingColor;
    }

    private void Update()
    {
        SelectTarget();
    }

    public void CheckSelectedBuilding(Building building)
    {
        BuildingSelector selector = prepBuildings.Find(x => x.Building == building);
        if (selector)
        {
            selector.ArrowInstance.DeSpawn();
            selector.ArrowInstance = null;
            prepBuildings.Remove(selector);
            hashSetSelectedBuildings.Remove(building);
        }
    }

    public void SelectTarget()
    {
        #region MouseInput
        #region MouseDown
        if (Input.GetMouseButton(0))
        {
            CastRayCast();
            if (mousePosition.collider != null)
            {
                if (mousePosition.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
                    if (!setted)
                    {
                        radius = buildingSelector.Col.radius;
                        setted = true;
                    }

                    if (buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        if (!buildingSelector.HasArrow)
                        {
                            if (!buildingSelector.Deselected)
                            {
                                if (!buildingSelector.OutOfRadius)
                                {
                                    GenerateSelectorArrow(buildingSelector);
                                }
                                else
                                {
                                    prepToRemoveBuilding = buildingSelector.Building;
                                    prepToRemoveBuilding.CollideDetector.ShowRadius(selectedBuildingColor);
                                }
                            }
                            else
                            {
                                buildingSelector.OutOfRadius = false;
                            }
                        }
                    }
                    else
                    {
                        if (tempBuilding == null)
                        {
                            tempBuilding = buildingSelector.Building;
                            tempBuilding.CollideDetector.ShowRadius(targetBuildingColor);
                        }                   
                    }
                }
                else RemoveTempBuilding();    
            }
            else RemoveTempBuilding();
            CheckArrowStatus();
        }
        #endregion

        #region MouseUp
        if (Input.GetMouseButtonUp(0))
        {
            CastRayCast();
            if (mousePosition.collider != null)
            {
                if (mousePosition.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
                    if (prepBuildings.Count > 0)
                    {
                        targetBuilding = buildingSelector.Building;
                        targetBuilding.CollideDetector.HideRadius(targetBuildingColor);
                        targetPosition = targetBuilding.transform.position;
                        foreach (BuildingSelector selector in prepBuildings)
                        {
                            if (selector.HasArrow)
                            {
                                selector.Building.FighterMarching(targetBuilding, targetPosition);
                            }
                        }
                        marched = true;
                    }
                }
            }
            Reset();
        }
        #endregion
        #endregion
    }

    private void RemoveTempBuilding()
    {
        if (tempBuilding != null)
        {
            tempBuilding.CollideDetector.HideRadius(targetBuildingColor);
            tempBuilding = null;
        }
    }

    public void CastRayCast()
    {
        mousePosition = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000f, layerMask);
    }

    private void GenerateSelectorArrow(BuildingSelector buildingSelector)
    {
        buildingSelector.HasArrow = true;
        if (!prepBuildings.Contains(buildingSelector))
        {
            prepBuildings.Add(buildingSelector);
        } 
        CreateArrow(buildingSelector);
        buildingSelector.Building.CollideDetector.ShowRadius(selectedBuildingColor);
    }

    private void CheckArrowStatus()
    {
        if (prepBuildings.Count > 0)
        {
            for (int i = 0; i < prepBuildings.Count; i++)
            {
                if (Vector2.Distance(mousePosition.point, prepBuildings[i].transform.position) > radius)
                {                 
                    if (prepBuildings[i].HasArrow)
                    {
                        if (prepBuildings[i].Deselected)
                        {
                            prepBuildings[i].OutOfRadius = false;
                        }
                        else
                        {
                            prepBuildings[i].OutOfRadius = true;
                        }
                    }
                    else
                    {
                        prepBuildings[i].Deselected = false;
                    }
                    prepBuildings[i].Building.CollideDetector.HideRadius(targetBuildingColor);
                }
                else 
                {
                    if (prepBuildings.Count > 1)
                    {
                        if (prepBuildings[i].OutOfRadius)
                        {
                            prepBuildings[i].HasArrow = false; 
                            prepBuildings[i].Deselected = true;
                            prepBuildings[i].OutOfRadius = false;
                            prepBuildings[i].ArrowInstance.DeSpawn();
                            prepBuildings[i].ArrowInstance = null;
                        } 
                    }
                    prepBuildings[i].Building.CollideDetector.ShowRadius(selectedBuildingColor);
                }
            }
        }
    }

    public void CreateArrow(BuildingSelector buildingSelector)
    {
        if (buildingSelector.ArrowInstance == null)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
            newArrow.transform.position = buildingSelector.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            buildingSelector.ArrowInstance = arrowData;
        }
    }

    public void Reset()
    {

        foreach (BuildingSelector buildingSelector in prepBuildings)
        {
            if (buildingSelector.ArrowInstance != null)
                buildingSelector.ArrowInstance.DeSpawn();
            buildingSelector.ArrowInstance = null;
            buildingSelector.HasArrow = false;
            buildingSelector.Deselected = false;
            buildingSelector.OutOfRadius = false;
            buildingSelector.Building.CollideDetector.HideRadius(selectedBuildingColor);
        }
        prepBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }
}
