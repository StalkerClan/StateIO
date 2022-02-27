using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour, IInitializeVariables
{
    public GameObject fighterHolder;
    public Rigidbody2D rb;
    public GlobalVariables.Fighter fighter;
    public Vector3[] positions;
    private Vector3 moveDirection;
    public float moveSpeed;
    private bool moving = false;
    private int positionIndex;

    public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public Vector3[] Positions { get => positions; set => positions = value; }

    public void InitializeVariables()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetFighter()
    {
        switch (fighter)
        {
            case GlobalVariables.Fighter.Player:
                moveSpeed = 5f;
                break;
            case GlobalVariables.Fighter.Opponent:
                moveSpeed = 1f;
                break;
            default:
                break;
        }
    }

    public void March(Vector3[] points)
    {
        positionIndex = 0;
        positions = points;
        moving = true;
    }

    public void Marching()
    {
        if (moving)
        {
            transform.position = positions[positionIndex];
            positionIndex++;
            if (positionIndex >= positions.Length)
                moving = false;
        }
    }

    public void Move()
    {
        rb.MovePosition(transform.position + moveDirection * Time.deltaTime * moveSpeed);
    }
}
