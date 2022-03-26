using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public event Action<Building, Vector3> OnSelectedTarget = delegate { };

    [SerializeField] private GameObject ArrowPrefab;

    public List<Building> selectedBuildings;
    private List<Arrow> arrows = new List<Arrow>();
    private HashSet<Building> hashSetPerparingBuildings = new HashSet<Building>();
    private HashSet<Building> hashSetSelectedBuildings = new HashSet<Building>();

    private Color buildingColor;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;

    private float selectRange;
    private bool marched;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public List<Arrow> Arrows { get => arrows; set => arrows = value; }
    public HashSet<Building> HashSetPerparingBuildings { get => hashSetPerparingBuildings; set => hashSetPerparingBuildings = value; }
    public HashSet<Building> HashSetSelectedBuildings { get => hashSetSelectedBuildings; set => hashSetSelectedBuildings = value; }
    public bool Marched { get => marched; set => marched = value; }

    private void Awake()
    {
        hashSetSelectedBuildings = new HashSet<Building>(selectedBuildings);
        selectRange = 0.22f;
        marched = false;
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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out Building building))
                {
                    if (building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        CreateArrow(building);

                        hashSetPerparingBuildings.Add(building);

                        if (Vector2.Distance(hit.transform.position, hit.point) > selectRange)
                        {
                            HideSelectedRange();
                            hashSetSelectedBuildings.Add(building);
                        }
                        else if (Vector2.Distance(hit.transform.position, hit.point) < selectRange)
                        {
                            ShowSelectedRange();
                            hashSetSelectedBuildings.Remove(building);
                        }                        
                    }
                    else if (!building.OwnerType.Equals(GlobalVariables.Owner.Player))
                    {
                        building.CollideDetector.ShowSelectedRange();
                    } 
                }
                else return;
            }
            else return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            HideSelectedRange();
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
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

    public void CreateArrow(Building building)
    {
        if (arrows.Count < hashSetPerparingBuildings.Count)
        {
            GameObject newArrow = ObjectPooler.Instance.GetObject(ArrowPrefab);
            newArrow.transform.position = building.transform.position;
            Arrow arrowData = newArrow.GetComponent<Arrow>();
            arrowData.SelectedBuilding = building;
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
        hashSetPerparingBuildings.Clear();
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }

    public void ShowSelectedRange()
    {
        foreach (Building prefBuilding in hashSetPerparingBuildings)
        {
            prefBuilding.CollideDetector.ShowSelectedRange();
        }
    }

    public void HideSelectedRange()
    {
        foreach (Building prefBuilding in hashSetPerparingBuildings)
        {
            prefBuilding.CollideDetector.HideSelectedRange();
        }
    }
}
