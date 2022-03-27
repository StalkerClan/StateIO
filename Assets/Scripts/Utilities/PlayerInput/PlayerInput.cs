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
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;

    private float selectRange;
    private bool marched;

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
        selectRange = 0.9f;
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
                    //float distance = Vector2.Distance(buildingSelector.transform.position, hit.point);
                    if (buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        hashSetPerparingBuildings.Add(buildingSelector);
                        Debug.Log("hashSetPerparingBuildings.Count" + hashSetPerparingBuildings.Count);
                        if (arrows.Count < hashSetPerparingBuildings.Count)
                        {
                            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
                            newArrow.transform.position = buildingSelector.transform.position;
                            Arrow arrowData = newArrow.GetComponent<Arrow>();
                            arrowData.SelectedBuilding = buildingSelector.Building;
                            arrows.Add(arrowData);
                            Debug.Log("arrows.Count" + arrows.Count);
                        }
                        ShowSelectedRange();

                        for (int i = 0; i < arrows.Count; i++)
                        {
                            if (arrows[i].Distance > selectRange)
                            {
                                hashSetSelectedBuildings.Add(arrows[i].SelectedBuilding);
                                HideSelectedRange();
                            }
                            else if (arrows[i].Distance < selectRange)
                            {
                                if (arrows.Count > 1)
                                {
                                    hashSetPerparingBuildings.Remove(buildingSelector);
                                    arrows[i].DeSpawn();
                                    arrows.Remove(arrows[i]);
                                }
                                else
                                {
                                    hashSetSelectedBuildings.Remove(arrows[i].SelectedBuilding);
                                }
                            }
                        }
                    }

                    if (!buildingSelector.Building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        targetBuilding = buildingSelector.Building;
                        float targetRange = Vector2.Distance(targetBuilding.transform.position, hit.point);
                        if (targetRange > selectRange)
                        {
                            targetBuilding.CollideDetector.HideSelectedRange(targetBuildingColor);
                        }
                        else if (targetRange < selectRange)
                        {
                            targetBuilding.CollideDetector.ShowSelectedRange(targetBuildingColor);
                        }
                    }
                }
                else return;
            }
            else return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (targetBuilding != null)
            {
                targetBuilding.CollideDetector.HideSelectedRange(targetBuildingColor);
            }
            HideSelectedRange();
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000f, layerMask);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out BuildingSelector buildingSelector))
                {
                    targetPosition = buildingSelector.Building.transform.position;
                    foreach (Building selectedBuilding in hashSetSelectedBuildings)
                    {
                        selectedBuilding.FighterMarching(buildingSelector.Building, targetPosition);
                    }
                    marched = true;
                    Reset();
                }
                else Reset();
            }
            else Reset();
        }
        #endregion

#if UNITY_EDITOR
        #region Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out Building building))
                    {
                        if (building.OwnerType.Equals(GlobalVariables.Owner.Player))
                        {
                            if (Vector2.Distance(hit.transform.position, hit.point) > selectRange)
                            {
                                hashSetSelectedBuildings.Add(building);

                            }
                            else if (Vector2.Distance(hit.transform.position, hit.point) < selectRange)
                            {
                                hashSetSelectedBuildings.Remove(building);
                            }
                        }
                        else return;
                    }
                    else return;
                }
                else return;
            }          

            if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out Building building))
                    {
                        targetPosition = hit.transform.gameObject.transform.position;
                        foreach (Building selectedBuilding in hashSetSelectedBuildings)
                        {
                            selectedBuilding.FighterMarching(building, targetPosition);
                        }
                        marched = true;
                        Reset();
                    }
                    else Reset();
                }
                else Reset();
            }
        }
        #endregion
#endif
    }

    public Arrow CreateArrow(Building building)
    {
        if (arrows.Count < hashSetPerparingBuildings.Count)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(arrowPrefab.gameObject);
            newArrow.transform.position = building.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            arrowData.SelectedBuilding = building;
            arrows.Add(arrowData);
            return arrowData;
        }
        else return null;
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
        hashSetPerparingBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }

    public void ShowSelectedRange()
    {
        foreach (BuildingSelector buildingSelector in hashSetPerparingBuildings)
        {
            buildingSelector.Building.CollideDetector.ShowSelectedRange(selectedBuildingColor);
        }
    }

    public void HideSelectedRange()
    {
        foreach (BuildingSelector buildingSelector in hashSetPerparingBuildings)
        {
            buildingSelector.Building.CollideDetector.HideSelectedRange(selectedBuildingColor);
        }
    }
}
