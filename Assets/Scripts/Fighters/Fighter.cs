using UnityEngine;

public abstract class Fighter : MonoBehaviour, IInitializeVariables
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public Building buildingOwner;
    public GlobalVariables.Fighter fighter;

    protected Vector3 moveDirection;
    public Vector3[] positions;

    public string targetID;

    public float defaultMoveSpeed;
    public float moveSpeed;

    private int positionIndex;

    private bool moving = false;

    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public Vector3[] Positions { get => positions; set => positions = value; }
    public Building BuildingOwner { get => buildingOwner; set => buildingOwner = value; }
    public string TargetID { get => targetID; set => targetID = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    public abstract void InitializeVariables();

    public void March(Vector3[] points)
    {
        positionIndex = 0;
        positions = points;
        moving = true;
    }

    public void CheckCurrentPosition()
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
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * moveDirection);
    }
}
