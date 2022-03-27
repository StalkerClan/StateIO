using UnityEngine;

public class Fighter : MonoBehaviour, IInitializeVariables
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Owner owner;

    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private Building targetBuilding;

    [SerializeField] private float moveSpeed;
    private float defaultMoveSpeed;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Owner Owner { get => owner; set => owner = value; }
    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public Building TargetBuilding { get => targetBuilding; set => targetBuilding = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DefaultMoveSpeed { get => defaultMoveSpeed; set => defaultMoveSpeed = value; }

    private void Start()
    {
        InitializeVariables();
        ObjectPooler.Instance.OnDeSpawningAllFighters += DeSpawn;
    }

    private void OnDisable()
    {
        ObjectPooler.Instance.OnDeSpawningAllFighters -= DeSpawn;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void InitializeVariables()
    {
        defaultMoveSpeed = owner.OwnerStat.DefaultMoveSpeed;
        moveSpeed = defaultMoveSpeed;
    }

    public void Move()
    {
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * moveDirection);
    }

    public void DeSpawn()
    {
        ObjectPooler.Instance.ReturnGameObject(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CollideDetector collideDetector))
        {
            if (targetBuilding.Equals(collideDetector.Building))
            {
                Vector3 direction = (collision.transform.position - this.transform.position).normalized;
                moveDirection = direction;
            }
        }

        if (collision.TryGetComponent(out Building building))
        {
            if (targetBuilding.Equals(building))
            {
                IReceiver receiver = collision.GetComponent<IReceiver>();
                if (receiver != null)
                {
                    receiver.ReceiveFighter(1, owner);
                    DeSpawn();
                }
            }
        }

        if (collision.TryGetComponent(out Fighter fighter))
        {
            if (this.owner != fighter.Owner)
            {
                DeSpawn();
                fighter.DeSpawn();
            }
        }
    }
}
