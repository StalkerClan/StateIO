using UnityEngine;

public abstract class Fighter : MonoBehaviour, IInitializeVariables
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    public Owner owner;
    public GlobalVariables.Owner fighterOwner;

    protected Vector3 moveDirection;

    public string targetID;

    public float moveSpeed;
    protected float defaultMoveSpeed;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Owner Owner { get => owner; set => owner = value; }
    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public string TargetID { get => targetID; set => targetID = value; }

    public abstract void InitializeVariables();

    public void Move()
    {
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * moveDirection);
    }

    public void DeSpawn()
    {
        moveDirection = Vector3.zero;
        moveSpeed = defaultMoveSpeed;
        ObjectPooler.Instance.ReturnGameObject(this.gameObject);
    }
}
