using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public event Action<string, Vector3> OnSelectedTarget = delegate { };

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;

    private string targetID;

    public float selectRange;


    public List<Building> selectedBuildings;
    private HashSet<Building> hashSetSelectedBuildings;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public string TargetID { get => targetID; }



    private void Awake()
    {
        hashSetSelectedBuildings = new HashSet<Building>(selectedBuildings);
        selectRange = 0.22f;
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

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out Building building))
                {
                    targetPosition = hit.transform.gameObject.transform.position;
                    targetID = building.BuildingID;
                    foreach (Building selectedBuilding in hashSetSelectedBuildings)
                    {
                        selectedBuilding.FighterMarching(targetID, targetPosition);
                    }                  
                    Reset();
                }
                else Reset();
            }
            else Reset();
        }
        #endregion

        #region Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
                       
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

            if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.TryGetComponent(out Building building))
                    {
                        targetPosition = hit.transform.gameObject.transform.position;
                        targetID = building.BuildingID;
                        OnSelectedTarget?.Invoke(targetID, targetPosition);
                        Reset();
                    }
                    else Reset();
                }
                else Reset();
            }
        }

            
        #endregion
    }

    public void Reset()
    {
        hashSetSelectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }

    private void Update()
    {
        SelectTarget();
    }
}
