using UnityEngine;

public class PlayerFighter : Fighter
{
    private void Start()
    {
        InitializeVariables();
    }
    public override void InitializeVariables()
    {
        defaultMoveSpeed = owner.OwnerStat.defaultMoveSpeed;
        moveSpeed = defaultMoveSpeed;
    }

    private void FixedUpdate()
    {
        Move();
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

        if (collision.TryGetComponent(out EnemyFighter enemyFighter))
        {
            DeSpawn();
            enemyFighter.DeSpawn();
        }
    }
}
