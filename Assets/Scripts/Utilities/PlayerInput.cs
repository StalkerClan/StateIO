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

    private bool released;
    private bool selected;

    public HashSet<Building> selectedBuildings;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public string TargetID { get => targetID; }
    public bool Released { get => released; set => released = value; }
    public bool Selected { get => selected; set => selected = value; }


    private void Awake()
    {
        selectedBuildings = new HashSet<Building>();
        selectRange = 0.22f;
        released = false;
        selected = false;
    }

    public void SelectTarget()
    {
#if UNITY_EDITOR
        #region Mouse
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.TryGetComponent(out Building building))
                {
                    if (building.Owned)
                    {
                        if (Vector2.Distance(hit.transform.position, hit.point) > selectRange)
                        {
                            building.Selected = true;
                            selectedBuildings.Add(building);

                        }
                        else if (Vector2.Distance(hit.transform.position, hit.point) < selectRange)
                        {
                            building.Selected = false; 
                            selectedBuildings.Remove(building);
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
                    OnSelectedTarget?.Invoke(targetID, targetPosition);
                    Reset();
                }
                else Reset();
            }
            else Reset();
        }
        #endregion
#endif

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
                        if (building.Owned)
                        {
                            if (Vector2.Distance(hit.transform.position, hit.point) > selectRange)
                            {
                                building.Selected = true;
                                selectedBuildings.Add(building);

                            }
                            else if (Vector2.Distance(hit.transform.position, hit.point) < selectRange)
                            {
                                building.Selected = false;
                                selectedBuildings.Remove(building);
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
        selectedBuildings.Clear(); 
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }

    private void Update()
    {
        SelectTarget();
    }
}
