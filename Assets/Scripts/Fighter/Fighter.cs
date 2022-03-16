using UnityEngine;

public class Fighter : MonoBehaviour, IInitializeVariables
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Owner owner;

    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private string targetID;

    [SerializeField] private float moveSpeed;
    private float defaultMoveSpeed;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Owner Owner { get => owner; set => owner = value; }
    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public string TargetID { get => targetID; set => targetID = value; }
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
            if (targetID.Equals(collideDetector.BuildingID))
            {
                Vector3 direction = (collision.transform.position - this.transform.position).normalized;
                moveDirection = direction;
                moveSpeed *= 2f;
            }
        }

        if (collision.TryGetComponent(out Building building))
        {
            if (targetID.Equals(building.BuildingID))
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
