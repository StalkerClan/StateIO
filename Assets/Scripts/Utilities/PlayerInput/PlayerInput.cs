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
    private HashSet<BuildingSelector> hashSetPerparingBuildings = new HashSet<BuildingSelector>();
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
    public HashSet<BuildingSelector> HashSetPerparingBuildings { get => hashSetPerparingBuildings; set => hashSetPerparingBuildings = value; }
    public HashSet<Building> HashSetSelectedBuildings { get => hashSetSelectedBuildings; set => hashSetSelectedBuildings = value; }
    public bool Marched { get => marched; set => marched = value; }

    private void Awake()
    {
        selectedBuildingColor = arrowPrefab.SpriteRenderer.color;
        targetBuildingColor = arrowPrefab.TargetBuildingColor;
    }

    private void Update()
    {
        Debug.Log(hashSetPerparingBuildings.Count);
        SelectTarget();
    }

    public void CheckSelectedBuilding(Building building)
    {
        if (hashSetSelectedBuildings.Contains(building))
        {
            Arrow arrowData = arrows.Find(x => x.SelectedBuilding == building);
            if (arrowData != null)
            {
                arrowData.DeSpawn();
                arrows.Remove(arrowData);
            }
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
                        if (buildingSelector.OutOfRadius)
                        {
                            if (!buildingSelector.HasArrow)
                            {
                                buildingSelector.OutOfRadius = false;
                                GenerateSelectorArrow(buildingSelector);
                            }
                        }

                        if (!buildingSelector.HasArrow)
                        {
                            if (buildingSelector.OutOfRadius)
                            {
                                prepToRemoveBuilding = buildingSelector.Building;
                                prepToRemoveBuilding.CollideDetector.ShowRadius(selectedBuildingColor);
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
        hashSetPerparingBuildings.Add(buildingSelector);
        CreateArrow(buildingSelector);
        buildingSelector.Building.CollideDetector.ShowRadius(selectedBuildingColor);
    }

    private void CheckArrowStatus()
    {
        if (hashSetPerparingBuildings.Count > 0)
        {
            foreach (BuildingSelector selector in hashSetPerparingBuildings)
            {
                float distance = Vector2.Distance(mousePosition.point, selector.transform.position);
                if (distance > radius)
                {
                    if (!selector.OutOfRadius)
                    {
                        selector.OutOfRadius = true;
                        hashSetSelectedBuildings.Add(selector.Building);
                        selector.Building.CollideDetector.HideRadius(targetBuildingColor);
                    }
                }
                else
                {
                    if (hashSetSelectedBuildings.Count > 1)
                    {
                        if (selector.OutOfRadius)
                        {
                            if (selector.HasArrow)
                            {
                                selector.HasArrow = false;
                                selector.SelectorArrow.DeSpawn();
                                if (selector.SelectorArrow == null)
                                {
                                    arrows.Remove(selector.SelectorArrow);
                                }
                            }                                            
                        }
                    }
                    selector.Building.CollideDetector.ShowRadius(selectedBuildingColor);
                    hashSetSelectedBuildings.Remove(selector.Building);
                }
            }
        }
    }

    public void CreateArrow(BuildingSelector buildingSelector)
    {
        if (arrows.Count < hashSetPerparingBuildings.Count)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
            newArrow.transform.position = buildingSelector.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            buildingSelector.SelectorArrow = arrowData;
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
            buildingSelector.HasArrow = false;
            buildingSelector.OutOfRadius = false;
        }
        hashSetPerparingBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }
}
