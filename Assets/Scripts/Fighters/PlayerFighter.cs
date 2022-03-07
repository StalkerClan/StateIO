using UnityEngine;

public class PlayerFighter : Fighter
{
    private void Start()
    {
        InitializeVariables();
    }
    public override void InitializeVariables()
    {
        defaultMoveSpeed = 4f;
        moveSpeed = defaultMoveSpeed;
    }

    private void Update()
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
                    receiver.ReceiveFighter(1, this.buildingOwner);
                    DeSpawn();
                }
            }
        }    
    }

    private void DeSpawn()
    {
        moveDirection = Vector3.zero;
        moveSpeed = defaultMoveSpeed;
        ObjectPooler.Instance.ReturnGameObject(this.gameObject);
    }
}
