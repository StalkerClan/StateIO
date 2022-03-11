using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : Fighter
{
    private void Start()
    {
        InitializeVariables();
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
    }
}