using System;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
   
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 distance;
    private Vector2 direction;

    private bool released;
    private bool selected;

    public Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector2 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public bool Released { get => released; set => released = value; }
    public bool Selected { get => selected; set => selected = value; }

    private void Awake()
    {
        released = false;
        selected = false;
    }

    public void GetDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            released = false;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag(GlobalVariables.PLAYER_BUILDING_TAG))
                {
                    startPosition = hit.transform.gameObject.transform.position;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            released = true;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag(GlobalVariables.NEUTRAL_BUILDING_TAG) ||
                    hit.transform.CompareTag(GlobalVariables.OPPONENT_BUILDING_TAG))
                {
                    selected = true;
                    targetPosition = hit.transform.gameObject.transform.position;
                }
            }
            selected = false;
        }
    }
}
