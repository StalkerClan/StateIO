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

    [SerializeField] private List<Arrow> arrows = new List<Arrow>();
    private List<BuildingSelector> prepBuildings = new List<BuildingSelector>();
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
    public List<Arrow> Arrows { get => arrows; set => arrows = value; }
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
        Debug.Log(prepBuildings.Count);
        SelectTarget();
    }

    public void CheckSelectedBuilding(Building building)
    {
        if (hashSetSelectedBuildings.Contains(building))
        {
            //Arrow arrowData = arrows.Find(x => x.SelectedBuilding == building);
            //if (arrowData != null)
            //{
            //    arrowData.DeSpawn();
            //    arrows.Remove(arrowData);
            //}
            prepBuildings.Find(x => x.Building == building);
            hashSetSelectedBuildings.Remove(building);
        }
    }

    public void SelectTarget()
    {
        mousePosition = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000f, layerMask);

        #region MouseInput
        #region MouseDown
        if (Input.GetMouseButton(0))
        {
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
                            if (!buildingSelector.OutOfRadius)
                            {
                                if (!buildingSelector.Visited)
                                {
                                    buildingSelector.Visited = true;
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
                                GenerateSelectorArrow(buildingSelector);
                            }
                        }
                    }

                    else if (!buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        if (hashSetSelectedBuildings.Count > 0)
                        {
                            tempBuilding = buildingSelector.Building;
                            float targetRange = Vector2.Distance(tempBuilding.transform.position, mousePosition.point);
                            if (targetRange < radius)
                            {
                                tempBuilding.CollideDetector.ShowRadius(targetBuildingColor);
                            }
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

                if (prepToRemoveBuilding != null)
                {
                    prepToRemoveBuilding.CollideDetector.HideRadius(selectedBuildingColor);
                }
            }

            CheckArrowStatus();
        }
        #endregion

        #region MouseUp
        if (Input.GetMouseButtonUp(0))
        {
            if (mousePosition.collider != null)
            {
                if (mousePosition.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
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
        #endregion
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
                float distance = Vector2.Distance(mousePosition.point, prepBuildings[i].transform.position);
                if (distance > radius)
                {
                    if (!prepBuildings[i].OutOfRadius)
                    {
                        prepBuildings[i].OutOfRadius = true;
                        prepBuildings[i].Building.CollideDetector.HideRadius(targetBuildingColor);
                        hashSetSelectedBuildings.Add(prepBuildings[i].Building);
                    }
                }
                else
                {
                    
                    if (prepBuildings.Count > 1)
                    {
                        if (prepBuildings[i].OutOfRadius)
                        {
                            if (!prepBuildings[i].Visited)
                            {
                                prepBuildings[i].HasArrow = false;
                                prepBuildings[i].SelectorArrow.DeSpawn();
                                prepBuildings[i].Visited = true;
                            }
                            
                        }
                        prepBuildings[i].Building.CollideDetector.ShowRadius(selectedBuildingColor);
                        hashSetSelectedBuildings.Remove(prepBuildings[i].Building);
                    }
                }
            }
        }
    }

    public void CreateArrow(BuildingSelector buildingSelector)
    {
        if (buildingSelector.SelectorArrow == null)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
            newArrow.transform.position = buildingSelector.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            buildingSelector.SelectorArrow = arrowData;
        }
    }

    public void Reset()
    {

        foreach (BuildingSelector buildingSelector in prepBuildings)
        {
            buildingSelector.SelectorArrow.DeSpawn();
            buildingSelector.SelectorArrow = null;
            buildingSelector.HasArrow = false;
            buildingSelector.OutOfRadius = false;
            buildingSelector.Visited = false;
        }
        prepBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }
}
