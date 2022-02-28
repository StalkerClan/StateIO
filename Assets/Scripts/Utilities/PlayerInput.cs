using System;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public event Action<string, Vector3> OnMarching = delegate { };    

    public Vector3 startPosition;
    public Vector3 targetPosition;
    private Vector3 distance;
    private Vector3 direction;

    private string targetID;

    private bool released;
    private bool selected;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public string TargetID { get => targetID; }
    public bool Released { get => released; set => released = value; }
    public bool Selected { get => selected; set => selected = value; }


    private void Awake()
    {
        released = false;
        selected = false;
    }

    public void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag(GlobalVariables.PLAYER_BUILDING_TAG) ||
                    hit.transform.CompareTag(GlobalVariables.NEUTRAL_BUILDING_TAG))
                    if (hit.transform.gameObject.TryGetComponent(out Building building))
                    {
                        if (building.Owned)
                        {
                            building.Selected = true;
                        }
                    }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            released = true;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform.CompareTag(GlobalVariables.NEUTRAL_BUILDING_TAG) ||
                hit.transform.CompareTag(GlobalVariables.OPPONENT_BUILDING_TAG))
            {
                if (hit.transform.gameObject.TryGetComponent(out Building building))
                {
                    targetPosition = hit.transform.gameObject.transform.position;
                    targetID = building.BuildingID;
                    OnMarching?.Invoke(targetID, targetPosition);
                }
            }
            released = false;
            ResetTarget();
        }
    }

    public void ResetTarget()
    {
        startPosition = Vector3.zero;
        targetPosition = Vector3.zero;
    }

    private void Update()
    {
        SelectTarget();
    }
}
